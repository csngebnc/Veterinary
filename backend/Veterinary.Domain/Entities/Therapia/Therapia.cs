using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.TherapiaEntities
{
    public class Therapia
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public double Unit { get; set; }
        public double PricePerUnit { get; set; }
        public bool IsInactive { get; set; }

        public ICollection<TherapiaRecord> TherapiaRecords { get; set; }
    }
}
