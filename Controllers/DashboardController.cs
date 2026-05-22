using AeroFuelHub.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AeroFuelHub.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Admin()
    {
        ViewBag.TotalTransactions =
            await _context.FuelTransactions.CountAsync();

        ViewBag.TotalAirlines =
            await _context.Airlines.CountAsync();

        ViewBag.TotalFuelCompanies =
            await _context.FuelCompanies.CountAsync();

        ViewBag.TotalAirports =
            await _context.Airports.CountAsync();

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