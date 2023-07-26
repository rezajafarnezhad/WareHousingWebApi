using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class inventoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventories_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FiscalYearId = table.Column<int>(type: "int", nullable: false),
                    ProductCountMain = table.Column<int>(type: "int", nullable: false),
                    ProductWastage = table.Column<int>(type: "int", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationType = table.Column<byte>(type: "tinyint", nullable: false),
                    ExpireData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories_tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_tbl_FiscalYears_tbl_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalTable: "FiscalYears_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_tbl_Products_tbl_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products_tbl",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_tbl_Users_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "Users_tbl",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Inventories_tbl_wareHouses_tbl_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "wareHouses_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_tbl_FiscalYearId",
                table: "Inventories_tbl",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_tbl_ProductId",
                table: "Inventories_tbl",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_tbl_UserId",
                table: "Inventories_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_tbl_WareHouseId",
                table: "Inventories_tbl",
                column: "WareHouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories_tbl");
        }
    }
}
