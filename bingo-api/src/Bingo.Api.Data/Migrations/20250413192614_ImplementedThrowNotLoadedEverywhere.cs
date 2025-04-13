using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementedThrowNotLoadedEverywhere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Npcs_NpcId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Npcs_NpcId",
                table: "Items",
                column: "NpcId",
                principalTable: "Npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Npcs_NpcId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "Items",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Npcs_NpcId",
                table: "Items",
                column: "NpcId",
                principalTable: "Npcs",
                principalColumn: "Id");
        }
    }
}
