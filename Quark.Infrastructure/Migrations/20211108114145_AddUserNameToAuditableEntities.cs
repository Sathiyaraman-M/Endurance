using Microsoft.EntityFrameworkCore.Migrations;

namespace Quark.Infrastructure.Migrations
{
    public partial class AddUserNameToAuditableEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                schema: "Identity",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                schema: "Identity",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                schema: "Identity",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                schema: "Identity",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Patrons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                table: "Patrons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                table: "Designations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Checkouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                table: "Checkouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Books");
        }
    }
}
