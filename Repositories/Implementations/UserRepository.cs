using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<ApplicationUser>> GetUsersAsync(string excludeUserId) =>
        _context.Users.AsNoTracking()
            .Where(x => x.Id != excludeUserId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public Task<List<SelectListItem>> GetRolesAsync() =>
        _context.Roles.AsNoTracking().Select(x => new SelectListItem { Value = x.Name!, Text = x.Name! }).ToListAsync();

    public Task<List<SelectListItem>> GetAirlinesAsync() =>
        _context.Airlines.AsNoTracking().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();

    public Task<List<SelectListItem>> GetFuelCompaniesAsync() =>
        _context.FuelCompanies.AsNoTracking().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();

    public Task<List<SelectListItem>> GetAirportsAsync() =>
        _context.Airports.AsNoTracking().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
}
