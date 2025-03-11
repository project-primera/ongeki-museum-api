using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OngekiMuseumApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateOfficialMusicTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "official_music",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    @new = table.Column<string>(name: "new", type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title_sort = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    artist = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_string = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    chap_id = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    chapter = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    character = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    chara_id = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category_id = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lunatic = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bonus = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    copyright1 = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lev_bas = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lev_adv = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lev_exc = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lev_mas = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lev_lnt = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_url = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_bin")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_official_music", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_bin");

            migrationBuilder.CreateIndex(
                name: "ix_official_music_id_string",
                table: "official_music",
                column: "id_string");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "official_music");
        }
    }
}
