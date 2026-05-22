using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public int? AirlineId { get; set; }

    public int? FuelCompanyId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}