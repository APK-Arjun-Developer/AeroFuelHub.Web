using System.Security.Claims;
using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAccountRepository _accountRepository;

    public AccountService(SignInManager<ApplicationUser> signInManager, IAccountRepository accountRepository)
    {
        _signInManager = signInManager;
        _accountRepository = accountRepository;
    }

    public async Task<(bool Success, string? ErrorMessage, string RedirectAction, string RedirectController)> LoginAsync(LoginViewModel model)
    {
        var user = await _accountRepository.FindByEmailAsync(model.Email);
        if (user == null) return (false, "Invalid email or password", "Login", "Account");

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, false);
        if (!result.Succeeded) return (false, "Invalid email or password", "Login", "Account");

        var roles = await _accountRepository.GetRolesAsync(user);
        var redirect = GetDashboardRedirectForRoles(roles);
        return (true, null, redirect.Action, redirect.Controller);
    }

    public async Task<(string Action, string Controller)> GetDashboardRedirectAsync(ClaimsPrincipal user)
    {
        var appUser = await _accountRepository.FindByEmailAsync(user.Identity!.Name!);
        if (appUser == null)
            return ("Login", "Account");

        var roles = await _accountRepository.GetRolesAsync(appUser);
        return GetDashboardRedirectForRoles(roles);
    }

    public Task LogoutAsync() => _signInManager.SignOutAsync();

    private static (string Action, string Controller) GetDashboardRedirectForRoles(IList<string> roles)
    {
        if (roles.Contains(Roles.Admin)) return ("Admin", "Dashboard");
        if (roles.Contains(Roles.AirlineExecutive)) return ("Airline", "Dashboard");
        if (roles.Contains(Roles.FuelSupplyExecutive)) return ("FuelSupply", "Dashboard");
        if (roles.Contains(Roles.FuelCoordinator)) return ("Coordinator", "Dashboard");
        return ("Login", "Account");
    }
}
