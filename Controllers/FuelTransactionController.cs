using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.FuelTransaction;
using AspNetCoreHero.ToastNotification.Abstractions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin + "," + Roles.FuelCoordinator + "," + Roles.AirlineExecutive + "," + Roles.FuelSupplyExecutive)]
public class FuelTransactionController : Controller
{
    private readonly IFuelTransactionService _fuelTransactionService;
    private readonly INotyfService _notyf;

    public FuelTransactionController(IFuelTransactionService fuelTransactionService, INotyfService notyf)
    {
        _fuelTransactionService = fuelTransactionService;
        _notyf = notyf;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await _fuelTransactionService.BuildCreateViewModelAsync(User);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFuelTransactionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var vm = await _fuelTransactionService.BuildCreateViewModelAsync(User);
            model.Airlines = vm.Airlines;
            model.Aircrafts = vm.Aircrafts;
            model.Airports = vm.Airports;
            model.FuelCompanies = vm.FuelCompanies;
            return View(model);
        }

        var result = await _fuelTransactionService.CreateTransactionAsync(model, User);
        if (!result.Success)
        {
            ModelState.AddModelError(result.ErrorKey!, result.ErrorMessage!);
            var vm = await _fuelTransactionService.BuildCreateViewModelAsync(User);
            model.Airlines = vm.Airlines;
            model.Aircrafts = vm.Aircrafts;
            model.Airports = vm.Airports;
            model.FuelCompanies = vm.FuelCompanies;
            return View(model);
        }

        _notyf.Success("Fuel transaction created successfully");
        return RedirectToAction(nameof(History));
    }

    public async Task<IActionResult> History(string? search)
    {
        var transactions = await _fuelTransactionService.GetHistoryAsync(search, User);
        return View(transactions);
    }

    public async Task<IActionResult> Details(int id)
    {
        var transaction = await _fuelTransactionService.GetDetailsAsync(id, User);
        if (transaction == null) return NotFound();
        return View(transaction);
    }

    public async Task<IActionResult> Invoice(int id)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var transaction = await _fuelTransactionService.GetInvoiceDataAsync(id, User);
        if (transaction == null) return NotFound();

        var pdf = Document.Create(container =>
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

        return File(pdf, "application/pdf", $"Invoice-{transaction.TransactionNumber}.pdf");
    }

    public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
    {
        var data = await _fuelTransactionService.GetReportsAsync(startDate, endDate, User);
        return View(data);
    }

    public async Task<IActionResult> ExportExcel()
    {
        var transactions = await _fuelTransactionService.GetExcelExportDataAsync(User);
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Transactions");
        worksheet.Cell(1, 1).Value = "Transaction Number";
        worksheet.Cell(1, 2).Value = "Airline";
        worksheet.Cell(1, 3).Value = "Airport";
        worksheet.Cell(1, 4).Value = "Fuel Company";
        worksheet.Cell(1, 5).Value = "Total Amount";

        int row = 2;
        foreach (var item in transactions)
        {
            worksheet.Cell(row, 1).Value = item.TransactionNumber;
            worksheet.Cell(row, 2).Value = item.Airline?.Name;
            worksheet.Cell(row, 3).Value = item.Airport?.Name;
            worksheet.Cell(row, 4).Value = item.FuelCompany?.Name;
            worksheet.Cell(row, 5).Value = item.TotalAmount;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FuelTransactions.xlsx");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _fuelTransactionService.SoftDeleteAsync(id, User);
        if (!deleted) return NotFound();
        _notyf.Success("Transaction deleted successfully");
        return RedirectToAction(nameof(History));
    }
}
