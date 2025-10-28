using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBTP.Repository.Migrations
{
    /// <inheritdoc />
    public partial class fixtableimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Brand",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Brand");
        }
    }
}
