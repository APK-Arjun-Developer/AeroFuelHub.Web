using System.Security.Claims;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.User;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IUserService
{
    Task<List<ApplicationUser>> GetUsersAsync(ClaimsPrincipal principal);
    Task<CreateUserViewModel> BuildCreateModelAsync();
    Task<(bool Success, string? ErrorMessage, IEnumerable<string>? Errors)> CreateAsync(CreateUserViewModel model);
    Task<EditUserViewModel?> BuildEditModelAsync(string id, ClaimsPrincipal principal);
    Task<(bool Success, string? ErrorMessage, IEnumerable<string>? Errors)> UpdateAsync(EditUserViewModel model, ClaimsPrincipal principal);
    Task<(bool Success, string? ErrorMessage)> DeleteAsync(string id, ClaimsPrincipal principal);
}
