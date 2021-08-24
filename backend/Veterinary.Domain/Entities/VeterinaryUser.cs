using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Domain.Entities
{
    public class VeterinaryUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhotoUrl { get; set; }

        public ICollection<Animal> Animals { get; set; }
        public ICollection<Treatment> Treatments { get; set; }
        public ICollection<Holiday> Holidays { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<Appointment> AppointmentsToDoctor { get; set; }
        public ICollection<Appointment> AppointmentsByUsers { get; set; }
    }
}
