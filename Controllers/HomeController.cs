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
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string term = "")
        {
            var user = await _userManager.GetUserAsync(User);

            var allEventsQuery = _db.Events.Include(e => e.TicketTypes).Include(e => e.Ratings);

            var searchResults = !string.IsNullOrWhiteSpace(term)
                ? await allEventsQuery
                    .Where(e => e.Title.ToLower().Contains(term.ToLower()) 
                                || e.Description.ToLower().Contains(term.ToLower()))
                    .ToListAsync()
                : new List<Event>();

            var vm = new DashboardVM
            {
                User = user,
                AllEvents = await allEventsQuery.ToListAsync(),
                SearchResults = searchResults,
                SearchTerm = term
            };

            return View(vm);
        }
    }
}