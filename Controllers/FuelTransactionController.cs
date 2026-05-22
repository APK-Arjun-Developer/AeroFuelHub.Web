using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Enums;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.FuelTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin + "," + Roles.FuelCoordinator)]
public class FuelTransactionController : Controller
{
    private readonly ApplicationDbContext _context;

    public FuelTransactionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateFuelTransactionViewModel();

        await LoadDropdowns(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateFuelTransactionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdowns(model);

            return View(model);
        }

        var transaction = new FuelTransaction
        {
            TransactionNumber = GenerateTransactionNumber(),

            TransactionDate = DateTime.UtcNow,

            AirlineId = model.AirlineId,

            AircraftId = model.AircraftId,

            AirportId = model.AirportId,

            FuelCompanyId = model.FuelCompanyId,

            FlightNumber = model.FlightNumber,

            FuelQuantity = model.FuelQuantity,

            PricePerLiter = model.PricePerLiter,

            TotalAmount =
                model.FuelQuantity * model.PricePerLiter,

            Remarks = model.Remarks,

            Status = FuelTransactionStatus.Completed,

            CreatedAt = DateTime.UtcNow,

            CreatedBy = User.Identity!.Name
        };

        _context.FuelTransactions.Add(transaction);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Fuel transaction created successfully";

        return RedirectToAction(nameof(History));
    }

    public async Task<IActionResult> History(
        string? search)
    {
        var query = _context.FuelTransactions
            .Include(x => x.Airline)
            .Include(x => x.Aircraft)
            .Include(x => x.Airport)
            .Include(x => x.FuelCompany)
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.TransactionNumber.Contains(search) ||
                x.FlightNumber.Contains(search));
        }

        var transactions = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return View(transactions);
    }

    public async Task<IActionResult> Details(int id)
    {
        var transaction = await _context.FuelTransactions
            .Include(x => x.Airline)
            .Include(x => x.Aircraft)
            .Include(x => x.Airport)
            .Include(x => x.FuelCompany)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
            return NotFound();

        return View(transaction);
    }

    public async Task<IActionResult> Invoice(int id)
    {
        QuestPDF.Settings.License =
            LicenseType.Community;

        var transaction = await _context.FuelTransactions
            .Include(x => x.Airline)
            .Include(x => x.Aircraft)
            .Include(x => x.Airport)
            .Include(x => x.FuelCompany)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
            return NotFound();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text("Fuel Transaction Invoice")
                    .FontSize(24)
                    .Bold();

                page.Content()
                    .Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text(
                            $"Transaction No: {transaction.TransactionNumber}");

                        col.Item().Text(
                            $"Airline: {transaction.Airline?.Name}");

                        col.Item().Text(
                            $"Aircraft: {transaction.Aircraft?.Model}");

                        col.Item().Text(
                            $"Airport: {transaction.Airport?.Name}");

                        col.Item().Text(
                            $"Fuel Company: {transaction.FuelCompany?.Name}");

                        col.Item().Text(
                            $"Flight Number: {transaction.FlightNumber}");

                        col.Item().Text(
                            $"Fuel Quantity: {transaction.FuelQuantity}");

                        col.Item().Text(
                            $"Price Per Liter: {transaction.PricePerLiter}");

                        col.Item().Text(
                            $"Total Amount: {transaction.TotalAmount}");

                        col.Item().Text(
                            $"Transaction Date: {transaction.TransactionDate}");
                    });

                page.Footer()
                    .AlignCenter()
                    .Text("AeroFuel Hub");
            });
        }).GeneratePdf();

        return File(
            pdf,
            "application/pdf",
            $"Invoice-{transaction.TransactionNumber}.pdf");
    }

    public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.FuelTransactions
            .Include(x => x.Airline)
            .Include(x => x.Airport)
            .Include(x => x.FuelCompany)
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(x =>
                x.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(x =>
                x.TransactionDate <= endDate.Value);
        }

        var data = await query
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync();

        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction =
            await _context.FuelTransactions.FindAsync(id);

        if (transaction == null)
            return NotFound();

        transaction.IsDeleted = true;

        transaction.DeletedAt = DateTime.UtcNow;

        transaction.DeletedBy = User.Identity!.Name;

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Transaction deleted successfully";

        return RedirectToAction(nameof(History));
    }

    private async Task LoadDropdowns(
        CreateFuelTransactionViewModel model)
    {
        model.Airlines = await _context.Airlines
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync();

        model.Aircrafts = await _context.Aircrafts
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Model
            })
            .ToListAsync();

        model.Airports = await _context.Airports
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync();

        model.FuelCompanies =
            await _context.FuelCompanies
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync();
    }

    private string GenerateTransactionNumber()
    {
        return $"FT-{DateTime.Now:yyyyMMddHHmmss}";
    }
}