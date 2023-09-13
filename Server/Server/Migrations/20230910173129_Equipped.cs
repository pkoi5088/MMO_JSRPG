using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Equipped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemDbid",
                table: "Item",
                newName: "ItemDbId");

            migrationBuilder.AddColumn<bool>(
                name: "Equipped",
                table: "Item",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipped",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ItemDbId",
                table: "Item",
                newName: "ItemDbid");
        }
    }
}
