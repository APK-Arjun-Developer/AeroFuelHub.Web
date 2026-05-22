using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.ViewModels.Profile;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AeroFuelHub.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser>
        _userManager;

    private readonly INotyfService _notyf;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        INotyfService notyf)
    {
        _userManager = userManager;

        _notyf = notyf;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user =
            await _userManager.GetUserAsync(User);

        var model = new ProfileViewModel
        {
            FullName = user!.FullName,

            Email = user.Email!
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user =
            await _userManager.GetUserAsync(User);

        user!.FullName = model.FullName;

        user.Email = model.Email;

        user.UserName = model.Email;

        await _userManager.UpdateAsync(user);

        _notyf.Success("Profile updated successfully");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user =
            await _userManager.GetUserAsync(User);

        var result =
            await _userManager.ChangePasswordAsync(
                user!,
                model.CurrentPassword,
                model.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }

            return View(model);
        }

        _notyf.Success("Password changed successfully");

        return RedirectToAction(nameof(Index));
    }
}