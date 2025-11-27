using COMP2139___assignment2.Data;
using comp2147.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139___assignment2.Controllers;

[Authorize(Policy = "RequireOrganizerOrAdmin")]

public class AnalyticsController : Controller
{
    private readonly ApplicationDbContext _db;
    public AnalyticsController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public IActionResult MyAnalytics() => View();

    [HttpGet]
    public async Task<IActionResult> SalesByCategory()
    {
        var data = await _db.Events
            .Include(e => e.Category)
            .GroupBy(e => e.Category!.Name)
            .Select(g => new { Category = g.Key, TicketsSold = g.Sum(e => e.TicketsSold) })
            .ToListAsync();
        return Json(data);
    }
    [HttpGet]
    public async Task<IActionResult> RevenuePerMonth()
    {
        var now = DateTime.UtcNow;
        var data = await _db.Orders
            .Where(o => o.EventDate >= now.AddMonths(-12))
            .GroupBy(o => new { o.EventDate.Year, o.EventDate.Month })
            .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Revenue = g.Sum(o => o.Total) })
            .ToListAsync();

        return Json(data);
    }

    [HttpGet]
    public async Task<IActionResult> Top5Events()
    {
        var data = await _db.Events
            .OrderByDescending(e => e.TicketsSold)
            .Take(5)
            .Select(e => new { e.Id, e.Title, e.TicketsSold })
            .ToListAsync();
        return Json(data);
    }
}
