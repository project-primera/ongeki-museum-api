using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OngekiMuseumApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateOfficialMusicTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "official_music",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    @new = table.Column<string>(name: "new", type: "character varying(3)", maxLength: 3, nullable: true),
                    date = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    title_sort = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    artist = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    id_string = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    chap_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    chapter = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    character = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    chara_id = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    category = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    category_id = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    lunatic = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    bonus = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    copyright1 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    lev_bas = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    lev_adv = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    lev_exc = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    lev_mas = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    lev_lnt = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    image_url = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_official_music", x => x.id);
                });

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
