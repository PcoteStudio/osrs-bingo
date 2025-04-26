using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_MultiLayerBoards_MultiLayerBoardId",
                table: "Tiles");

            migrationBuilder.RenameColumn(
                name: "MultiLayerBoardId",
                table: "Tiles",
                newName: "BoardLayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tiles_MultiLayerBoardId",
                table: "Tiles",
                newName: "IX_Tiles_BoardLayerId");

            migrationBuilder.CreateTable(
                name: "BoardLayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardLayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardLayers_MultiLayerBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "MultiLayerBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardLayers_BoardId",
                table: "BoardLayers",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_BoardLayers_BoardLayerId",
                table: "Tiles",
                column: "BoardLayerId",
                principalTable: "BoardLayers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_BoardLayers_BoardLayerId",
                table: "Tiles");

            migrationBuilder.DropTable(
                name: "BoardLayers");

            migrationBuilder.RenameColumn(
                name: "BoardLayerId",
                table: "Tiles",
                newName: "MultiLayerBoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Tiles_BoardLayerId",
                table: "Tiles",
                newName: "IX_Tiles_MultiLayerBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_MultiLayerBoards_MultiLayerBoardId",
                table: "Tiles",
                column: "MultiLayerBoardId",
                principalTable: "MultiLayerBoards",
                principalColumn: "Id");
        }
    }
}
