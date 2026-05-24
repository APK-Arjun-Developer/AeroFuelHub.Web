using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Repositories.Implementations;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ApplicationUser?> GetUserByUserNameWithRelationsAsync(string userName) => _context.Users
        .Include(x => x.Airline)
        .Include(x => x.FuelCompany)
        .Include(x => x.Airport)
        .FirstOrDefaultAsync(x => x.UserName == userName);
}
