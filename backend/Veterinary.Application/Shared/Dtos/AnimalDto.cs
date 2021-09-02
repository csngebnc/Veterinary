using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class AnimalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public Guid SpeciesId { get; set; }
        public string SubSpeciesName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
