using System;
using System.Collections.Generic;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Domain.Entities.AnimalEntities
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Weight { get; set; }
        public string Sex { get; set; }

        public Guid SpeciesId { get; set; }
        public AnimalSpecies Species { get; set; }
        public string? SubSpecies { get; set; }

        public Guid OwnerId { get; set; }
        public VeterinaryUser Owner { get; set; }

        public string PhotoUrl { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<VaccineRecord> VaccinationRecords { get; set; }

    }
}
