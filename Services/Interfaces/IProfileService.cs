using AeroFuelHub.Web.ViewModels.Profile;
using System.Security.Claims;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileViewModel?> GetProfileAsync(ClaimsPrincipal principal);
    Task UpdateProfileAsync(ProfileViewModel model, ClaimsPrincipal principal);
    Task<(bool Success, IEnumerable<string> Errors)> ChangePasswordAsync(ChangePasswordViewModel model, ClaimsPrincipal principal);
}
