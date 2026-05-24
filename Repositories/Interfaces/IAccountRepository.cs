using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
}
