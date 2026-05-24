using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AeroFuelHub.Web.ViewModels.FuelTransaction;

public class CreateFuelTransactionViewModel
{
    [Required]
    [Display(Name = "Airline")]
    public int AirlineId { get; set; }

    [Required]
    [Display(Name = "Aircraft")]
    public int AircraftId { get; set; }

    [Display(Name = "Airport")]
    public int? AirportId { get; set; }

    [Required]
    [Display(Name = "Fuel Company")]
    public int FuelCompanyId { get; set; }

    [Required]
    [Display(Name = "Flight Number")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, 1000000)]
    [Display(Name = "Fuel Quantity (Liters)")]
    public decimal FuelQuantity { get; set; }

    [Required]
    [Range(1, 1000000)]
    [Display(Name = "Price Per Liter")]
    public decimal PricePerLiter { get; set; }

    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Remarks")]
    public string? Remarks { get; set; }

    public List<SelectListItem> Airlines { get; set; } = [];

    public List<SelectListItem> Aircrafts { get; set; } = [];

    public List<AircraftOptionViewModel> AircraftOptions { get; set; } = [];

    public bool IsAirlineLocked { get; set; }

    public List<SelectListItem> Airports { get; set; } = [];

    public List<SelectListItem> FuelCompanies { get; set; } = [];
}
