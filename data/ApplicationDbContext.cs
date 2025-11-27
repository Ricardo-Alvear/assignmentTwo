using COMP2139___assignment2.Models;
using comp2147.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace comp2147.data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<Rating> Ratings { get; set; }
}