using System;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Domain.Entities.MedicationEntities
{
    public class MedicationRecord
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }

        public Guid MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public Guid MedicationId { get; set; }
        public Medication Medication { get; set; }
    }
}
