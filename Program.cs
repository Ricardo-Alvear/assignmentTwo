using COMP2139___assignment2.Data;
using COMP2139___assignment2.Models;
using COMP2139___assignment2.Services;
using comp2147.data;
using comp2147.Services;
using comp2147.Services.QrCode;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// SERILOG
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();



// DATABASE
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conn));

// IDENTITY
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Index";  
    options.AccessDeniedPath = "/Home/Index"; 
    options.SlidingExpiration = true;
});

// AUTH POLICIES
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireOrganizerOrAdmin", policy =>
        policy.RequireRole("Organizer", "Admin"));
});

// SERVICES
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.AddSingleton<QrCodeService>();
builder.Services.AddScoped<QrCodeService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// START APP ON DASHBOARD
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// DATABASE SEEDING
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        // Seed roles
        await RoleSeeder.SeedRolesAndAdminAsync(services);

        // Seed default user if none exists
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (!await db.Users.AnyAsync())
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "defaultuser@example.com",
                Email = "defaultuser@example.com",
                FullName = "Default User",
                DateOfBirth = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), // UTC
                EmailConfirmed = true
            };

            await userManager.CreateAsync(defaultUser, "Password123!");
            await userManager.AddToRoleAsync(defaultUser, "Attendee");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error seeding database.");
    }
}

app.Run();