using System.ComponentModel.DataAnnotations;
using comp2147.Models;

namespace COMP2139___assignment2.Models;

public class Rating
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    [Range(1,5)]
    public int Stars { get; set; }

    public string? Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}