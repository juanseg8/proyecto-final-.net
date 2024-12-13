using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPi.Migrations
{
    /// <inheritdoc />
    public partial class editsale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Withdraw",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Withdraw",
                table: "Sales");
        }
    }
}
