using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using AeroFuelHub.Web.Models.Entities;

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
        if (roles.Contains(Roles.Admin)) return (true, null, "Admin", "Dashboard");
        if (roles.Contains(Roles.AirlineExecutive)) return (true, null, "Airline", "Dashboard");
        if (roles.Contains(Roles.FuelSupplyExecutive)) return (true, null, "FuelSupply", "Dashboard");
        if (roles.Contains(Roles.FuelCoordinator)) return (true, null, "Coordinator", "Dashboard");
        return (true, null, "Index", "Home");
    }

    public Task LogoutAsync() => _signInManager.SignOutAsync();
}
