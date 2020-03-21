using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MahjongBuddy.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    TileType = table.Column<int>(nullable: false),
                    TileValue = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    ImageSmall = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameTiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TileId = table.Column<int>(nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    OpenTileCounter = table.Column<int>(nullable: false),
                    ActiveTileIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTiles_Tiles_TileId",
                        column: x => x.TileId,
                        principalTable: "Tiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTiles_GameId",
                table: "GameTiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTiles_TileId",
                table: "GameTiles",
                column: "TileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTiles");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Tiles");
        }
    }
}
