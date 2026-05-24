using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Services.Interfaces;
using ClosedXML.Excel;

namespace AeroFuelHub.Web.Services.Implementations;

public class ExportService : IExportService
{
    public byte[] GenerateTransactionsExcel(IEnumerable<FuelTransaction> transactions)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Transactions");
        worksheet.Cell(1, 1).Value = "Transaction Number";
        worksheet.Cell(1, 2).Value = "Airline";
        worksheet.Cell(1, 3).Value = "Airport";
        worksheet.Cell(1, 4).Value = "Fuel Company";
        worksheet.Cell(1, 5).Value = "Total Amount";
        worksheet.Cell(1, 6).Value = "Date";

        var row = 2;
        foreach (var item in transactions)
        {
            worksheet.Cell(row, 1).Value = item.TransactionNumber;
            worksheet.Cell(row, 2).Value = item.Airline?.Name;
            worksheet.Cell(row, 3).Value = item.Airport?.Name;
            worksheet.Cell(row, 4).Value = item.FuelCompany?.Name;
            worksheet.Cell(row, 5).Value = item.TotalAmount;
            worksheet.Cell(row, 6).Value = item.TransactionDate;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
