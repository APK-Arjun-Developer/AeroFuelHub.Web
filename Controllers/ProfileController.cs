using AeroFuelHub.Web.Services.Interfaces;
using AeroFuelHub.Web.ViewModels.Profile;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly INotyfService _notyf;

    public ProfileController(IProfileService profileService, INotyfService notyf)
    {
        _profileService = profileService;
        _notyf = notyf;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _profileService.GetProfileAsync(User);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _profileService.UpdateProfileAsync(model, User);
        _notyf.Success("Profile updated successfully");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult ChangePassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _profileService.ChangePasswordAsync(model, User);
        if (!result.Success)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
            return View(model);
        }

        _notyf.Success("Password changed successfully");
        return RedirectToAction(nameof(Index));
    }
}
