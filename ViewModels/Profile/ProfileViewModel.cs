using System.ComponentModel.DataAnnotations;

namespace AeroFuelHub.Web.ViewModels.Profile;

public class ProfileViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Role")]
    public string Role { get; set; } = string.Empty;

    [Display(Name = "Airline")]
    public string? Airline { get; set; }

    [Display(Name = "Fuel Company")]
    public string? FuelCompany { get; set; }

    [Display(Name = "Airport")]
    public string? Airport { get; set; }
}
