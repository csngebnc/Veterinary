using System;

namespace Veterinary.Domain.Entities.Doctor.HolidayEntities
{
    public class Holiday
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid DoctorId { get; set; }
        public VeterinaryUser Doctor { get; set; }
    }
}
