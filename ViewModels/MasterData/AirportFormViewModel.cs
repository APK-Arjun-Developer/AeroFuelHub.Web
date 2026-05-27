using System.ComponentModel.DataAnnotations;

namespace AeroFuelHub.Web.ViewModels.MasterData;

public class AirportFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Display(Name = "Airport Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [Display(Name = "Airport Code")]
    public string Code { get; set; } = string.Empty;
}

