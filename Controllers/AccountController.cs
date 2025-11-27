using COMP2139___assignment2.Models;
using COMP2139___assignment2.ViewModels.Account;
using COMP2139___assignment2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139___assignment2.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: Registration page
    [HttpGet]
    public IActionResult Register() => View(new RegisterVM());

    // POST: Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            DateOfBirth = model.DateOfBirth
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "Attendee");

        // No email confirmation — just log them in
        await _signInManager.SignInAsync(user, false);

        return RedirectToAction("Index", "Dashboard");
    }

    // GET: Login page
    [HttpGet]
    public IActionResult Login(string? returnUrl = null) =>
        View(new LoginVM { ReturnUrl = returnUrl });

    // POST: Login
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
            return LocalRedirect(model.ReturnUrl ?? "/Dashboard/Index");

        ModelState.AddModelError("", "Invalid login.");
        return View(model);
    }

    // POST: Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}