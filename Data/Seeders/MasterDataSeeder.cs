using AeroFuelHub.Web.Models.Entities;

namespace AeroFuelHub.Web.Data.Seeders;

public static class MasterDataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!context.Airlines.Any())
        {
            context.Airlines.AddRange(
                new Airline { Name = "Emirates" },
                new Airline { Name = "Qatar Airways" },
                new Airline { Name = "Air India" }
            );
        }

        if (!context.Airports.Any())
        {
            context.Airports.AddRange(
                new Airport
                {
                    Name = "Dubai International Airport",
                    Code = "DXB"
                },
                new Airport
                {
                    Name = "Chennai International Airport",
                    Code = "MAA"
                },
                new Airport
                {
                    Name = "Doha International Airport",
                    Code = "DOH"
                }
            );
        }

        if (!context.FuelCompanies.Any())
        {
            context.FuelCompanies.AddRange(
                new FuelCompany
                {
                    Name = "Shell Aviation"
                },
                new FuelCompany
                {
                    Name = "Bharat Petroleum Aviation"
                }
            );
        }

        await context.SaveChangesAsync();

        if (!context.Aircrafts.Any())
        {
            var emirates = context.Airlines.First(x => x.Name == "Emirates");

            var qatar = context.Airlines.First(x => x.Name == "Qatar Airways");

            var airIndia = context.Airlines.First(x => x.Name == "Air India");

            context.Aircrafts.AddRange(
                new Aircraft
                {
                    Model = "Airbus A320",
                    AircraftCode = "EK-A320",
                    AirlineId = emirates.Id
                },
                new Aircraft
                {
                    Model = "Boeing 777",
                    AircraftCode = "QR-B777",
                    AirlineId = qatar.Id
                },
                new Aircraft
                {
                    Model = "Boeing 737",
                    AircraftCode = "AI-B737",
                    AirlineId = airIndia.Id
                }
            );
        }

        await context.SaveChangesAsync();
    }
}