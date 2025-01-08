using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignNewsLetter.Migrations
{
    /// <inheritdoc />
    public partial class NameColumnsOnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Subscribes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Subscribes");
        }
    }
}
