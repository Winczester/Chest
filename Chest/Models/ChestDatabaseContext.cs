using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chest.Models
{
    public class ChestDatabaseContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ManufacturerCategory> ManufacturerCategories { get; set; }
        public DbSet<ShopBasket> ShopBasket { get; set; }

        public ChestDatabaseContext(DbContextOptions<ChestDatabaseContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Goods>().HasKey(goods => goods.ID);
            modelBuilder.Entity<Goods>()
                .HasOne(goods => goods.Category)
                .WithMany(category => category.Goods)
                .HasForeignKey(goods => goods.CategoryID);
            modelBuilder.Entity<Goods>()
                .HasOne(goods => goods.Manufacturer)
                .WithMany(manufacturer => manufacturer.Goods)
                .HasForeignKey(goods => goods.ManufacturerID);

            modelBuilder.Entity<Category>().HasKey(category => category.ID);

            modelBuilder.Entity<Manufacturer>().HasKey(manufacturer => manufacturer.ID);

            modelBuilder.Entity<ManufacturerCategory>()
                .HasOne(manuCat => manuCat.Manufacturer)
                .WithMany(manufacturer => manufacturer.ManufacturerCategories)
                .HasForeignKey(manuCat => manuCat.ManufacturerID);
            modelBuilder.Entity<ManufacturerCategory>()
                .HasOne(manuCat => manuCat.Category)
                .WithMany(category => category.ManufacturerCategories)
                .HasForeignKey(manuCat => manuCat.CategoryID);
            modelBuilder.Entity<ManufacturerCategory>().HasKey(manuCat => new {manuCat.CategoryID, manuCat.ManufacturerID});

            modelBuilder.Entity<ShopBasket>().HasKey(basket => basket.Id);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }



    }
}
