using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SaveDropEhc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Ehc",
                table: "DropInfos",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ehc",
                table: "DropInfos");
        }
    }
}
