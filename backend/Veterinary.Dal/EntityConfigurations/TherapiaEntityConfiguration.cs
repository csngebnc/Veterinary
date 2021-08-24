using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class TherapiaEntityConfiguration : IEntityTypeConfiguration<Therapia>
    {
        public void Configure(EntityTypeBuilder<Therapia> builder)
        {
            builder.HasMany(x => x.TherapiaRecords)
                .WithOne(x => x.Therapia)
                .HasForeignKey(x => x.TherapiaId);
        }
    }
}
