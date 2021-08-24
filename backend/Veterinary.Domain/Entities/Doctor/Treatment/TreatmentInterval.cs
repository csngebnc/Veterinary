using System;

namespace Veterinary.Domain.Entities.Doctor.TreatmentEntities
{
    public class TreatmentInterval
    {
        public Guid Id { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsInactive { get; set; }

        public Guid TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public int GetStartInMinutes()
            => StartHour * 60 + StartMin;

        public int GetEndInMinutes()
            => EndHour * 60 + EndMin;
    }
}
