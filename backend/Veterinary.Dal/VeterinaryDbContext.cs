using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Domain.Entities.MedicationEntities;
using Veterinary.Domain.Entities.TherapiaEntities;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Dal.Data
{
    public class VeterinaryDbContext : IdentityDbContext<VeterinaryUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalSpecies> AnimalSpecies { get; set; }

        public DbSet<Vaccine> Vaccines {  get; set; }
        public DbSet<Medication> Medications {  get; set; }
        public DbSet<Therapia> Therapias {  get; set; }

        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentInterval> TreatmentIntervals { get; set; }

        public DbSet<VaccineRecord> VaccineRecords { get; set; }

        public VeterinaryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
