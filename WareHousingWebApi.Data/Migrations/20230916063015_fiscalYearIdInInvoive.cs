using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class fiscalYearIdInInvoive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fiscalYearId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_fiscalYearId",
                table: "Invoice",
                column: "fiscalYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_FiscalYears_tbl_fiscalYearId",
                table: "Invoice",
                column: "fiscalYearId",
                principalTable: "FiscalYears_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_FiscalYears_tbl_fiscalYearId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_fiscalYearId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "fiscalYearId",
                table: "Invoice");
        }
    }
}
