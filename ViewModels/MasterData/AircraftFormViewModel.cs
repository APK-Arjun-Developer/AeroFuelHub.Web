using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AeroFuelHub.Web.ViewModels.MasterData;

public class AircraftFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Display(Name = "Aircraft Model")]
    public string Model { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Display(Name = "Aircraft Code")]
    public string AircraftCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Airline")]
    public int AirlineId { get; set; }

    public List<SelectListItem> Airlines { get; set; } = [];
}

