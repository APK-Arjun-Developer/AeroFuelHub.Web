using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeroFuelHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FuelTransactions",
                newName: "TransactionNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Aircrafts",
                newName: "AircraftCode");

            migrationBuilder.AddColumn<int>(
                name: "AircraftId",
                table: "FuelTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "FuelTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AirportId",
                table: "FuelTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FuelTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "FuelTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FuelTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "FuelTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "FuelTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FuelCompanyId",
                table: "FuelTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FuelQuantity",
                table: "FuelTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FuelTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerLiter",
                table: "FuelTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "FuelTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FuelTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "FuelTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "FuelTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FuelTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "FuelTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FuelCompanies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FuelCompanies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "FuelCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FuelCompanies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "FuelCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FuelCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FuelCompanies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "FuelCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Airports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Airports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Airports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Airports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Airports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Airports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Airports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Airports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airlines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Airlines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Airlines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Airlines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Airlines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Airlines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Airlines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Airlines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "Aircrafts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Aircrafts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Aircrafts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Aircrafts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Aircrafts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Aircrafts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Aircrafts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Aircrafts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Aircrafts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransactions_AircraftId",
                table: "FuelTransactions",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransactions_AirlineId",
                table: "FuelTransactions",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransactions_AirportId",
                table: "FuelTransactions",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransactions_FuelCompanyId",
                table: "FuelTransactions",
                column: "FuelCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AirlineId",
                table: "Aircrafts",
                column: "AirlineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelTransactions_Aircrafts_AircraftId",
                table: "FuelTransactions",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelTransactions_Airlines_AirlineId",
                table: "FuelTransactions",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelTransactions_Airports_AirportId",
                table: "FuelTransactions",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FuelTransactions_FuelCompanies_FuelCompanyId",
                table: "FuelTransactions",
                column: "FuelCompanyId",
                principalTable: "FuelCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aircrafts_Airlines_AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelTransactions_Aircrafts_AircraftId",
                table: "FuelTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelTransactions_Airlines_AirlineId",
                table: "FuelTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelTransactions_Airports_AirportId",
                table: "FuelTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_FuelTransactions_FuelCompanies_FuelCompanyId",
                table: "FuelTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FuelTransactions_AircraftId",
                table: "FuelTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FuelTransactions_AirlineId",
                table: "FuelTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FuelTransactions_AirportId",
                table: "FuelTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FuelTransactions_FuelCompanyId",
                table: "FuelTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Aircrafts_AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "AircraftId",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "AirportId",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "FuelCompanyId",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "FuelQuantity",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "PricePerLiter",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FuelTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FuelCompanies");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Aircrafts");

            migrationBuilder.RenameColumn(
                name: "TransactionNumber",
                table: "FuelTransactions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AircraftCode",
                table: "Aircrafts",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FuelCompanies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airlines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
