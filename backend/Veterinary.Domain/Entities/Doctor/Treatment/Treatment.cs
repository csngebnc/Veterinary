using System;
using System.Collections.Generic;

namespace Veterinary.Domain.Entities.Doctor.TreatmentEntities
{
    public class Treatment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool IsInactive { get; set; }

        public Guid DoctorId { get; set; }
        public VeterinaryUser Doctor { get; set; }

        public ICollection<TreatmentInterval> TreatmentIntervals { get; set; }
    }
}
