using COMP2139___assignment2.Models;

namespace COMP2139___assignment2.ViewModels;

public class CartVm
{
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public int TotalQuantity => Items.Sum(i => i.Quantity);
    public decimal TotalPrice => Items.Sum(i => i.Quantity * i.UnitPrice);
}