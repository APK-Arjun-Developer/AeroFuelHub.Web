using AeroFuelHub.Web.Data;
using AeroFuelHub.Web.Data.Seeders;
using AeroFuelHub.Web.Models.Entities;
using AeroFuelHub.Web.Repositories.Implementations;
using AeroFuelHub.Web.Repositories.Interfaces;
using AeroFuelHub.Web.Services.Implementations;
using AeroFuelHub.Web.Services.Interfaces;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 3;

    config.IsDismissable = true;

    config.Position = NotyfPosition.TopRight;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IFuelTransactionRepository, FuelTransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IFuelTransactionService, FuelTransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IExportService, ExportService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();

    var context =
        services.GetRequiredService<ApplicationDbContext>();

    await DbSeeder.SeedRolesAsync(roleManager);

    await DbSeeder.SeedAdminUserAsync(userManager);

    await DbSeeder.SeedDemoUsersAsync(userManager, context);

    await MasterDataSeeder.SeedAsync(context);
}

app.Run();