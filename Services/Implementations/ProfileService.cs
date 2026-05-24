using System.Security.Claims;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Services.Implementations;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IProfileRepository _profileRepository;

    public ProfileService(UserManager<ApplicationUser> userManager, IProfileRepository profileRepository)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
    }

    public async Task<ProfileViewModel?> GetProfileAsync(ClaimsPrincipal principal)
    {
        var user = await _profileRepository.GetUserByUserNameWithRelationsAsync(principal.Identity!.Name!);
        if (user == null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return new ProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? string.Empty,
            Airline = user.Airline?.Name,
            FuelCompany = user.FuelCompany?.Name,
            Airport = user.Airport?.Name
        };
    }

    public async Task UpdateProfileAsync(ProfileViewModel model, ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        user!.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;
        await _userManager.UpdateAsync(user);
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> ChangePasswordAsync(ChangePasswordViewModel model, ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        var result = await _userManager.ChangePasswordAsync(user!, model.CurrentPassword, model.NewPassword);
        return result.Succeeded ? (true, []) : (false, result.Errors.Select(x => x.Description));
    }
}
