using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class StartedMultiLayerBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tiles",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tiles",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Tiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Tiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BoardEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_BoardId",
                table: "Tiles",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_EventId",
                table: "Tiles",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventId",
                table: "Events",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_BoardEntity_EventId",
                table: "Events",
                column: "EventId",
                principalTable: "BoardEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MultiLayerBoards_BoardEntity_Id",
                table: "MultiLayerBoards",
                column: "Id",
                principalTable: "BoardEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_BoardEntity_BoardId",
                table: "Tiles",
                column: "BoardId",
                principalTable: "BoardEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Events_EventId",
                table: "Tiles",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_BoardEntity_EventId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_MultiLayerBoards_BoardEntity_Id",
                table: "MultiLayerBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_BoardEntity_BoardId",
                table: "Tiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Events_EventId",
                table: "Tiles");

            migrationBuilder.DropTable(
                name: "BoardEntity");

            migrationBuilder.DropIndex(
                name: "IX_Tiles_BoardId",
                table: "Tiles");

            migrationBuilder.DropIndex(
                name: "IX_Tiles_EventId",
                table: "Tiles");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tiles");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Tiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "Tiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);
        }
    }
}
