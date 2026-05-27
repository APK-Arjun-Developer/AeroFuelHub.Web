using System.ComponentModel.DataAnnotations;

namespace AeroFuelHub.Web.ViewModels.MasterData;

public class AirlineFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Display(Name = "Airline Name")]
    public string Name { get; set; } = string.Empty;
}

