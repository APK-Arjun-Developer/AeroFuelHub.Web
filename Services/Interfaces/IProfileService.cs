using System.Security.Claims;
using AeroFuelHub.Web.ViewModels.Profile;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileViewModel?> GetProfileAsync(ClaimsPrincipal principal);
    Task UpdateProfileAsync(ProfileViewModel model, ClaimsPrincipal principal);
    Task<(bool Success, IEnumerable<string> Errors)> ChangePasswordAsync(ChangePasswordViewModel model, ClaimsPrincipal principal);
}
