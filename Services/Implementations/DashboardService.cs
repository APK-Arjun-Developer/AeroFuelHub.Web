using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AeroFuelHub.Web.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardService(IDashboardRepository dashboardRepository, UserManager<ApplicationUser> userManager)
    {
        _dashboardRepository = dashboardRepository;
        _userManager = userManager;
    }

    public async Task<AdminDashboardViewModel> GetAdminDashboardAsync()
    {
        var monthlyData = await _dashboardRepository.GetMonthlyTransactionCountsAsync();
        return new AdminDashboardViewModel
        {
            TotalTransactions = await _dashboardRepository.GetTotalTransactionsAsync(),
            TotalAirlines = await _dashboardRepository.GetTotalAirlinesAsync(),
            TotalFuelCompanies = await _dashboardRepository.GetTotalFuelCompaniesAsync(),
            TotalAirports = await _dashboardRepository.GetTotalAirportsAsync(),
            TotalRevenue = await _dashboardRepository.GetTotalRevenueAsync(),
            TotalFuelQuantity = await _dashboardRepository.GetTotalFuelQuantityAsync(),
            ChartLabels = monthlyData.Select(x => new DateTime(1, x.Month, 1).ToString("MMM")).ToList(),
            ChartData = monthlyData.Select(x => x.Count).ToList(),
            RecentTransactions = await BuildRecentTransactionsAsync(_dashboardRepository.QueryRoleTransactions(), 5)
        };
    }

    public async Task<RoleDashboardViewModel?> GetAirlineDashboardAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user?.AirlineId == null) return null;
        var query = _dashboardRepository.QueryRoleTransactions().Where(x => x.AirlineId == user.AirlineId);
        return await BuildRoleDashboardAsync(query, user.Airline?.Name ?? "Airline Dashboard");
    }

    public async Task<RoleDashboardViewModel?> GetFuelSupplyDashboardAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user?.FuelCompanyId == null) return null;
        var query = _dashboardRepository.QueryRoleTransactions().Where(x => x.FuelCompanyId == user.FuelCompanyId);
        return await BuildRoleDashboardAsync(query, user.FuelCompany?.Name ?? "Fuel Company Dashboard");
    }

    public async Task<RoleDashboardViewModel?> GetCoordinatorDashboardAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user?.AirportId == null) return null;
        var query = _dashboardRepository.QueryRoleTransactions().Where(x => x.AirportId == user.AirportId);
        return await BuildRoleDashboardAsync(query, user.Airport?.Name ?? "Airport Dashboard");
    }

    private static async Task<List<RecentTransactionViewModel>> BuildRecentTransactionsAsync(IQueryable<Models.Entities.FuelTransaction> query, int take)
        => await query.OrderByDescending(x => x.TransactionDate).Take(take).Select(x => new RecentTransactionViewModel
        {
            TransactionNumber = x.TransactionNumber,
            Airline = x.Airline!.Name,
            TotalAmount = x.TotalAmount,
            TransactionDate = x.TransactionDate
        }).ToListAsync();

    private static async Task<RoleDashboardViewModel> BuildRoleDashboardAsync(IQueryable<Models.Entities.FuelTransaction> query, string title)
    {
        return new RoleDashboardViewModel
        {
            Title = title,
            TotalTransactions = await query.CountAsync(),
            TotalFuelQuantity = await query.SumAsync(x => (decimal?)x.FuelQuantity) ?? 0,
            TotalRevenue = await query.SumAsync(x => (decimal?)x.TotalAmount) ?? 0,
            RecentTransactions = await BuildRecentTransactionsAsync(query, 5)
        };
    }
}
