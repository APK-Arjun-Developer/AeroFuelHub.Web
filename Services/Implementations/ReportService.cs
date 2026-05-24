using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace AeroFuelHub.Web.Services.Implementations;

public class ReportService : IReportService
{
    public byte[] GenerateInvoicePdf(FuelTransaction transaction)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Fuel Transaction Invoice").FontSize(24).Bold();
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text($"Transaction No: {transaction.TransactionNumber}");
                    col.Item().Text($"Airline: {transaction.Airline?.Name}");
                    col.Item().Text($"Aircraft: {transaction.Aircraft?.Model}");
                    col.Item().Text($"Airport: {transaction.Airport?.Name}");
                    col.Item().Text($"Fuel Company: {transaction.FuelCompany?.Name}");
                    col.Item().Text($"Flight Number: {transaction.FlightNumber}");
                    col.Item().Text($"Fuel Quantity: {transaction.FuelQuantity}");
                    col.Item().Text($"Price Per Liter: {transaction.PricePerLiter}");
                    col.Item().Text($"Total Amount: {transaction.TotalAmount}");
                    col.Item().Text($"Transaction Date: {transaction.TransactionDate}");
                });
                page.Footer().AlignCenter().Text("AeroFuel Hub");
            });
        }).GeneratePdf();
    }
}
