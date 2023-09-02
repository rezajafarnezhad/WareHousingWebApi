using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductLocationId",
                table: "Inventories_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_tbl_ProductLocationId",
                table: "Inventories_tbl",
                column: "ProductLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_tbl_ProductLocation_tbl_ProductLocationId",
                table: "Inventories_tbl",
                column: "ProductLocationId",
                principalTable: "ProductLocation_tbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_tbl_ProductLocation_tbl_ProductLocationId",
                table: "Inventories_tbl");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_tbl_ProductLocationId",
                table: "Inventories_tbl");

            migrationBuilder.DropColumn(
                name: "ProductLocationId",
                table: "Inventories_tbl");
        }
    }
}
