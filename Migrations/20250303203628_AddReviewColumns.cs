using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersApp.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Reviews",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Reviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reviews",
                newName: "DatePosted");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Reviews",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
