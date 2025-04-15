using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDropInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Npcs_NpcId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_NpcId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DropRate",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "NpcId",
                table: "Items");

            migrationBuilder.AddColumn<decimal>(
                name: "KillsPerHours",
                table: "Npcs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DropInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NpcId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    DropRate = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropInfos_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DropInfos_Npcs_NpcId",
                        column: x => x.NpcId,
                        principalTable: "Npcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DropInfos_ItemId",
                table: "DropInfos",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DropInfos_NpcId",
                table: "DropInfos",
                column: "NpcId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropInfos");

            migrationBuilder.DropColumn(
                name: "KillsPerHours",
                table: "Npcs");

            migrationBuilder.AddColumn<decimal>(
                name: "DropRate",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NpcId",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_NpcId",
                table: "Items",
                column: "NpcId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Npcs_NpcId",
                table: "Items",
                column: "NpcId",
                principalTable: "Npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
