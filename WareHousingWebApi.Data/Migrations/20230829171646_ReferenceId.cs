using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "Inventories_tbl",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Inventories_tbl");
        }
    }
}
