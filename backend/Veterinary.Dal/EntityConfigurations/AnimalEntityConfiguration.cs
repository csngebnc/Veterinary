using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.AnimalEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class AnimalEntityConfiguration : IEntityTypeConfiguration<Animal>
    {

        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.HasOne(x => x.Species)
                .WithMany(x => x.Animals)
                .HasForeignKey(x => x.SpeciesId);

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.Animals)
                .HasForeignKey(x => x.OwnerId);

            builder.HasMany(x => x.VaccinationRecords)
                .WithOne(x => x.Animal)
                .HasForeignKey(x => x.AnimalId);
        }
    }
}
