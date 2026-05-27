using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AeroFuelHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddFuelTransactionIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionNumber",
                table: "FuelTransactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransactions_TransactionDate",
                table: "FuelTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_FuelTransactions_TransactionNumber",
                table: "FuelTransactions",
                column: "TransactionNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuelTransactions_TransactionDate",
                table: "FuelTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FuelTransactions_TransactionNumber",
                table: "FuelTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionNumber",
                table: "FuelTransactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
