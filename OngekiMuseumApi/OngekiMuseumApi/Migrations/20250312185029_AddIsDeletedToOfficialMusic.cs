using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OngekiMuseumApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToOfficialMusic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "official_music",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "official_music");
        }
    }
}
