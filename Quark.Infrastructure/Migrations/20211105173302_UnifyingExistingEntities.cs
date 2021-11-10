using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quark.Infrastructure.Migrations
{
    public partial class UnifyingExistingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_LibraryAssets_AssetId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryAssets_AssetId",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryCards_LibraryCardId",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons");

            migrationBuilder.DropTable(
                name: "LibraryAssets");

            migrationBuilder.DropTable(
                name: "LibraryCards");

            migrationBuilder.DropIndex(
                name: "IX_Patrons_LibraryCardId",
                table: "Patrons");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_AssetId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_LibraryCardId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Books_AssetId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LibraryCardId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "LibraryCardId",
                table: "Checkouts");

            migrationBuilder.RenameColumn(
                name: "BarcodeValue",
                table: "Books",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "Books",
                newName: "Copies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Patrons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentFees",
                table: "Patrons",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Issued",
                table: "Patrons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MulitpleCheckoutLimit",
                table: "Patrons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegisterId",
                table: "Patrons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckedOutSince",
                table: "Checkouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Checkouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatronId",
                table: "Checkouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_BookId",
                table: "Checkouts",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_PatronId",
                table: "Checkouts",
                column: "PatronId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Books_BookId",
                table: "Checkouts",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Patrons_PatronId",
                table: "Checkouts",
                column: "PatronId",
                principalTable: "Patrons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Books_BookId",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Patrons_PatronId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_BookId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_PatronId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "CurrentFees",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "Issued",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "MulitpleCheckoutLimit",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "PatronId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Books",
                newName: "BarcodeValue");

            migrationBuilder.RenameColumn(
                name: "Copies",
                table: "Books",
                newName: "AssetId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Patrons",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "LibraryCardId",
                table: "Patrons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckedOutSince",
                table: "Checkouts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "Checkouts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LibraryCardId",
                table: "Checkouts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LibraryAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailabilityId = table.Column<int>(type: "int", nullable: true),
                    Copies = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryAssets_AssetAvailabilityStatuses_AvailabilityId",
                        column: x => x.AvailabilityId,
                        principalTable: "AssetAvailabilityStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LibraryCards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Issued = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_AssetId",
                table: "Checkouts",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_LibraryCardId",
                table: "Checkouts",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AssetId",
                table: "Books",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssets_AvailabilityId",
                table: "LibraryAssets",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_LibraryAssets_AssetId",
                table: "Books",
                column: "AssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryAssets_AssetId",
                table: "Checkouts",
                column: "AssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryCards_LibraryCardId",
                table: "Checkouts",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
