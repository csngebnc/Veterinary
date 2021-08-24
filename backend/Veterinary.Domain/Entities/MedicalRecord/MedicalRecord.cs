using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Domain.Entities.MedicalRecordEntities
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid DoctorId { get; set; }
        public VeterinaryUser Doctor { get; set; }

        public string OwnerEmail { get; set; }
        public Guid? OwnerId { get; set; }
        public VeterinaryUser Owner { get; set; }

        public Guid? AnimalId { get; set; }
        public Animal Animal { get; set; }

        public string Anamnesis { get; set; }
        public string Symptoma { get; set; }
        public string Details { get; set; }

        public ICollection<MedicalRecordPhoto> Photos { get; set; }
        public ICollection<TherapiaRecord> TherapiaRecords { get; set; }
    }
}
