using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class AnimalSpeciesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsInactive { get; set; }
    }

}
