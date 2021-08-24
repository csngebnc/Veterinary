using System;
using System.Collections.Generic;

namespace Veterinary.Domain.Entities.Vaccination
{
    public class Vaccine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<VaccineRecord> VaccinationRecords { get; set; }
    }
}
