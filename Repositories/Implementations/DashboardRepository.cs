using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Repositories.Implementations;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<(int Month, int Count)>> GetMonthlyTransactionCountsAsync() =>
        (await _context.FuelTransactions.Where(x => !x.IsDeleted)
        .GroupBy(x => new { x.TransactionDate.Month })
        .Select(x => new { x.Key.Month, Count = x.Count() })
        .OrderBy(x => x.Month).ToListAsync())
        .Select(x => (x.Month, x.Count)).ToList();

    public Task<int> GetTotalTransactionsAsync() => _context.FuelTransactions.CountAsync(x => !x.IsDeleted);
    public Task<int> GetTotalAirlinesAsync() => _context.Airlines.CountAsync();
    public Task<int> GetTotalFuelCompaniesAsync() => _context.FuelCompanies.CountAsync();
    public Task<int> GetTotalAirportsAsync() => _context.Airports.CountAsync();
    public async Task<decimal> GetTotalRevenueAsync() => await _context.FuelTransactions.Where(x => !x.IsDeleted).SumAsync(x => (decimal?)x.TotalAmount) ?? 0;
    public async Task<decimal> GetTotalFuelQuantityAsync() => await _context.FuelTransactions.Where(x => !x.IsDeleted).SumAsync(x => (decimal?)x.FuelQuantity) ?? 0;

    public IQueryable<FuelTransaction> QueryRoleTransactions() => _context.FuelTransactions
        .Include(x => x.Airline)
        .Include(x => x.FuelCompany)
        .Include(x => x.Airport)
        .Where(x => !x.IsDeleted)
        .AsQueryable();
}
