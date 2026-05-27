using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.MasterData;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AirportsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notyf;

    public AirportsController(ApplicationDbContext context, INotyfService notyf)
    {
        _context = context;
        _notyf = notyf;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _context.Airports
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(items);
    }

    [HttpGet]
    public IActionResult Create() => View(new AirportFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AirportFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var entity = new Airport
        {
            Name = model.Name.Trim(),
            Code = model.Code.Trim().ToUpperInvariant(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name
        };

        _context.Airports.Add(entity);
        await _context.SaveChangesAsync();

        _notyf.Success("Airport created successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _context.Airports.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null) return NotFound();

        return View(new AirportFormViewModel { Id = entity.Id, Name = entity.Name, Code = entity.Code });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AirportFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Id == null) return BadRequest();

        var entity = await _context.Airports.FirstOrDefaultAsync(x => x.Id == model.Id.Value && !x.IsDeleted);
        if (entity == null) return NotFound();

        entity.Name = model.Name.Trim();
        entity.Code = model.Code.Trim().ToUpperInvariant();
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        _notyf.Success("Airport updated successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Airports.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null) return NotFound();

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = User.Identity?.Name;
        await _context.SaveChangesAsync();

        _notyf.Success("Airport deleted successfully");
        return RedirectToAction(nameof(Index));
    }
}

