using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Dal.EntityConfigurations
{
    public class VaccineEntityConfiguration : IEntityTypeConfiguration<Vaccine>
    {
        public void Configure(EntityTypeBuilder<Vaccine> builder)
        {
            builder.HasMany(x => x.VaccinationRecords)
                .WithOne(x => x.Vaccine)
                .HasForeignKey(x => x.VaccineId);
        }
    }
}
