using System;

namespace Veterinary.Application.Shared.Dtos
{
    public class TreatmentIntervalDetailsDto
    {
        public Guid Id { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsInactive { get; set; }

        public Guid TreatmentId { get; set; }
    }
}
