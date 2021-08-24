using System;
using System.Collections.Generic;

namespace Veterinary.Domain.Entities.AnimalEntities
{
    public class AnimalSpecies
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsInactive { get; set; }

        public ICollection<Animal> Animals { get; set; }
    }
}
