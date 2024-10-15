using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class aggiunti_campi_a_tabella_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Data",
                table: "Relations",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Relations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stato",
                table: "Relations",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zhe",
                table: "Relations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "Stato",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "Zhe",
                table: "Relations");
        }
    }
}
