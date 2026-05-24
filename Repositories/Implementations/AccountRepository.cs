using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Repositories.Implementations;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<ApplicationUser?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

    public Task<IList<string>> GetRolesAsync(ApplicationUser user) => _userManager.GetRolesAsync(user);
}
