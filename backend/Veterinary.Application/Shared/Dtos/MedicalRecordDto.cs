using System;
using System.Collections.Generic;

namespace Veterinary.Application.Shared.Dtos
{
    public class MedicalRecordDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }

        public Guid? AnimalId { get; set; }
        public string AnimalName { get; set; }

        public string OwnerEmail { get; set; }
        public Guid? OwnerId { get; set; }
        public string OwnerName { get; set; }

        public string HtmlContent { get; set; }

        public List<string> PhotoUrls { get; set; }
        public ICollection<MedicationRecordOnRecord> MedicationRecords { get; set; }
        public ICollection<TherapiaRecordOnRecord> TherapiaRecords { get; set; }

        public class MedicationRecordOnRecord
        {
            public Guid MedicationId { get; set; }
            public string Name { get; set; }
            public double Amount { get; set; }
            public string UnitName { get; set; }
        }

        public class TherapiaRecordOnRecord
        {
            public Guid TherapiaId { get; set; }
            public string Name { get; set; }
            public double Amount { get; set; }
        }
    }
}
