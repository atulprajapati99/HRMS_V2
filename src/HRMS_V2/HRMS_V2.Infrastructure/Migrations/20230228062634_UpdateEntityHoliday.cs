using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityHoliday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "created_by",
                table: "Holiday",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by_name",
                table: "Holiday",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "Holiday",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "modified_by",
                table: "Holiday",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "modified_by_name",
                table: "Holiday",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_date",
                table: "Holiday",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "record_status",
                table: "Holiday",
                type: "char(1)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "created_by_name",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "modified_by",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "modified_by_name",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "modified_date",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "record_status",
                table: "Holiday");
        }
    }
}
