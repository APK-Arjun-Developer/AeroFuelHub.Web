using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.MasterData;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AircraftsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notyf;

    public AircraftsController(ApplicationDbContext context, INotyfService notyf)
    {
        _context = context;
        _notyf = notyf;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _context.Aircrafts
            .AsNoTracking()
            .Include(x => x.Airline)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Airline!.Name)
            .ThenBy(x => x.Model)
            .ToListAsync();

        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new AircraftFormViewModel
        {
            Airlines = await BuildAirlinesAsync()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AircraftFormViewModel vm)
    {
        vm.Airlines = await BuildAirlinesAsync();
        if (!ModelState.IsValid) return View(vm);

        var entity = new Aircraft
        {
            Model = vm.Model.Trim(),
            AircraftCode = vm.AircraftCode.Trim(),
            AirlineId = vm.AirlineId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name
        };

        _context.Aircrafts.Add(entity);
        await _context.SaveChangesAsync();

        _notyf.Success("Aircraft created successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _context.Aircrafts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null) return NotFound();

        return View(new AircraftFormViewModel
        {
            Id = entity.Id,
            Model = entity.Model,
            AircraftCode = entity.AircraftCode,
            AirlineId = entity.AirlineId,
            Airlines = await BuildAirlinesAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AircraftFormViewModel vm)
    {
        vm.Airlines = await BuildAirlinesAsync();
        if (!ModelState.IsValid) return View(vm);
        if (vm.Id == null) return BadRequest();

        var entity = await _context.Aircrafts.FirstOrDefaultAsync(x => x.Id == vm.Id.Value && !x.IsDeleted);
        if (entity == null) return NotFound();

        entity.Model = vm.Model.Trim();
        entity.AircraftCode = vm.AircraftCode.Trim();
        entity.AirlineId = vm.AirlineId;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        _notyf.Success("Aircraft updated successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Aircrafts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null) return NotFound();

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = User.Identity?.Name;
        await _context.SaveChangesAsync();

        _notyf.Success("Aircraft deleted successfully");
        return RedirectToAction(nameof(Index));
    }

    private Task<List<SelectListItem>> BuildAirlinesAsync() => _context.Airlines
        .AsNoTracking()
        .Where(x => !x.IsDeleted)
        .OrderBy(x => x.Name)
        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
        .ToListAsync();
}

