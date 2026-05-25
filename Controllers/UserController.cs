using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.User;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly INotyfService _notyf;

    public UserController(IUserService userService, INotyfService notyf)
    {
        _userService = userService;
        _notyf = notyf;
    }

    public async Task<IActionResult> Index() => View(await _userService.GetUsersAsync(User));

    [HttpGet]
    public async Task<IActionResult> Create() => View(await _userService.BuildCreateModelAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await MergeCreateModelAsync(model));

        var result = await _userService.CreateAsync(model);
        if (!result.Success)
        {
            _notyf.Error(result.ErrorMessage!);
            if (result.Errors != null) foreach (var error in result.Errors) ModelState.AddModelError("", error);
            return View(await MergeCreateModelAsync(model));
        }

        _notyf.Success("User created successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var model = await _userService.BuildEditModelAsync(id, User);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await MergeEditModelAsync(model));

        var result = await _userService.UpdateAsync(model, User);
        if (!result.Success)
        {
            _notyf.Error(result.ErrorMessage!);
            if (result.Errors != null) foreach (var error in result.Errors) ModelState.AddModelError("", error);
            if (result.ErrorMessage == "User not found") return NotFound();
            return View(await MergeEditModelAsync(model));
        }

        _notyf.Success("User updated successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _userService.DeleteAsync(id, User);
        if (!result.Success)
        {
            if (result.ErrorMessage == "User not found") return NotFound();
            _notyf.Error(result.ErrorMessage!);
            return RedirectToAction(nameof(Index));
        }

        _notyf.Success("User deleted successfully");
        return RedirectToAction(nameof(Index));
    }

    private async Task<CreateUserViewModel> MergeCreateModelAsync(CreateUserViewModel model)
    {
        var source = await _userService.BuildCreateModelAsync();
        model.Roles = source.Roles;
        model.Airlines = source.Airlines;
        model.FuelCompanies = source.FuelCompanies;
        model.Airports = source.Airports;
        return model;
    }

    private async Task<EditUserViewModel> MergeEditModelAsync(EditUserViewModel model)
    {
        var source = await _userService.BuildEditModelAsync(model.Id, User);
        if (source != null)
        {
            model.Roles = source.Roles;
            model.Airlines = source.Airlines;
            model.FuelCompanies = source.FuelCompanies;
            model.Airports = source.Airports;
        }

        return model;
    }
}
