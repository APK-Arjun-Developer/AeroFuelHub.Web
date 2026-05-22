using AeroFuelHub.Web.Enums;
using AeroFuelHub.Web.Models.Common;

namespace AeroFuelHub.Web.Models.Entities;

public class FuelTransaction : BaseEntity
{
    public int Id { get; set; }

    public string TransactionNumber { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; }

    public int AirlineId { get; set; }

    public int AircraftId { get; set; }

    public int AirportId { get; set; }

    public int FuelCompanyId { get; set; }

    public string FlightNumber { get; set; } = string.Empty;

    public decimal FuelQuantity { get; set; }

    public decimal PricePerLiter { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Remarks { get; set; }

    public FuelTransactionStatus Status { get; set; }

    public Airline? Airline { get; set; }

    public Aircraft? Aircraft { get; set; }

    public Airport? Airport { get; set; }

    public FuelCompany? FuelCompany { get; set; }
}