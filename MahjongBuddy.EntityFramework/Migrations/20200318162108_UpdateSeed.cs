using Microsoft.EntityFrameworkCore.Migrations;

namespace MahjongBuddy.EntityFramework.Migrations
{
    public partial class UpdateSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tiles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Tile 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tiles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
