using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBTP.Repository.Migrations
{
    /// <inheritdoc />
    public partial class fixtablelisting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Listing",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Listing");
        }
    }
}
