using AeroFuelHub.Web.Constants;
using AeroFuelHub.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace AeroFuelHub.Web.Data.Seeders;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            Roles.Admin,
            Roles.AirlineExecutive,
            Roles.FuelSupplyExecutive,
            Roles.FuelCoordinator
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdminUserAsync(
        UserManager<ApplicationUser> userManager)
    {
        var email = "admin@aerofuelhub.com";

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return;

        var user = new ApplicationUser
        {
            FullName = "System Administrator",
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Admin@123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.Admin);
        }
    }

    public static async Task SeedDemoUsersAsync(
    UserManager<ApplicationUser> userManager)
{
    var demoUsers = new List<(string FullName,
                               string Email,
                               string Role)>
    {
        (
            "Airline Executive",
            "airline@aerofuelhub.com",
            Roles.AirlineExecutive
        ),

        (
            "Fuel Supply Executive",
            "fuel@aerofuelhub.com",
            Roles.FuelSupplyExecutive
        ),

        (
            "Fuel Coordinator",
            "coordinator@aerofuelhub.com",
            Roles.FuelCoordinator
        )
    };

    foreach (var demo in demoUsers)
    {
        var existingUser =
            await userManager.FindByEmailAsync(demo.Email);

        if (existingUser != null)
            continue;

        var user = new ApplicationUser
        {
            FullName = demo.FullName,

            UserName = demo.Email,

            Email = demo.Email,

            EmailConfirmed = true,

            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(
            user,
            "Password@123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(
                user,
                demo.Role);
        }
    }
}
}