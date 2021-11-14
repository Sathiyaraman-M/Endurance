using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quark.Infrastructure.Migrations
{
    public partial class RemoveCopiesFromBookEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Copies",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Availability",
                table: "Books",
                newName: "IsAvailable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Books",
                newName: "Availability");

            migrationBuilder.AddColumn<int>(
                name: "Copies",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
