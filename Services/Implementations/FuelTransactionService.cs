using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Enums;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.FuelTransaction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AeroFuelHub.Web.Services.Implementations;

public class FuelTransactionService : IFuelTransactionService
{
    private readonly IFuelTransactionRepository _fuelTransactionRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public FuelTransactionService(IFuelTransactionRepository fuelTransactionRepository, UserManager<ApplicationUser> userManager)
    {
        _fuelTransactionRepository = fuelTransactionRepository;
        _userManager = userManager;
    }

    public async Task<CreateFuelTransactionViewModel> BuildCreateViewModelAsync(ClaimsPrincipal user)
    {
        var model = new CreateFuelTransactionViewModel
        {
            Airlines = await _fuelTransactionRepository.GetAirlinesAsync(),
            Aircrafts = await _fuelTransactionRepository.GetAircraftsAsync(),
            FuelCompanies = await _fuelTransactionRepository.GetFuelCompaniesAsync()
        };

        var currentUser = await _userManager.GetUserAsync(user);
        if (user.IsInRole(Roles.Admin))
            model.Airports = await _fuelTransactionRepository.GetAirportsAsync();
        else if (user.IsInRole(Roles.FuelCoordinator) && currentUser?.AirportId != null)
        {
            model.Airports = await _fuelTransactionRepository.GetAirportsByIdAsync(currentUser.AirportId.Value);
            model.AirportId = currentUser.AirportId;
        }

        return model;
    }

    public async Task<(bool Success, string? ErrorKey, string? ErrorMessage)> CreateTransactionAsync(CreateFuelTransactionViewModel model, ClaimsPrincipal user)
    {
        var currentUser = await _userManager.GetUserAsync(user);

        if (user.IsInRole(Roles.Admin) && !model.AirportId.HasValue)
            return (false, "AirportId", "Airport is required");

        if (user.IsInRole(Roles.FuelCoordinator))
            model.AirportId = currentUser?.AirportId;

        if (!model.AirportId.HasValue)
            return (false, "AirportId", "Airport is required");

        var transaction = new FuelTransaction
        {
            TransactionNumber = GenerateTransactionNumber(),
            TransactionDate = DateTime.UtcNow,
            AirlineId = model.AirlineId,
            AircraftId = model.AircraftId,
            AirportId = model.AirportId.Value,
            FuelCompanyId = model.FuelCompanyId,
            FlightNumber = model.FlightNumber,
            FuelQuantity = model.FuelQuantity,
            PricePerLiter = model.PricePerLiter,
            TotalAmount = model.FuelQuantity * model.PricePerLiter,
            Remarks = model.Remarks,
            Status = FuelTransactionStatus.Completed,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = user.Identity?.Name
        };

        await _fuelTransactionRepository.AddTransactionAsync(transaction);
        await _fuelTransactionRepository.SaveChangesAsync();
        return (true, null, null);
    }

    public async Task<List<FuelTransaction>> GetHistoryAsync(string? search, ClaimsPrincipal user)
    {
        var query = await GetFilteredTransactionsQueryAsync(user);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.TransactionNumber.Contains(search) || x.FlightNumber.Contains(search));

        return await query.OrderByDescending(x => x.Id).ToListAsync();
    }

    public async Task<FuelTransaction?> GetDetailsAsync(int id, ClaimsPrincipal user)
        => await (await GetFilteredTransactionsQueryAsync(user)).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<FuelTransaction?> GetInvoiceDataAsync(int id, ClaimsPrincipal user)
        => await (await GetFilteredTransactionsQueryAsync(user)).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<FuelTransaction>> GetReportsAsync(DateTime? startDate, DateTime? endDate, ClaimsPrincipal user)
    {
        var query = await GetFilteredTransactionsQueryAsync(user);
        if (startDate.HasValue)
            query = query.Where(x => x.TransactionDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(x => x.TransactionDate <= endDate.Value);
        return await query.OrderByDescending(x => x.TransactionDate).ToListAsync();
    }

    public async Task<List<FuelTransaction>> GetExcelExportDataAsync(ClaimsPrincipal user)
        => await (await GetFilteredTransactionsQueryAsync(user)).ToListAsync();

    public async Task<List<FuelTransaction>> GetDashboardTransactionsAsync(ClaimsPrincipal user, int take)
        => await (await GetFilteredTransactionsQueryAsync(user))
            .OrderByDescending(x => x.TransactionDate)
            .Take(take)
            .ToListAsync();

    public async Task<bool> SoftDeleteAsync(int id, ClaimsPrincipal user)
    {
        var transaction = await _fuelTransactionRepository.GetTransactionByIdAsync(id);
        if (transaction == null)
            return false;

        transaction.IsDeleted = true;
        transaction.DeletedAt = DateTime.UtcNow;
        transaction.DeletedBy = user.Identity?.Name;
        await _fuelTransactionRepository.SaveChangesAsync();
        return true;
    }

    private async Task<IQueryable<FuelTransaction>> GetFilteredTransactionsQueryAsync(ClaimsPrincipal user)
    {
        var currentUser = await _userManager.GetUserAsync(user);
        var roles = await _userManager.GetRolesAsync(currentUser!);

        var query = _fuelTransactionRepository.QueryTransactionsWithIncludes();

        if (roles.Contains(Roles.Admin))
            return query;
        if (roles.Contains(Roles.AirlineExecutive))
            query = query.Where(x => x.AirlineId == currentUser!.AirlineId);
        if (roles.Contains(Roles.FuelSupplyExecutive))
            query = query.Where(x => x.FuelCompanyId == currentUser!.FuelCompanyId);
        if (roles.Contains(Roles.FuelCoordinator))
            query = query.Where(x => x.AirportId == currentUser!.AirportId);

        return query;
    }

    private static string GenerateTransactionNumber()
    {
        var random = new Random();
        return $"FT-{DateTime.Now:yyyyMMdd}-{random.Next(1000, 9999)}";
    }
}
