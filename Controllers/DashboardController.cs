using AeroFuelHub.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AeroFuelHub.Web.Data;
using Microsoft.EntityFrameworkCore;
using AeroFuelHub.Web.ViewModels.Dashboard;

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