using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.FuelTransaction;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin + "," + Roles.FuelCoordinator + "," + Roles.AirlineExecutive + "," + Roles.FuelSupplyExecutive)]
public class FuelTransactionController : Controller
{
    private readonly IFuelTransactionService _fuelTransactionService;
    private readonly IReportService _reportService;
    private readonly IExportService _exportService;
    private readonly INotyfService _notyf;

    public FuelTransactionController(
        IFuelTransactionService fuelTransactionService,
        IReportService reportService,
        IExportService exportService,
        INotyfService notyf)
    {
        _fuelTransactionService = fuelTransactionService;
        _reportService = reportService;
        _exportService = exportService;
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
        var transaction = await _fuelTransactionService.GetInvoiceDataAsync(id, User);
        if (transaction == null) return NotFound();

        var pdf = _reportService.GenerateInvoicePdf(transaction);
        return File(pdf, "application/pdf", $"Invoice-{transaction.TransactionNumber}.pdf");
    }

    public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
    {
        var data = await _fuelTransactionService.GetReportsAsync(startDate, endDate, User);
        return View(data);
    }

    public async Task<IActionResult> ExportExcel(DateTime? startDate, DateTime? endDate)
    {
        var transactions = await _fuelTransactionService.GetExcelExportDataAsync(startDate, endDate, User);
        var excel = _exportService.GenerateTransactionsExcel(transactions);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FuelTransactions.xlsx");
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
