using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class MedicationEntityConfiguration : IEntityTypeConfiguration<Medication>
    {
        public void Configure(EntityTypeBuilder<Medication> builder)
        {
            builder.HasMany(x => x.MedicationRecords)
                .WithOne(x => x.Medication)
                .HasForeignKey(x => x.MedicationId);
        }
    }
}
