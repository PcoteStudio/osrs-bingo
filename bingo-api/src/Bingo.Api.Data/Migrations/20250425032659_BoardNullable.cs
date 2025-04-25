using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class BoardNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_BoardEntity_EventId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Events",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "Events",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_BoardEntity_EventId",
                table: "Events",
                column: "EventId",
                principalTable: "BoardEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_BoardEntity_EventId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_BoardEntity_EventId",
                table: "Events",
                column: "EventId",
                principalTable: "BoardEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
