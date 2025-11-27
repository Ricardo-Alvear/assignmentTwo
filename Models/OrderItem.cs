using System.ComponentModel.DataAnnotations.Schema;
using comp2147.Models;

namespace COMP2139___assignment2.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public int TicketTypeId { get; set; }
    public TicketType TicketType { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    [NotMapped] public decimal TotalPrice => Quantity * UnitPrice;
}