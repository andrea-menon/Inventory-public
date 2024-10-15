using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class aggiunti_campi_zhe_stato_data_note_a_devices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Data",
                table: "Devices",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stato",
                table: "Devices",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zhe",
                table: "Devices",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Stato",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Zhe",
                table: "Devices");
        }
    }
}
