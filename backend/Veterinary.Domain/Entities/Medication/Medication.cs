using System;
using System.Collections.Generic;

namespace Veterinary.Domain.Entities.MedicationEntities
{
    public class Medication
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public double Unit { get; set; }
        public double PricePerUnit { get; set; }
        public bool IsInactive { get; set; }

        public ICollection<MedicationRecord> MedicineRecords { get; set; }
    }
}
