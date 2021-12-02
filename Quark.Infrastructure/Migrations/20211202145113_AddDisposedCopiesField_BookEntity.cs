using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quark.Infrastructure.Migrations
{
    public partial class AddDisposedCopiesField_BookEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisposedCopies",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisposedCopies",
                table: "Books");
        }
    }
}
