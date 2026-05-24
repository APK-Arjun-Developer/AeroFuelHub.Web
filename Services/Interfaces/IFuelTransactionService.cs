using System.Security.Claims;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.FuelTransaction;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IFuelTransactionService
{
    Task<CreateFuelTransactionViewModel> BuildCreateViewModelAsync(ClaimsPrincipal user);
    Task<(bool Success, string? ErrorKey, string? ErrorMessage)> CreateTransactionAsync(CreateFuelTransactionViewModel model, ClaimsPrincipal user);
    Task<FuelTransactionHistoryViewModel> GetHistoryAsync(string? search, int page, ClaimsPrincipal user);
    Task<FuelTransactionDetailsViewModel?> GetDetailsAsync(int id, ClaimsPrincipal user);
    Task<FuelTransaction?> GetInvoiceDataAsync(int id, ClaimsPrincipal user);
    Task<List<FuelTransaction>> GetReportsAsync(DateTime? startDate, DateTime? endDate, ClaimsPrincipal user);
    Task<List<FuelTransaction>> GetExcelExportDataAsync(DateTime? startDate, DateTime? endDate, ClaimsPrincipal user);
    Task<bool> SoftDeleteAsync(int id, ClaimsPrincipal user);
}
