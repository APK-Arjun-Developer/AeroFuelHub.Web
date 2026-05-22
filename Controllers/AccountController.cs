using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");

            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!,
            model.Password,
            model.RememberMe,
            false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid email or password");

            return View(model);
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains(Roles.Admin))
            return RedirectToAction("Admin", "Dashboard");

        if (roles.Contains(Roles.AirlineExecutive))
            return RedirectToAction("Airline", "Dashboard");
    
        if (roles.Contains(Roles.FuelSupplyExecutive))
            return RedirectToAction("FuelSupply", "Dashboard");

        if (roles.Contains(Roles.FuelCoordinator))
            return RedirectToAction("Coordinator", "Dashboard");

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}