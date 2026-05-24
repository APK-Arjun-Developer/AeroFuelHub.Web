using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Services.Interfaces;

public interface IExportService
{
    byte[] GenerateTransactionsExcel(IEnumerable<FuelTransaction> transactions);
}
