using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AeroFuelHub.Web.ViewModels.User;

public class CreateUserViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    public int? AirlineId { get; set; }

    public int? FuelCompanyId { get; set; }

    public List<SelectListItem> Roles { get; set; } = [];

    public List<SelectListItem> Airlines { get; set; } = [];

    public List<SelectListItem> FuelCompanies { get; set; } = [];
}