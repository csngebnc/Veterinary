using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class MedicationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public double Unit { get; set; }
        public double PricePerUnit { get; set; }
        public bool IsInactive { get; set; }
    }
}
