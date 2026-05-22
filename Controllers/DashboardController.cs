using AeroFuelHub.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Admin()
    {
        return View();
    }

    [Authorize(Roles = Roles.AirlineExecutive)]
    public IActionResult Airline()
    {
        return View();
    }

    [Authorize(Roles = Roles.FuelSupplyExecutive)]
    public IActionResult FuelSupply()
    {
        return View();
    }

    [Authorize(Roles = Roles.FuelCoordinator)]
    public IActionResult Coordinator()
    {
        return View();
    }
}