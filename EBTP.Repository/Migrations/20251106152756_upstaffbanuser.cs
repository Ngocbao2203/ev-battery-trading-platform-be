using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBTP.Repository.Migrations
{
    /// <inheritdoc />
    public partial class upstaffbanuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BanDate",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BanDescription",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageReport",
                table: "Report",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { 3, "Staff" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                columns: new[] { "BanDate", "BanDescription", "IsBanned" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "BanDate", "BanDescription", "IsBanned", "RoleId" },
                values: new object[] { null, null, false, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "BanDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BanDescription",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageReport",
                table: "Report");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "RoleId",
                value: 2);
        }
    }
}
