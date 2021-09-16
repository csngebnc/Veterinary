using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class VaccineRecordDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid VaccineId { get; set; }
        public string VaccineName { get; set; }

        public Guid AnimalId { get; set; }
        public string AnimalName { get; set; }

    }
}
