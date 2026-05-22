using AeroFuelHub.Web.Models.Common;

namespace AeroFuelHub.Web.Models.Entities;

public class Aircraft : BaseEntity
{
    public int Id { get; set; }

    public string Model { get; set; } = string.Empty;

    public string AircraftCode { get; set; } = string.Empty;

    public int AirlineId { get; set; }

    public Airline? Airline { get; set; }
}