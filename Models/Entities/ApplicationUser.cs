using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
        = string.Empty;

    public DateTime CreatedAt { get; set; }

    public int? AirlineId { get; set; }

    public int? FuelCompanyId { get; set; }

    public int? AirportId { get; set; }

    public Airline? Airline { get; set; }

    public FuelCompany? FuelCompany { get; set; }

    public Airport? Airport { get; set; }
}