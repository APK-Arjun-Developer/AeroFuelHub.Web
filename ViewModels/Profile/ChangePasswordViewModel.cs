using System.ComponentModel.DataAnnotations;

namespace AeroFuelHub.Web.ViewModels.Profile;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
        = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
        = string.Empty;

    [Required]
    [Compare("NewPassword")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
        = string.Empty;
}