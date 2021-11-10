using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quark.Infrastructure.Migrations
{
    public partial class AddExpectedCheckInDateForCheckoutEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetAvailabilityStatuses");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedCheckInDate",
                table: "Checkouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedCheckInDate",
                table: "Checkouts");

            migrationBuilder.CreateTable(
                name: "AssetAvailabilityStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAvailabilityStatuses", x => x.Id);
                });
        }
    }
}
