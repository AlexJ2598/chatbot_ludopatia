﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using chatbot_ludo.Web.Data;

#nullable disable

namespace chatbot_ludo.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240913055704_RenameFechaCreacionColumn")]
    partial class RenameFechaCreacionColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("chatbot_ludo.Web.Data.Entities.Consejo", b =>
                {
                    b.Property<int>("ID_Consejo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Consejo"), 1L, 1);

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Fecha_Creacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("Grado_Recomendacion")
                        .HasColumnType("int");

                    b.Property<string>("Texto_Consejo")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("ID_Consejo");

                    b.ToTable("Consejos");
                });
#pragma warning restore 612, 618
        }
    }
}
