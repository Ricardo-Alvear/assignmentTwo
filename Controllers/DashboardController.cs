using COMP2139___assignment2.Data;
using COMP2139___assignment2.Models;
using COMP2139___assignment2.ViewModels;
using comp2147.data;
using comp2147.Models;
using comp2147.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp2147.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Dashboard
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm = null)
        {
            var user = await _userManager.GetUserAsync(User);

            // Base query for all events
            var eventsQuery = _db.Events
                .Include(e => e.TicketTypes)
                .Include(e => e.Ratings)
                .AsQueryable();

            // Filter search results if searchTerm is provided
            var searchResults = !string.IsNullOrWhiteSpace(searchTerm)
                ? await eventsQuery
                    .Where(e => e.Title.ToLower().Contains(searchTerm.ToLower())
                             || e.Description.ToLower().Contains(searchTerm.ToLower()))
                    .ToListAsync()
                : new List<Event>();

            // Organizer’s own events
            var myEvents = (user != null && (await _userManager.IsInRoleAsync(user, "Organizer") 
                                            || await _userManager.IsInRoleAsync(user, "Admin")))
                ? await _db.Events
                    .Where(e => e.OrganizerId == user.Id)
                    .Include(e => e.TicketTypes)
                    .ToListAsync()
                : new List<Event>();

            var vm = new DashboardVM
            {
                User = user,
                SearchTerm = searchTerm ?? string.Empty,
                SearchResults = searchResults,
                AllEvents = await eventsQuery.ToListAsync(),
                MyEvents = myEvents,
                MyTickets = user != null 
                    ? await _db.Orders
                        .Where(o => o.UserId == user.Id && o.EventDate >= DateTime.UtcNow)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Event)
                        .SelectMany(o => o.OrderItems)
                        .ToListAsync()
                    : new List<OrderItem>(),
                PurchaseHistory = user != null
                    ? await _db.Orders
                        .Where(o => o.UserId == user.Id && o.EventDate < DateTime.UtcNow)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Event)
                        .ToListAsync()
                    : new List<Order>()
            };

            return View(vm);
        }
    }
}