using COMP2139___assignment2.Models;
using Microsoft.AspNetCore.Identity;

namespace COMP2139___assignment2.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = new[] { "Admin", "Organizer", "Attendee" };

        foreach (var r in roles)
        {
            if (!await roleManager.RoleExistsAsync(r))
            {
                await roleManager.CreateAsync(new IdentityRole(r));
            }
        }

        var adminEmail = "admin@event.local";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
                { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, FullName = "Admin of Site" };
            var result = await userManager.CreateAsync(admin, "Admin555");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }




    }
}