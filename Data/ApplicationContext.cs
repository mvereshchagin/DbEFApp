using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public partial class ApplicationContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; } = null!;
        public DbSet<Enclosure> Enclosures { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnimalTypeConfiguration());

            var encID = Guid.NewGuid();
            var enc2ID = Guid.NewGuid();

            modelBuilder.Entity<Enclosure>().HasData(
                new Enclosure() { ID = encID, Number = 1 },
                new Enclosure() { ID = enc2ID, Number = 2 }
                );

            modelBuilder.Entity<Animal>().HasData(
                new Animal("Red")
                {
                    ID = Guid.NewGuid(),
                    Species = "Bull",
                    EnclosureID = encID
                },
            new Animal("Blue")
            {
                ID = Guid.NewGuid(),
                Species = "Bull",
                EnclosureID = enc2ID,
            });
        }

    }
}
