using CoffeeTime.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CoffeeTime.Data.EF
{
    public class CoffeeDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Coffee> Coffees { get; set; }
        public DbSet<CoffeeData> CoffeeData { get; set; }
        public DbSet<VolumeData> VolumeData { get; set; }
        public DbSet<PriceData> PriceData { get; set; }
        public DbSet<CheckTime> CheckTimes { get; set; }

        public CoffeeDbContext(DbContextOptions<CoffeeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var priceData = new List<PriceData>()
            {
                new PriceData { Id = 1, Price = 3.5m, VolumeDataId = 1 },
                new PriceData { Id = 2, Price = 4m, VolumeDataId = 2 },
                new PriceData { Id = 3, Price = 4.5m, VolumeDataId = 3 },
                new PriceData { Id = 4, Price = 4.4m, VolumeDataId = 4 },
                new PriceData { Id = 5, Price = 5.2m, VolumeDataId = 5 },
                new PriceData { Id = 6, Price = 4.2m, VolumeDataId = 6 },
                new PriceData { Id = 7, Price = 5.4m, VolumeDataId = 7 }
            };

            var volumeData = new List<VolumeData>()
            {
                new VolumeData { Id = 1, Volume = "0.133 L", CoffeeDataId = 1 },
                new VolumeData { Id = 2, Volume = "0.250 L", CoffeeDataId = 1 },
                new VolumeData { Id = 3, Volume = "0.500 L", CoffeeDataId = 1 },
                new VolumeData { Id = 4, Volume = "0.133 L", CoffeeDataId = 2 },
                new VolumeData { Id = 5, Volume = "0.250 L", CoffeeDataId = 2 },
                new VolumeData { Id = 6, Volume = "0.133 L", CoffeeDataId = 3 },
                new VolumeData { Id = 7, Volume = "0.250 L", CoffeeDataId = 3 }
            };

            var checkTime = new CheckTime { Id = 1, DeletionTime = new DateTime(2021, 5, 12, 19, 00, 00) };

            modelBuilder.Entity<CoffeeData>()
                .HasData(
                    new CoffeeData
                    {
                        Id = 1,
                        Name = "Americano",
                        Image = "/img/americano.png",
                    },

                    new CoffeeData
                    {
                        Id = 2,
                        Name = "Latte",
                        Image = "/img/latte.png",
                    },

                    new CoffeeData
                    {
                        Id = 3,
                        Name = "Espresso",
                        Image = "/img/espresso.png",
                    }
                );

            modelBuilder.Entity<VolumeData>().HasData(volumeData);
            modelBuilder.Entity<PriceData>().HasData(priceData);
            modelBuilder.Entity<CheckTime>().HasData(checkTime);
        }
    }
}
