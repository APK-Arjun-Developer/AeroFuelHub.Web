using System.Security.Claims;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;

    public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public Task<List<ApplicationUser>> GetUsersAsync(ClaimsPrincipal principal)
    {
        var currentUserId = _userManager.GetUserId(principal)
            ?? throw new InvalidOperationException("Current user id is not available.");
        return _userRepository.GetUsersAsync(currentUserId);
    }

    public async Task<CreateUserViewModel> BuildCreateModelAsync() => new()
    {
        Roles = await _userRepository.GetRolesAsync(),
        Airlines = await _userRepository.GetAirlinesAsync(),
        FuelCompanies = await _userRepository.GetFuelCompaniesAsync(),
        Airports = await _userRepository.GetAirportsAsync()
    };

    public async Task<(bool Success, string? ErrorMessage, IEnumerable<string>? Errors)> CreateAsync(CreateUserViewModel model)
    {
        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing != null) return (false, "Email already exists", null);

        var user = new ApplicationUser
        {
            FullName = model.FullName,
            UserName = model.Email,
            Email = model.Email,
            AirlineId = model.AirlineId,
            FuelCompanyId = model.FuelCompanyId,
            AirportId = model.AirportId,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return (false, "Failed to create user", result.Errors.Select(x => x.Description));

        await _userManager.AddToRoleAsync(user, model.Role);
        return (true, null, null);
    }

    public async Task<EditUserViewModel?> BuildEditModelAsync(string id, ClaimsPrincipal principal)
    {
        if (IsCurrentUser(id, principal)) return null;

        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        return new EditUserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? string.Empty,
            AirlineId = user.AirlineId,
            FuelCompanyId = user.FuelCompanyId,
            AirportId = user.AirportId,
            Roles = await _userRepository.GetRolesAsync(),
            Airlines = await _userRepository.GetAirlinesAsync(),
            FuelCompanies = await _userRepository.GetFuelCompaniesAsync(),
            Airports = await _userRepository.GetAirportsAsync()
        };
    }

    public async Task<(bool Success, string? ErrorMessage, IEnumerable<string>? Errors)> UpdateAsync(
        EditUserViewModel model,
        ClaimsPrincipal principal)
    {
        if (IsCurrentUser(model.Id, principal))
            return (false, "Update your profile from the Profile page.", null);

        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null) return (false, "User not found", null);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null && existingUser.Id != model.Id) return (false, "Email already exists", null);

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.AirlineId = model.AirlineId;
        user.FuelCompanyId = model.FuelCompanyId;
        user.AirportId = model.AirportId;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return (false, "Failed to update user", result.Errors.Select(x => x.Description));

        var existingRoles = await _userManager.GetRolesAsync(user);
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);
        if (!removeRolesResult.Succeeded)
            return (false, "Failed to update user role", removeRolesResult.Errors.Select(x => x.Description));

        var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
        if (!addRoleResult.Succeeded)
            return (false, "Failed to update user role", addRoleResult.Errors.Select(x => x.Description));

        return (true, null, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(string id, ClaimsPrincipal principal)
    {
        if (IsCurrentUser(id, principal))
            return (false, "You cannot delete your own account.");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return (false, "User not found");
        if (user.Email == "admin@aerofuelhub.com") return (false, "Default admin cannot be deleted");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return (false, "Failed to delete user");

        return (true, null);
    }

    private bool IsCurrentUser(string id, ClaimsPrincipal principal) =>
        _userManager.GetUserId(principal) == id;
}
