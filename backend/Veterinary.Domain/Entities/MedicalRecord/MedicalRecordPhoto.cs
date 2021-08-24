using System;

namespace Veterinary.Domain.Entities.MedicalRecordEntities
{
    public class MedicalRecordPhoto
    {
        public Guid Id { get; set; }
        public string PhotoUrl { get; set; }

        public Guid MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }
}