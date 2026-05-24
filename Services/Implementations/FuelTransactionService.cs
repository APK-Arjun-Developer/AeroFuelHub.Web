using System.Security.Claims;
using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Enums;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.FuelTransaction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        var currentUser = await _userManager.GetUserAsync(user);
        var aircraftOptions = await _fuelTransactionRepository.GetAircraftOptionsAsync();

        var model = new CreateFuelTransactionViewModel
        {
            Airlines = await _fuelTransactionRepository.GetAirlinesAsync(),
            AircraftOptions = aircraftOptions,
            FuelCompanies = await _fuelTransactionRepository.GetFuelCompaniesAsync()
        };

        if (user.IsInRole(Roles.AirlineExecutive) && currentUser?.AirlineId != null)
        {
            model.AirlineId = currentUser.AirlineId.Value;
            model.IsAirlineLocked = true;
            model.Airlines = model.Airlines
                .Where(x => x.Value == currentUser.AirlineId.Value.ToString())
                .ToList();
            model.Aircrafts = await _fuelTransactionRepository.GetAircraftsByAirlineAsync(currentUser.AirlineId.Value);
        }

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

        if (user.IsInRole(Roles.AirlineExecutive))
        {
            if (currentUser?.AirlineId == null)
                return (false, "", "Your account is not linked to an airline");
            model.AirlineId = currentUser.AirlineId.Value;
        }

        if (user.IsInRole(Roles.FuelSupplyExecutive))
        {
            if (currentUser?.FuelCompanyId == null)
                return (false, "", "Your account is not linked to a fuel company");
            model.FuelCompanyId = currentUser.FuelCompanyId.Value;
        }

        if (user.IsInRole(Roles.Admin) && !model.AirportId.HasValue)
            return (false, "AirportId", "Airport is required");

        if (user.IsInRole(Roles.FuelCoordinator))
            model.AirportId = currentUser?.AirportId;

        if (!model.AirportId.HasValue)
            return (false, "AirportId", "Airport is required");

        if (!await _fuelTransactionRepository.AircraftBelongsToAirlineAsync(model.AircraftId, model.AirlineId))
            return (false, "AircraftId", "Selected aircraft does not belong to the selected airline");

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

    public async Task<FuelTransactionHistoryViewModel> GetHistoryAsync(string? search, int page, ClaimsPrincipal user)
    {
        page = page < 1 ? 1 : page;
        var pageSize = FuelTransactionHistoryViewModel.DefaultPageSize;

        var query = await GetFilteredTransactionsQueryAsync(user);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.TransactionNumber.Contains(search) || x.FlightNumber.Contains(search));

        query = query.OrderByDescending(x => x.Id);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new FuelTransactionListItemViewModel
            {
                Id = x.Id,
                TransactionNumber = x.TransactionNumber,
                AirlineName = x.Airline!.Name,
                AircraftModel = x.Aircraft!.Model,
                AirportName = x.Airport!.Name,
                FuelCompanyName = x.FuelCompany!.Name,
                FlightNumber = x.FlightNumber,
                TotalAmount = x.TotalAmount,
                TransactionDate = x.TransactionDate
            })
            .ToListAsync();

        return new FuelTransactionHistoryViewModel
        {
            Search = search,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<FuelTransactionDetailsViewModel?> GetDetailsAsync(int id, ClaimsPrincipal user)
    {
        var transaction = await (await GetFilteredTransactionsQueryAsync(user))
            .FirstOrDefaultAsync(x => x.Id == id);
        return transaction == null ? null : MapToDetailsViewModel(transaction);
    }

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

    public Task<List<FuelTransaction>> GetExcelExportDataAsync(DateTime? startDate, DateTime? endDate, ClaimsPrincipal user)
        => GetReportsAsync(startDate, endDate, user);

    public async Task<bool> SoftDeleteAsync(int id, ClaimsPrincipal user)
    {
        var authorized = await (await GetFilteredTransactionsQueryAsync(user)).AnyAsync(x => x.Id == id);
        if (!authorized)
            return false;

        var transaction = await _fuelTransactionRepository.GetTrackedTransactionByIdAsync(id);
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

        var hasRoleFilter = false;

        if (roles.Contains(Roles.AirlineExecutive))
        {
            query = query.Where(x => x.AirlineId == currentUser!.AirlineId);
            hasRoleFilter = true;
        }
        if (roles.Contains(Roles.FuelSupplyExecutive))
        {
            query = query.Where(x => x.FuelCompanyId == currentUser!.FuelCompanyId);
            hasRoleFilter = true;
        }
        if (roles.Contains(Roles.FuelCoordinator))
        {
            query = query.Where(x => x.AirportId == currentUser!.AirportId);
            hasRoleFilter = true;
        }

        return hasRoleFilter ? query : query.Where(x => false);
    }

    private static FuelTransactionDetailsViewModel MapToDetailsViewModel(FuelTransaction x) => new()
    {
        Id = x.Id,
        TransactionNumber = x.TransactionNumber,
        AirlineName = x.Airline?.Name ?? string.Empty,
        AircraftModel = x.Aircraft?.Model ?? string.Empty,
        AirportName = x.Airport?.Name ?? string.Empty,
        FuelCompanyName = x.FuelCompany?.Name ?? string.Empty,
        FlightNumber = x.FlightNumber,
        FuelQuantity = x.FuelQuantity,
        PricePerLiter = x.PricePerLiter,
        TotalAmount = x.TotalAmount,
        Status = x.Status.ToString(),
        Remarks = x.Remarks,
        TransactionDate = x.TransactionDate
    };

    private static string GenerateTransactionNumber()
    {
        var random = new Random();
        return $"FT-{DateTime.Now:yyyyMMdd}-{random.Next(1000, 9999)}";
    }
}
