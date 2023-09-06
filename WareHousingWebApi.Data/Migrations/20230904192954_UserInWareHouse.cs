using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHousingWebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserInWareHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInWareHouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdInWareHouse = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateDateTime = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInWareHouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInWareHouse_Users_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "Users_tbl",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserInWareHouse_Users_tbl_UserIdInWareHouse",
                        column: x => x.UserIdInWareHouse,
                        principalTable: "Users_tbl",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserInWareHouse_wareHouses_tbl_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "wareHouses_tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInWareHouse_UserId",
                table: "UserInWareHouse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInWareHouse_UserIdInWareHouse",
                table: "UserInWareHouse",
                column: "UserIdInWareHouse");

            migrationBuilder.CreateIndex(
                name: "IX_UserInWareHouse_WareHouseId",
                table: "UserInWareHouse",
                column: "WareHouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInWareHouse");
        }
    }
}
