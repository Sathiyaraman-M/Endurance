using Microsoft.EntityFrameworkCore.Migrations;

namespace Quark.Infrastructure.Migrations
{
    public partial class PatronEntitySpellCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MulitpleCheckoutLimit",
                table: "Patrons",
                newName: "MultipleCheckoutLimit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MultipleCheckoutLimit",
                table: "Patrons",
                newName: "MulitpleCheckoutLimit");
        }
    }
}
