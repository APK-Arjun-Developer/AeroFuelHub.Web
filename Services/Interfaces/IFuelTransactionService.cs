using System.Security.Claims;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.FuelTransaction;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IFuelTransactionService
{
    Task<CreateFuelTransactionViewModel> BuildCreateViewModelAsync(ClaimsPrincipal user);
    Task<(bool Success, string? ErrorKey, string? ErrorMessage)> CreateTransactionAsync(CreateFuelTransactionViewModel model, ClaimsPrincipal user);
    Task<List<FuelTransaction>> GetHistoryAsync(string? search, ClaimsPrincipal user);
    Task<FuelTransaction?> GetDetailsAsync(int id, ClaimsPrincipal user);
    Task<FuelTransaction?> GetInvoiceDataAsync(int id, ClaimsPrincipal user);
    Task<List<FuelTransaction>> GetReportsAsync(DateTime? startDate, DateTime? endDate, ClaimsPrincipal user);
    Task<List<FuelTransaction>> GetExcelExportDataAsync(DateTime? startDate, DateTime? endDate, ClaimsPrincipal user);
    Task<List<FuelTransaction>> GetDashboardTransactionsAsync(ClaimsPrincipal user, int take);
    Task<bool> SoftDeleteAsync(int id, ClaimsPrincipal user);
}
