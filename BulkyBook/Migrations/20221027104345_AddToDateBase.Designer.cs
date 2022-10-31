﻿// <auto-generated />
using System;
using BulkyBook.Areas.Admin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BulkyBook.Migrations
{
    [DbContext(typeof(ApplecationDbContext))]
    [Migration("20221027104345_AddToDateBase")]
    partial class AddToDateBase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BulkyBook.Models.Categery", b =>
                {
                    b.Property<double>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float");

                    b.Property<DateTime>("createdDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("displyOrder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("id");

                    b.ToTable("categeries");
                });
#pragma warning restore 612, 618
        }
    }
}
