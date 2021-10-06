using System;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Shared.Enums;

namespace Veterinary.Domain.Entities.AppointmentEntities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public Guid DoctorId { get; set; }
        public VeterinaryUser Doctor { get; set; }

        public Guid OwnerId { get; set; }
        public VeterinaryUser Owner { get; set; }

        public Guid? AnimalId { get; set; }
        public Animal Animal { get; set; }

        public string Reasons { get; set; }
        public AppointmentStatusEnum Status { get; set; }
    }
}
