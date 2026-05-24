using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<List<(int Month, int Count)>> GetMonthlyTransactionCountsAsync();
    Task<int> GetTotalTransactionsAsync();
    Task<int> GetTotalAirlinesAsync();
    Task<int> GetTotalFuelCompaniesAsync();
    Task<int> GetTotalAirportsAsync();
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetTotalFuelQuantityAsync();
    IQueryable<FuelTransaction> QueryRoleTransactions();
}
