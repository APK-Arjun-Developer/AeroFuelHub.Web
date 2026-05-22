using AeroFuelHub.Web.Models.Common;

namespace AeroFuelHub.Web.Models.Entities;

public class Airline : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Aircraft> Aircrafts { get; set; } = [];
}