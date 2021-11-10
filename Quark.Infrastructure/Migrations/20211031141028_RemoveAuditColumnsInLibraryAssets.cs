using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quark.Infrastructure.Migrations
{
    public partial class RemoveAuditColumnsInLibraryAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_LibraryAssets_AssetId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "CheckoutHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "LibraryAssets");

            migrationBuilder.AlterColumn<int>(
                name: "AssetId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_LibraryAssets_AssetId",
                table: "Books",
                column: "AssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_LibraryAssets_AssetId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LibraryAssets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "LibraryAssets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "LibraryAssets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "LibraryAssets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssetId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CheckoutHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: true),
                    CheckedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LibraryCardId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckoutHistories_LibraryAssets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "LibraryAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckoutHistories_LibraryCards_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "LibraryCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutHistories_AssetId",
                table: "CheckoutHistories",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutHistories_LibraryCardId",
                table: "CheckoutHistories",
                column: "LibraryCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_LibraryAssets_AssetId",
                table: "Books",
                column: "AssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
