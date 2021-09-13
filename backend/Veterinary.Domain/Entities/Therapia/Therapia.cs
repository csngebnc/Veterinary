using System;
using System.Collections.Generic;

namespace Veterinary.Domain.Entities.TherapiaEntities
{
    public class Therapia
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsInactive { get; set; }

        public ICollection<TherapiaRecord> TherapiaRecords { get; set; }
    }
}
