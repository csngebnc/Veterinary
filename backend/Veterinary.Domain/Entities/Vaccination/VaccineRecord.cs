using System;
using Veterinary.Domain.Entities.AnimalEntities;

namespace Veterinary.Domain.Entities.Vaccination
{
    public class VaccineRecord
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid VaccineId { get; set; }
        public Vaccine Vaccine { get; set; }

        public Guid AnimalId { get; set; }
        public Animal Animal { get; set; }
    }
}
