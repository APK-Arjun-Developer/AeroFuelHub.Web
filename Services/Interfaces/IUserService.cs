using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.User;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IUserService
{
    Task<List<ApplicationUser>> GetUsersAsync();
    Task<CreateUserViewModel> BuildCreateModelAsync();
    Task<(bool Success, string? ErrorMessage, IEnumerable<string>? Errors)> CreateAsync(CreateUserViewModel model);
    Task<EditUserViewModel?> BuildEditModelAsync(string id);
    Task<(bool Success, string? ErrorMessage, IEnumerable<string>? Errors)> UpdateAsync(EditUserViewModel model);
    Task<(bool Success, string? ErrorMessage)> DeleteAsync(string id);
}
