using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IReportService
{
    byte[] GenerateInvoicePdf(FuelTransaction transaction);
}
