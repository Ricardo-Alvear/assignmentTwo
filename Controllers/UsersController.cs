using COMP2139___assignment2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139___assignment2.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }
}