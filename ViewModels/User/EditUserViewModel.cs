using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoleNames = AeroFuelHub.Web.Constants.Roles;

namespace AeroFuelHub.Web.ViewModels.User;

public class EditUserViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Role")]
    [AllowedValues(RoleNames.Admin, RoleNames.AirlineExecutive, RoleNames.FuelSupplyExecutive, RoleNames.FuelCoordinator)]
    public string Role { get; set; } = string.Empty;

    [Display(Name = "Airline")]
    public int? AirlineId { get; set; }

    [Display(Name = "Fuel Company")]
    public int? FuelCompanyId { get; set; }

    [Display(Name = "Airport")]
    public int? AirportId { get; set; }

    public List<SelectListItem> Roles { get; set; } = [];

    public List<SelectListItem> Airlines { get; set; } = [];

    public List<SelectListItem> FuelCompanies { get; set; } = [];

    public IEnumerable<SelectListItem> Airports { get; set; } = [];
}
