using AeroFuelHub.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AeroFuelHub.Web.Data;
using Microsoft.EntityFrameworkCore;
using AeroFuelHub.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Identity;
using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;

        _userManager = userManager;
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Admin()
    {
        var monthlyData =
            await _context.FuelTransactions
            .Where(x => !x.IsDeleted)
            .GroupBy(x => new
            {
                x.TransactionDate.Year,
                x.TransactionDate.Month
            })
            .Select(x => new
            {
                Month =
                    x.Key.Month,

                Count =
                    x.Count()
            })
            .OrderBy(x => x.Month)
            .ToListAsync();

        var model =
            new AdminDashboardViewModel
            {
                TotalTransactions =
                    await _context.FuelTransactions
                    .CountAsync(x => !x.IsDeleted),

                TotalAirlines =
                    await _context.Airlines.CountAsync(),

                TotalFuelCompanies =
                    await _context.FuelCompanies.CountAsync(),

                TotalAirports =
                    await _context.Airports.CountAsync(),

                TotalRevenue =
                    await _context.FuelTransactions
                    .Where(x => !x.IsDeleted)
                    .SumAsync(x =>
                        (decimal?)x.TotalAmount) ?? 0,

                TotalFuelQuantity =
                    await _context.FuelTransactions
                    .Where(x => !x.IsDeleted)
                    .SumAsync(x =>
                        (decimal?)x.FuelQuantity) ?? 0,

                ChartLabels =
                    monthlyData
                    .Select(x =>
                        new DateTime(
                            1,
                            x.Month,
                            1).ToString("MMM"))
                    .ToList(),

                ChartData =
                    monthlyData
                    .Select(x => x.Count)
                    .ToList(),

                RecentTransactions =
                    await _context.FuelTransactions
                    .Include(x => x.Airline)
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x =>
                        x.TransactionDate)
                    .Take(5)
                    .Select(x =>
                        new RecentTransactionViewModel
                        {
                            TransactionNumber =
                                x.TransactionNumber,

                            Airline =
                                x.Airline!.Name,

                            TotalAmount =
                                x.TotalAmount,

                            TransactionDate =
                                x.TransactionDate
                        })
                    .ToListAsync()
            };

        return View(model);
    }

    [Authorize(Roles = Roles.AirlineExecutive)]

    [Authorize(Roles = Roles.AirlineExecutive)]
    public async Task<IActionResult> Airline()
    {
        var user =
            await _userManager.GetUserAsync(User);

        var query =
            _context.FuelTransactions
            .Include(x => x.Airline)
            .Where(x =>
                !x.IsDeleted &&
                x.AirlineId == user!.AirlineId);

        var model =
            new RoleDashboardViewModel
            {
                Title =
                    user.Airline?.Name ??
                    "Airline Dashboard",

                TotalTransactions =
                    await query.CountAsync(),

                TotalFuelQuantity =
                    await query.SumAsync(x =>
                        (decimal?)x.FuelQuantity) ?? 0,

                TotalRevenue =
                    await query.SumAsync(x =>
                        (decimal?)x.TotalAmount) ?? 0,

                RecentTransactions =
                    await query
                    .OrderByDescending(x =>
                        x.TransactionDate)
                    .Take(5)
                    .Select(x =>
                        new RecentTransactionViewModel
                        {
                            TransactionNumber =
                                x.TransactionNumber,

                            Airline =
                                x.Airline!.Name,

                            TotalAmount =
                                x.TotalAmount,

                            TransactionDate =
                                x.TransactionDate
                        })
                    .ToListAsync()
            };

        return View("RoleDashboard", model);
    }

    [Authorize(Roles = Roles.FuelSupplyExecutive)]
    public async Task<IActionResult> FuelSupply()
    {
        var user =
            await _userManager.GetUserAsync(User);

        var query =
            _context.FuelTransactions
            .Include(x => x.FuelCompany)
            .Include(x => x.Airline)
            .Where(x =>
                !x.IsDeleted &&
                x.FuelCompanyId ==
                user!.FuelCompanyId);

        var model =
            new RoleDashboardViewModel
            {
                Title =
                    user.FuelCompany?.Name ??
                    "Fuel Company Dashboard",

                TotalTransactions =
                    await query.CountAsync(),

                TotalFuelQuantity =
                    await query.SumAsync(x =>
                        (decimal?)x.FuelQuantity) ?? 0,

                TotalRevenue =
                    await query.SumAsync(x =>
                        (decimal?)x.TotalAmount) ?? 0,

                RecentTransactions =
                    await query
                    .OrderByDescending(x =>
                        x.TransactionDate)
                    .Take(5)
                    .Select(x =>
                        new RecentTransactionViewModel
                        {
                            TransactionNumber =
                                x.TransactionNumber,

                            Airline =
                                x.Airline!.Name,

                            TotalAmount =
                                x.TotalAmount,

                            TransactionDate =
                                x.TransactionDate
                        })
                    .ToListAsync()
            };

        return View("RoleDashboard", model);
    }

    [Authorize(Roles = Roles.FuelCoordinator)]
    public async Task<IActionResult> Coordinator()
    {
        var user =
            await _userManager.GetUserAsync(User);

        var query =
            _context.FuelTransactions
            .Include(x => x.Airport)
            .Include(x => x.Airline)
            .Where(x =>
                !x.IsDeleted &&
                x.AirportId ==
                user!.AirportId);

        var model =
            new RoleDashboardViewModel
            {
                Title =
                    user.Airport?.Name ??
                    "Airport Dashboard",

                TotalTransactions =
                    await query.CountAsync(),

                TotalFuelQuantity =
                    await query.SumAsync(x =>
                        (decimal?)x.FuelQuantity) ?? 0,

                TotalRevenue =
                    await query.SumAsync(x =>
                        (decimal?)x.TotalAmount) ?? 0,

                RecentTransactions =
                    await query
                    .OrderByDescending(x =>
                        x.TransactionDate)
                    .Take(5)
                    .Select(x =>
                        new RecentTransactionViewModel
                        {
                            TransactionNumber =
                                x.TransactionNumber,

                            Airline =
                                x.Airline!.Name,

                            TotalAmount =
                                x.TotalAmount,

                            TransactionDate =
                                x.TransactionDate
                        })
                    .ToListAsync()
            };

        return View("RoleDashboard", model);
    }
}