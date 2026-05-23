using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.User;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly ApplicationDbContext _context;

    private readonly INotyfService _notyf;

    public UserController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        INotyfService notyf)
    {
        _userManager = userManager;

        _roleManager = roleManager;

        _context = context;

        _notyf = notyf;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateUserViewModel();

        await LoadDropdowns(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdowns(model);

            return View(model);
        }

        var existing =
            await _userManager.FindByEmailAsync(model.Email);

        if (existing != null)
        {
            _notyf.Error("Email already exists");

            await LoadDropdowns(model);

            return View(model);
        }

        var user = new ApplicationUser
        {
            FullName = model.FullName,

            UserName = model.Email,

            Email = model.Email,

            AirlineId = model.AirlineId,

            FuelCompanyId = model.FuelCompanyId,

            AirportId = model.AirportId,

            EmailConfirmed = true,

            CreatedAt = DateTime.UtcNow
        };

        var result =
            await _userManager.CreateAsync(
                user,
                model.Password);

        if (!result.Succeeded)
        {
            _notyf.Error("Failed to create user");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }

            await LoadDropdowns(model);

            return View(model);
        }

        await _userManager.AddToRoleAsync(
            user,
            model.Role);

        _notyf.Success("User created successfully");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user =
            await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        var roles =
            await _userManager.GetRolesAsync(user);

        var model = new EditUserViewModel
        {
            Id = user.Id,

            FullName = user.FullName,

            Email = user.Email!,

            Role = roles.FirstOrDefault() ?? "",

            AirlineId = user.AirlineId,

            FuelCompanyId = user.FuelCompanyId
        };

        await LoadEditDropdowns(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadEditDropdowns(model);

            return View(model);
        }

        var user =
            await _userManager.FindByIdAsync(model.Id);

        if (user == null)
            return NotFound();

        var existingUser =
    await _userManager.FindByEmailAsync(model.Email);

        if (existingUser != null
            && existingUser.Id != model.Id)
        {
            _notyf.Error("Email already exists");

            await LoadEditDropdowns(model);

            return View(model);
        }

        user.FullName = model.FullName;

        user.Email = model.Email;

        user.UserName = model.Email;

        user.AirlineId = model.AirlineId;

        user.FuelCompanyId = model.FuelCompanyId;

        var existingRoles =
            await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(
            user,
            existingRoles);

        await _userManager.AddToRoleAsync(
            user,
            model.Role);

        var updateResult =
    await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            _notyf.Error("Failed to update user");

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }

            await LoadEditDropdowns(model);

            return View(model);
        }

        _notyf.Success("User updated successfully");

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var user =
            await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        if (user.Email == "admin@aerofuelhub.com")
        {
            _notyf.Error("Default admin cannot be deleted");

            return RedirectToAction(nameof(Index));
        }

        var result =
    await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            _notyf.Error("Failed to delete user");

            return RedirectToAction(nameof(Index));
        }

        _notyf.Success("User deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadDropdowns(
        CreateUserViewModel model)
    {
        model.Roles =
            await _roleManager.Roles
            .Select(x => new SelectListItem
            {
                Value = x.Name!,
                Text = x.Name!
            })
            .ToListAsync();

        model.Airlines =
            await _context.Airlines
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

        model.Airports = await _context.Airports
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync();
    }

    private async Task LoadEditDropdowns(EditUserViewModel model)
    {
        model.Roles =
            await _roleManager.Roles
            .Select(x => new SelectListItem
            {
                Value = x.Name!,
                Text = x.Name!
            })
            .ToListAsync();

        model.Airlines =
            await _context.Airlines
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

        model.Airports = await _context.Airports
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync();
    }
}