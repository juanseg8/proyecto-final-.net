using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPi.Migrations
{
    /// <inheritdoc />
    public partial class editsalemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PorudctId",
                table: "SaleItems",
                newName: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "SaleItems",
                newName: "PorudctId");
        }
    }
}
