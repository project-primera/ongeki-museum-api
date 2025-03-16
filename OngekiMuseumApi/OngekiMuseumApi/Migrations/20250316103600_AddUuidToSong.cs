using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OngekiMuseumApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUuidToSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "official_uuid",
                table: "song",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "uuid",
                table: "song",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "official_uuid",
                table: "song");

            migrationBuilder.DropColumn(
                name: "uuid",
                table: "song");
        }
    }
}
