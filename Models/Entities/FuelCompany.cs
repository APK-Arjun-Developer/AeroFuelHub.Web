using AeroFuelHub.Web.Models.Common;

namespace AeroFuelHub.Web.Models.Entities;

public class FuelCompany : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}