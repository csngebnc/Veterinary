using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class HolidayEntityConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.Holidays)
                .HasForeignKey(x => x.DoctorId);
        }
    }
}
