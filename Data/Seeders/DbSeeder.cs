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
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context)
    {
        var emirates =
            context.Airlines.FirstOrDefault(x =>
                x.Name == "Emirates");

        var shell =
            context.FuelCompanies.FirstOrDefault(x =>
                x.Name == "Shell Aviation");

        var dubaiAirport =
            context.Airports.FirstOrDefault(x =>
                x.Code == "DXB");

        var demoUsers = new List<ApplicationUser>
    {
        new()
        {
            FullName = "Airline Executive",

            UserName = "airline@aerofuelhub.com",

            Email = "airline@aerofuelhub.com",

            EmailConfirmed = true,

            AirlineId = emirates?.Id,

            CreatedAt = DateTime.UtcNow
        },

        new()
        {
            FullName = "Fuel Supply Executive",

            UserName = "fuel@aerofuelhub.com",

            Email = "fuel@aerofuelhub.com",

            EmailConfirmed = true,

            FuelCompanyId = shell?.Id,

            CreatedAt = DateTime.UtcNow
        },

        new()
        {
            FullName = "Fuel Coordinator",

            UserName = "coordinator@aerofuelhub.com",

            Email = "coordinator@aerofuelhub.com",

            EmailConfirmed = true,

            AirportId = dubaiAirport?.Id,

            CreatedAt = DateTime.UtcNow
        }
    };

        var roles = new[]
        {
        Roles.AirlineExecutive,
        Roles.FuelSupplyExecutive,
        Roles.FuelCoordinator
    };

        for (int i = 0; i < demoUsers.Count; i++)
        {
            var existing =
                await userManager.FindByEmailAsync(
                    demoUsers[i].Email!);

            if (existing != null)
                continue;

            var result =
                await userManager.CreateAsync(
                    demoUsers[i],
                    "Password@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    demoUsers[i],
                    roles[i]);
            }
        }

        if (dubaiAirport != null)
        {
            var existingCoordinator =
                await userManager.FindByEmailAsync("coordinator@aerofuelhub.com");

            if (existingCoordinator != null && existingCoordinator.AirportId == null)
            {
                existingCoordinator.AirportId = dubaiAirport.Id;
                await userManager.UpdateAsync(existingCoordinator);
            }
        }
    }
}