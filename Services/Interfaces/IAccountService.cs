using System.Security.Claims;
using AeroFuelHub.Web.ViewModels.Account;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IAccountService
{
    Task<(bool Success, string? ErrorMessage, string RedirectAction, string RedirectController)> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
    Task<(string Action, string Controller)> GetDashboardRedirectAsync(ClaimsPrincipal user);
}
