using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data
{
    [Table("Creatures")]
    public class Animal
    {
        public Animal(string name)
        {
            this.name = name;
        }

        public Guid ID { get; set; }

        private string? name;

        public string? Species { get; set; }

        [MaxLength(50)]
        public string? Name
        {
            get => this.name;
            set
            {
                if (value is not null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("");
                this.name = value;
            }
        }

        // [NotMapped]
        public Enclosure Enclosure { get; set; }

        [ForeignKey("EnclosureID")]
        public Guid EnclosureID { get; set; }
    }

    public class AnimalTypeConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnName("FullName")
                .HasMaxLength(50)
                .IsRequired()
                .HasField("name");

            builder.HasIndex(p => p.Name);

            builder
                .HasOne(p => p.Enclosure)
                .WithMany(p => p.Animals)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
