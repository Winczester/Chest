﻿// <auto-generated />
using Chest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chest.Migrations
{
    [DbContext(typeof(ChestDatabaseContext))]
    [Migration("20190915162335_BaskedGoodsListRemoved")]
    partial class BaskedGoodsListRemoved
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Chest.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Chest.Models.Goods", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryID");

                    b.Property<int>("ManufacturerID");

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ManufacturerID");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("Chest.Models.Manufacturer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("Chest.Models.ManufacturerCategory", b =>
                {
                    b.Property<int>("CategoryID");

                    b.Property<int>("ManufacturerID");

                    b.HasKey("CategoryID", "ManufacturerID");

                    b.HasIndex("ManufacturerID");

                    b.ToTable("ManufacturerCategories");
                });

            modelBuilder.Entity("Chest.Models.ShopBasket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GoodsId");

                    b.HasKey("Id");

                    b.ToTable("ShopBasket");
                });

            modelBuilder.Entity("Chest.Models.Goods", b =>
                {
                    b.HasOne("Chest.Models.Category", "Category")
                        .WithMany("Goods")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Chest.Models.Manufacturer", "Manufacturer")
                        .WithMany("Goods")
                        .HasForeignKey("ManufacturerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Chest.Models.ManufacturerCategory", b =>
                {
                    b.HasOne("Chest.Models.Category", "Category")
                        .WithMany("ManufacturerCategories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Chest.Models.Manufacturer", "Manufacturer")
                        .WithMany("ManufacturerCategories")
                        .HasForeignKey("ManufacturerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
