using AeroFuelHub.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AeroFuelHub.Web.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<ApplicationUser>> GetUsersAsync(string excludeUserId);
    Task<List<SelectListItem>> GetRolesAsync();
    Task<List<SelectListItem>> GetAirlinesAsync();
    Task<List<SelectListItem>> GetFuelCompaniesAsync();
    Task<List<SelectListItem>> GetAirportsAsync();
}
