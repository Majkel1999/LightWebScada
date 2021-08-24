﻿// <auto-generated />
using FrontEnd.Areas.Organizations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FrontEnd.Migrations.View
{
    [DbContext(typeof(ViewContext))]
    [Migration("20210729195636_Views")]
    partial class Views
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("common")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("FrontEnd.Areas.Organizations.Data.ViewObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("OrganizationId")
                        .HasColumnType("integer");

                    b.Property<string>("ViewJson")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("viewobject");
                });
#pragma warning restore 612, 618
        }
    }
}