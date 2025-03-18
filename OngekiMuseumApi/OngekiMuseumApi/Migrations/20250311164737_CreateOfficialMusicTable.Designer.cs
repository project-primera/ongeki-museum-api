﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OngekiMuseumApi.Data;

#nullable disable

namespace OngekiMuseumApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250311164737_CreateOfficialMusicTable")]
    partial class CreateOfficialMusicTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_bin")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("OngekiMuseumApi.Models.OfficialMusic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Artist")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("artist");

                    b.Property<string>("Bonus")
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .HasColumnName("bonus");

                    b.Property<string>("Category")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("category");

                    b.Property<string>("CategoryId")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)")
                        .HasColumnName("category_id");

                    b.Property<string>("ChapId")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)")
                        .HasColumnName("chap_id");

                    b.Property<string>("Chapter")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("chapter");

                    b.Property<string>("CharaId")
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)")
                        .HasColumnName("chara_id");

                    b.Property<string>("Character")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("character");

                    b.Property<string>("Copyright1")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("copyright1");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<string>("Date")
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("date");

                    b.Property<string>("IdString")
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)")
                        .HasColumnName("id_string");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("image_url");

                    b.Property<string>("LevAdv")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("lev_adv");

                    b.Property<string>("LevBas")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("lev_bas");

                    b.Property<string>("LevExc")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("lev_exc");

                    b.Property<string>("LevLnt")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("lev_lnt");

                    b.Property<string>("LevMas")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("lev_mas");

                    b.Property<string>("Lunatic")
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .HasColumnName("lunatic");

                    b.Property<string>("New")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("new");

                    b.Property<string>("Title")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("title");

                    b.Property<string>("TitleSort")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("title_sort");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_official_music");

                    b.HasIndex("IdString")
                        .HasDatabaseName("ix_official_music_id_string");

                    b.ToTable("official_music", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
