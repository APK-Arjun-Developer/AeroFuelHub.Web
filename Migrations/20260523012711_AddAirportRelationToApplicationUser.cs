using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeroFuelHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAirportRelationToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirportId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AirlineId",
                table: "AspNetUsers",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AirportId",
                table: "AspNetUsers",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FuelCompanyId",
                table: "AspNetUsers",
                column: "FuelCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Airlines_AirlineId",
                table: "AspNetUsers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Airports_AirportId",
                table: "AspNetUsers",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FuelCompanies_FuelCompanyId",
                table: "AspNetUsers",
                column: "FuelCompanyId",
                principalTable: "FuelCompanies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Airlines_AirlineId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Airports_AirportId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FuelCompanies_FuelCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AirlineId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AirportId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FuelCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AirportId",
                table: "AspNetUsers");
        }
    }
}
