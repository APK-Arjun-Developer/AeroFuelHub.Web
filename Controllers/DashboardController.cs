using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Admin() => View(await _dashboardService.GetAdminDashboardAsync());

    [Authorize(Roles = Roles.AirlineExecutive)]
    public async Task<IActionResult> Airline()
    {
        var model = await _dashboardService.GetAirlineDashboardAsync(User);
        if (model == null) return NotFound();
        return View("RoleDashboard", model);
    }

    [Authorize(Roles = Roles.FuelSupplyExecutive)]
    public async Task<IActionResult> FuelSupply()
    {
        var model = await _dashboardService.GetFuelSupplyDashboardAsync(User);
        if (model == null) return NotFound();
        return View("RoleDashboard", model);
    }

    [Authorize(Roles = Roles.FuelCoordinator)]
    public async Task<IActionResult> Coordinator()
    {
        var model = await _dashboardService.GetCoordinatorDashboardAsync(User);
        if (model == null) return NotFound();
        return View("RoleDashboard", model);
    }
}
