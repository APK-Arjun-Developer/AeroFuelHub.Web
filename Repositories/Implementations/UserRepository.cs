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

    public Task<List<ApplicationUser>> GetUsersAsync() => _context.Users.OrderByDescending(x => x.CreatedAt).ToListAsync();

    public Task<List<SelectListItem>> GetRolesAsync() => _context.Roles.Select(x => new SelectListItem { Value = x.Name!, Text = x.Name! }).ToListAsync();

    public Task<List<SelectListItem>> GetAirlinesAsync() => _context.Airlines.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();

    public Task<List<SelectListItem>> GetFuelCompaniesAsync() => _context.FuelCompanies.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();

    public Task<List<SelectListItem>> GetAirportsAsync() => _context.Airports.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
}
