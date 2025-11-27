using comp2147.Models;

namespace COMP2139___assignment2.Models;

public class EventCategory
{
   
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Event> Events { get; set; } = new();
    
}