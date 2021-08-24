using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class MedicalRecordEntityConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasOne(x => x.Doctor)
                .WithMany()
                .HasForeignKey(x => x.DoctorId);

            builder.Property(x => x.OwnerId)
                .IsRequired(false);

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.MedicalRecords)
                .HasForeignKey(x => x.OwnerId)
                .IsRequired(false);
            
            builder.Property(x => x.AnimalId)
                .IsRequired(false);

            builder.HasOne(x => x.Animal)
                .WithMany(x => x.MedicalRecords)
                .HasForeignKey(x => x.AnimalId)
                .IsRequired(false);

            builder.HasMany(x => x.Photos)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey(x => x.MedicalRecordId);

            builder.HasMany(x => x.TherapiaRecords)
                .WithOne(x => x.MedicalRecord)
                .HasForeignKey(x => x.MedicalRecordId);
        }
    }
}
