using comp2147.Models;

namespace COMP2139___assignment2.Models;

public class TicketType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int QuantitySold { get; set; }
}