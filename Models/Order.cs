namespace COMP2139___assignment2.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public DateTime EventDate { get; set; }
    public decimal Total { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
    public ApplicationUser User { get; set; } = default!;
}