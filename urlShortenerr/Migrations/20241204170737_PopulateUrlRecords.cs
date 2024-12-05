using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace urlShortenerr.Migrations
{
    /// <inheritdoc />
    public partial class PopulateUrlRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanEdit",
                table: "UrlRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanEdit",
                table: "UrlRecords");
        }
    }
}
