using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersApp.Migrations
{
    /// <inheritdoc />
    public partial class Test4567 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
       
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorites");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorites",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_ProductId",
                table: "Favorites",
                columns: new[] { "UserId", "ProductId" },
                unique: true);
        }
    }
}
