using System;

namespace Veterinary.Domain.Entities.MedicalRecordEntities
{
    public class MedicalRecordTextTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string HtmlContent { get; set; }
    }
}
