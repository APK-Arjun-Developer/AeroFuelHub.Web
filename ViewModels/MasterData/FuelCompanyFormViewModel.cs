using System.ComponentModel.DataAnnotations;

namespace AeroFuelHub.Web.ViewModels.MasterData;

public class FuelCompanyFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Display(Name = "Fuel Company Name")]
    public string Name { get; set; } = string.Empty;
}

