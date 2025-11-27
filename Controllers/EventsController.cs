using COMP2139___assignment2.Models;
using comp2147.data;
using comp2147.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp2147.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Events/Manage
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);

            var events = await _db.Events
                .Where(e => e.OrganizerId == user.Id)
                .ToListAsync();

            return View(events);
        }

        // GET: /Events/Create
        public IActionResult Create()
        {
            return View(new Event());
        }

        // POST: /Events/Create
        [HttpPost]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            model.OrganizerId = user?.Id ?? "guest";

            _db.Events.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); // redirect to dashboard / home
        }

        // GET: /Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        // POST: /Events/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Event model)
        {
            var ev = await _db.Events.FindAsync(model.Id);
            if (ev == null) return NotFound();

            ev.Title = model.Title;
            ev.StartDate = model.StartDate;
            ev.Description = model.Description;

            await _db.SaveChangesAsync();
            return RedirectToAction("Manage");
        }

        // GET: /Events/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound();

            _db.Events.Remove(ev);
            await _db.SaveChangesAsync();

            return RedirectToAction("Manage");
        }
        
        // GET: /Events
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var eventsList = await _db.Events
                .Include(e => e.TicketTypes)
                .ToListAsync();

            return View(eventsList);
        }

// GET: /Events/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _db.Events
                .Include(e => e.TicketTypes)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            return View(ev);
        }
    }
}