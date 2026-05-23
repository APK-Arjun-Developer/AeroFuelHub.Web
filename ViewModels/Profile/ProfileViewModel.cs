using System.ComponentModel.DataAnnotations;

namespace AeroFuelHub.Web.ViewModels.Profile;

public class ProfileViewModel
{
    [Required]
    public string FullName { get; set; }
        = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; }
        = string.Empty;

    public string Role { get; set; }
        = string.Empty;

    public string? Airline { get; set; }

    public string? FuelCompany { get; set; }

    public string? Airport { get; set; }
}