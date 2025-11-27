using COMP2139___assignment2.Data;
using COMP2139___assignment2.Models;
using comp2147.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comp2147.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await db.Users.ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> ManageRoles(string id)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoles(string userId, string[] roles)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) return NotFound();
        
        // remove all current roles
        var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);

        await userManager.AddToRolesAsync(user, roles);
        return RedirectToAction("Index");
    }
    
    
}