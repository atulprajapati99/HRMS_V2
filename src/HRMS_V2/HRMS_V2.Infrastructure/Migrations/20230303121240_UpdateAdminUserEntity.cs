using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_working",
                table: "AspNetUsers",
                newName: "is_enable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_enable",
                table: "AspNetUsers",
                newName: "is_working");
        }
    }
}
