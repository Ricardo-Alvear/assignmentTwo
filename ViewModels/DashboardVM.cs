using COMP2139___assignment2.Models;
using comp2147.Models;

namespace comp2147.ViewModels;

public class DashboardVM
{
    public ApplicationUser? User { get; set; } = null!;
    public List<OrderItem> MyTickets { get; set; } = new();
    public List<Order> PurchaseHistory { get; set; } = new();
    public List<Event> MyEvents { get; set; } = new();
    public decimal TotalRevenue { get; set; }

    // Search functionality
    public string SearchTerm { get; set; } = string.Empty;
    public List<Event> SearchResults { get; set; } = new();

    public List<Event> AllEvents { get; set; } = new();
}