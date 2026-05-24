using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AeroFuelHub.Web.Models;
using AeroFuelHub.Web.Services.Interfaces;

namespace AeroFuelHub.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class HomeController : Controller
{
    private readonly IAccountService _accountService;

    public HomeController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (User.Identity != null &&
            User.Identity.IsAuthenticated)
        {
            var redirect = await _accountService.GetDashboardRedirectAsync(User);
            return RedirectToAction(redirect.Action, redirect.Controller);
        }

        return RedirectToAction(
            "Login",
            "Account");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}
