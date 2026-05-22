using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AeroFuelHub.Web.ViewModels.FuelTransaction;

public class CreateFuelTransactionViewModel
{
    [Required]
    public int AirlineId { get; set; }

    [Required]
    public int AircraftId { get; set; }

    [Required]
    public int AirportId { get; set; }

    [Required]
    public int FuelCompanyId { get; set; }

    [Required]
    public string FlightNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, 1000000)]
    public decimal FuelQuantity { get; set; }

    [Required]
    [Range(1, 1000000)]
    public decimal PricePerLiter { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Remarks { get; set; }

    public List<SelectListItem> Airlines { get; set; } = [];

    public List<SelectListItem> Aircrafts { get; set; } = [];

    public List<SelectListItem> Airports { get; set; } = [];

    public List<SelectListItem> FuelCompanies { get; set; } = [];
}