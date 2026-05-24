using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Repositories.Implementations;

public class FuelTransactionRepository : IFuelTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public FuelTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<FuelTransaction> QueryTransactionsWithIncludes()
    {
        return _context.FuelTransactions
            .Include(x => x.Airline)
            .Include(x => x.Aircraft)
            .Include(x => x.Airport)
            .Include(x => x.FuelCompany)
            .Where(x => !x.IsDeleted)
            .AsQueryable();
    }

    public Task<List<SelectListItem>> GetAirlinesAsync() => _context.Airlines
        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
        .ToListAsync();

    public Task<List<SelectListItem>> GetAircraftsAsync() => _context.Aircrafts
        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Model })
        .ToListAsync();

    public Task<List<SelectListItem>> GetAirportsAsync() => _context.Airports
        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
        .ToListAsync();

    public Task<List<SelectListItem>> GetAirportsByIdAsync(int airportId) => _context.Airports
        .Where(x => x.Id == airportId)
        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
        .ToListAsync();

    public Task<List<SelectListItem>> GetFuelCompaniesAsync() => _context.FuelCompanies
        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
        .ToListAsync();

    public Task<bool> AircraftBelongsToAirlineAsync(int aircraftId, int airlineId) =>
        _context.Aircrafts.AnyAsync(x => x.Id == aircraftId && x.AirlineId == airlineId);

    public Task AddTransactionAsync(FuelTransaction transaction) => _context.FuelTransactions.AddAsync(transaction).AsTask();

    public Task<FuelTransaction?> GetTransactionByIdAsync(int id) => _context.FuelTransactions.FirstOrDefaultAsync(x => x.Id == id);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
