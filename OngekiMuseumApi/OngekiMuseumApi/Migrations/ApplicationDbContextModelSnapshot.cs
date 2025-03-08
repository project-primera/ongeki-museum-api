﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OngekiMuseumApi.Data;

#nullable disable

namespace OngekiMuseumApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_bin")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OngekiMuseumApi.Models.OfficialMusic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Artist")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("artist");

                    b.Property<string>("Bonus")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)")
                        .HasColumnName("bonus");

                    b.Property<string>("Category")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasColumnName("category");

                    b.Property<string>("CategoryId")
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)")
                        .HasColumnName("category_id");

                    b.Property<string>("ChapId")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("chap_id");

                    b.Property<string>("Chapter")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("chapter");

                    b.Property<string>("CharaId")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)")
                        .HasColumnName("chara_id");

                    b.Property<string>("Character")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("character");

                    b.Property<string>("Copyright1")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("copyright1");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Date")
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("date");

                    b.Property<string>("IdString")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("id_string");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("image_url");

                    b.Property<string>("LevAdv")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("lev_adv");

                    b.Property<string>("LevBas")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("lev_bas");

                    b.Property<string>("LevExc")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("lev_exc");

                    b.Property<string>("LevLnt")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("lev_lnt");

                    b.Property<string>("LevMas")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("lev_mas");

                    b.Property<string>("Lunatic")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)")
                        .HasColumnName("lunatic");

                    b.Property<string>("New")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("new");

                    b.Property<string>("Title")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("title");

                    b.Property<string>("TitleSort")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("title_sort");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
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
