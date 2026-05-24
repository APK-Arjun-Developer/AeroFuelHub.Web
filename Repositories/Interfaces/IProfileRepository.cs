using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<ApplicationUser?> GetUserByUserNameWithRelationsAsync(string userName);
}
