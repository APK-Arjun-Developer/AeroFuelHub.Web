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
public class FuelCompaniesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notyf;

    public FuelCompaniesController(ApplicationDbContext context, INotyfService notyf)
    {
        _context = context;
        _notyf = notyf;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _context.FuelCompanies
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(items);
    }

    [HttpGet]
    public IActionResult Create() => View(new FuelCompanyFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FuelCompanyFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var entity = new FuelCompany
        {
            Name = model.Name.Trim(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name
        };

        _context.FuelCompanies.Add(entity);
        await _context.SaveChangesAsync();

        _notyf.Success("Fuel company created successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _context.FuelCompanies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null) return NotFound();

        return View(new FuelCompanyFormViewModel { Id = entity.Id, Name = entity.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(FuelCompanyFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Id == null) return BadRequest();

        var entity = await _context.FuelCompanies.FirstOrDefaultAsync(x => x.Id == model.Id.Value && !x.IsDeleted);
        if (entity == null) return NotFound();

        entity.Name = model.Name.Trim();
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = User.Identity?.Name;

        await _context.SaveChangesAsync();

        _notyf.Success("Fuel company updated successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.FuelCompanies.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null) return NotFound();

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = User.Identity?.Name;
        await _context.SaveChangesAsync();

        _notyf.Success("Fuel company deleted successfully");
        return RedirectToAction(nameof(Index));
    }
}

