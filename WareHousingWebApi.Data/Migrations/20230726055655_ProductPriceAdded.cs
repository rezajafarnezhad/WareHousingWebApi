using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductPriceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPrices_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchasePrice = table.Column<int>(type: "int", nullable: false),
                    SalesPrice = table.Column<int>(type: "int", nullable: false),
                    CoverPrice = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FiscalYearId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices_tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_tbl_FiscalYears_tbl_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalTable: "FiscalYears_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPrices_tbl_Products_tbl_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products_tbl",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPrices_tbl_Users_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "Users_tbl",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_tbl_FiscalYearId",
                table: "ProductPrices_tbl",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_tbl_ProductId",
                table: "ProductPrices_tbl",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_tbl_UserId",
                table: "ProductPrices_tbl",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPrices_tbl");
        }
    }
}
