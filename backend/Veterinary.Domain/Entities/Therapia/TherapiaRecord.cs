using System;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Domain.Entities.TherapiaEntities
{
    public class TherapiaRecord
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }

        public Guid MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public Guid TherapiaId { get; set; }
        public Therapia Therapia { get; set; }

    }
}
