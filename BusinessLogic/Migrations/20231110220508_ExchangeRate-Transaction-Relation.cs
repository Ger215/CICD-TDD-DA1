using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExchangeRateTransactionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExchangeRateId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ExchangeRateId",
                table: "Transactions",
                column: "ExchangeRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_ExchangeRates_ExchangeRateId",
                table: "Transactions",
                column: "ExchangeRateId",
                principalTable: "ExchangeRates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_ExchangeRates_ExchangeRateId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ExchangeRateId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ExchangeRateId",
                table: "Transactions");
        }
    }
}
