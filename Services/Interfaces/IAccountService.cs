using AeroFuelHub.Web.ViewModels.Account;
using System.Security.Claims;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IAccountService
{
    Task<(bool Success, string? ErrorMessage, string RedirectAction, string RedirectController)> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
    Task<(string Action, string Controller)> GetDashboardRedirectAsync(ClaimsPrincipal user);
}
