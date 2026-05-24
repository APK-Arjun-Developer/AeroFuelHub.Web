using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.FuelTransaction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AeroFuelHub.Web.Repositories.Interfaces;

public interface IFuelTransactionRepository
{
    IQueryable<FuelTransaction> QueryTransactionsWithIncludes();
    Task<List<SelectListItem>> GetAirlinesAsync();
    Task<List<SelectListItem>> GetAircraftsAsync();
    Task<List<AircraftOptionViewModel>> GetAircraftOptionsAsync();
    Task<List<SelectListItem>> GetAircraftsByAirlineAsync(int airlineId);
    Task<List<SelectListItem>> GetAirportsAsync();
    Task<List<SelectListItem>> GetAirportsByIdAsync(int airportId);
    Task<List<SelectListItem>> GetFuelCompaniesAsync();
    Task<bool> AircraftBelongsToAirlineAsync(int aircraftId, int airlineId);
    Task AddTransactionAsync(FuelTransaction transaction);
    Task<FuelTransaction?> GetTrackedTransactionByIdAsync(int id);
    Task SaveChangesAsync();
}
