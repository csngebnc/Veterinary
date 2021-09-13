using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class TherapiaDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsInactive { get; set; }
    }
}
