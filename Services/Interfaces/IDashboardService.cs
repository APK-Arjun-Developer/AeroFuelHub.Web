using System.Security.Claims;
using AeroFuelHub.Web.ViewModels.Dashboard;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IDashboardService
{
    Task<AdminDashboardViewModel> GetAdminDashboardAsync();
    Task<RoleDashboardViewModel?> GetAirlineDashboardAsync(ClaimsPrincipal principal);
    Task<RoleDashboardViewModel?> GetFuelSupplyDashboardAsync(ClaimsPrincipal principal);
    Task<RoleDashboardViewModel?> GetCoordinatorDashboardAsync(ClaimsPrincipal principal);
}
