using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductLocation_tbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductLocationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLocation_tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLocation_tbl_Users_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "Users_tbl",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_ProductLocation_tbl_wareHouses_tbl_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "wareHouses_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductLocation_tbl_UserId",
                table: "ProductLocation_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLocation_tbl_WareHouseId",
                table: "ProductLocation_tbl",
                column: "WareHouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductLocation_tbl");
        }
    }
}
