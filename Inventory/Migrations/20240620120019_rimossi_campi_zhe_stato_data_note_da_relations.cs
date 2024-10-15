using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class rimossi_campi_zhe_stato_data_note_da_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zhe",
                table: "Relations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
