using System;
using Veterinary.Shared.Enums;

namespace Veterinary.Application.Shared.Dtos
{
    public class AppointmentForDoctorDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public Guid TreatmentId { get; set; }
        public string TreatmentName { get; set; }

        public Guid? AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string AnimalSpecies { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Reasons { get; set; }
        public AppointmentStatusEnum Status { get; set; }
    }
}
