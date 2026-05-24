namespace AeroFuelHub.Web.ViewModels.FuelTransaction;

public class FuelTransactionDetailsViewModel
{
    public int Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string AirlineName { get; set; } = string.Empty;
    public string AircraftModel { get; set; } = string.Empty;
    public string AirportName { get; set; } = string.Empty;
    public string FuelCompanyName { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public decimal FuelQuantity { get; set; }
    public decimal PricePerLiter { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime TransactionDate { get; set; }
}
