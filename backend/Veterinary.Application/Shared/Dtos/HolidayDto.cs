using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class HolidayDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}