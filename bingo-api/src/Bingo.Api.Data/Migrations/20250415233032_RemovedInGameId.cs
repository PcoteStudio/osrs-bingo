using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInGameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InGameId",
                table: "Npcs");

            migrationBuilder.DropColumn(
                name: "InGameId",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Npcs_Name",
                table: "Npcs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Npcs_Name",
                table: "Npcs");

            migrationBuilder.DropIndex(
                name: "IX_Items_Name",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "InGameId",
                table: "Npcs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InGameId",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
