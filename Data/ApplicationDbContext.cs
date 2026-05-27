using AeroFuelHub.Web.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AeroFuelHub.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public required DbSet<Airline> Airlines { get; set; }

    public required DbSet<Aircraft> Aircrafts { get; set; }

    public required DbSet<Airport> Airports { get; set; }

    public required DbSet<FuelCompany> FuelCompanies { get; set; }

    public required DbSet<FuelTransaction> FuelTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FuelTransaction>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<Airline>()
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<Aircraft>()
            .Property(x => x.Model)
            .HasMaxLength(200);

        builder.Entity<Airport>()
            .Property(x => x.Name)
            .HasMaxLength(200);

        builder.Entity<FuelCompany>()
            .Property(x => x.Name)
            .HasMaxLength(200);

        builder.Entity<FuelTransaction>()
            .Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<FuelTransaction>()
            .Property(x => x.PricePerLiter)
            .HasColumnType("decimal(18,2)");

        builder.Entity<FuelTransaction>()
            .Property(x => x.FuelQuantity)
            .HasColumnType("decimal(18,2)");

        builder.Entity<FuelTransaction>()
            .HasIndex(x => x.TransactionNumber)
            .IsUnique();

        builder.Entity<FuelTransaction>()
            .HasIndex(x => x.TransactionDate);

        builder.Entity<FuelTransaction>()
            .HasIndex(x => x.AirlineId);

        builder.Entity<FuelTransaction>()
            .HasIndex(x => x.AirportId);

        builder.Entity<FuelTransaction>()
            .HasIndex(x => x.FuelCompanyId);

        builder.Entity<FuelTransaction>()
            .HasOne(x => x.Airline)
            .WithMany()
            .HasForeignKey(x => x.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FuelTransaction>()
            .HasOne(x => x.Aircraft)
            .WithMany()
            .HasForeignKey(x => x.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FuelTransaction>()
            .HasOne(x => x.Airport)
            .WithMany()
            .HasForeignKey(x => x.AirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FuelTransaction>()
            .HasOne(x => x.FuelCompany)
            .WithMany()
            .HasForeignKey(x => x.FuelCompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
