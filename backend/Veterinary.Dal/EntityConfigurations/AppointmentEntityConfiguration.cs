using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Veterinary.Domain.Entities.AppointmentEntities;

namespace Veterinary.Dal.EntityConfigurations
{
    public class AppointmentEntityConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(x => x.Treatment)
                .WithMany()
                .HasForeignKey(x => x.TreatmentId)
                .OnDelete(DeleteBehavior.Restrict); ;

            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.AppointmentsByUsers)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.AppointmentsToDoctor)
                .HasForeignKey(x => x.OwnerId);

            builder.Property(x => x.AnimalId)
                .IsRequired(false);

            builder.HasOne(x => x.Animal)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.AnimalId)
                .IsRequired(false);
        }
    }
}
