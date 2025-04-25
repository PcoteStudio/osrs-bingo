using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDepth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Depth",
                table: "MultiLayerBoards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depth",
                table: "MultiLayerBoards");
        }
    }
}
