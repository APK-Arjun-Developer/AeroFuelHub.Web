using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AeroFuelHub.Web.Models;

namespace AeroFuelHub.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        if (User.Identity != null &&
            User.Identity.IsAuthenticated)
        {
            return RedirectToAction(
                "Admin",
                "Dashboard");
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
