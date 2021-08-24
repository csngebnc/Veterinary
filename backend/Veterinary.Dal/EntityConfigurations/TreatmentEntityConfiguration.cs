using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class TreatmentEntityConfiguration : IEntityTypeConfiguration<Treatment>
    {
        public void Configure(EntityTypeBuilder<Treatment> builder)
        {
            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.Treatments)
                .HasForeignKey(x => x.DoctorId);

            builder.HasMany(x => x.TreatmentIntervals)
                .WithOne(x => x.Treatment)
                .HasForeignKey(x => x.TreatmentId);
        }
    }
}
