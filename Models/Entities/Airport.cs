using AeroFuelHub.Web.Models.Common;

namespace AeroFuelHub.Web.Models.Entities;

public class Airport : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
}