using System.ComponentModel.DataAnnotations.Schema;
using COMP2139___assignment2.Models;

namespace comp2147.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string OrganizerId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    
    [ForeignKey(nameof(OrganizerId))]
    public ApplicationUser Organizer { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public EventCategory? Category { get; set; }

    public decimal Price { get; set; }
    public int TotalTickets { get; set; }
    public int TicketsSold { get; set; }
    
    public virtual ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}