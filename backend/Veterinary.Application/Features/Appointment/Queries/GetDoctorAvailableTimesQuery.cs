using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.AppointmentFeatures.Queries
{
    public class GetDoctorAvailableTimesQuery : IRequest<List<AvailableTime>>
    {
        public Guid DoctorId { get; set; }
        public Guid TreatmentId { get; set; }
        public DateTime Date { get; set; }
    }

    public class AvailableTime
    {
        public int Id { get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime {  get; set; }
    }

    public class GetDoctorAvailableTimesQueryHandler : IRequestHandler<GetDoctorAvailableTimesQuery, List<AvailableTime>>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IHolidayRepository holidayRepository;
        private readonly IAppointmentRepository appointmentRepository;

        public GetDoctorAvailableTimesQueryHandler(
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IHolidayRepository holidayRepository,
            IAppointmentRepository appointmentRepository)
        {
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.holidayRepository = holidayRepository;
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<List<AvailableTime>> Handle(GetDoctorAvailableTimesQuery request, CancellationToken cancellationToken)
        {
            var dateInRequest = request.Date.ToLocalTime();

            var holidays = await holidayRepository.GetDoctorHolidaysByInterval(request.DoctorId, dateInRequest, 0);
            if(holidays.Count > 0)
            {
                return new List<AvailableTime>();
            }

            var treatment = await treatmentRepository.FindAsync(request.TreatmentId);
            var treatmentTimes = await treatmentIntervalRepository.GetTreatmentIntervalsByTreatmentIdAsQueryable(treatment.Id)
                                    .Where(interval => interval.DayOfWeek == (int)dateInRequest.DayOfWeek)
                                    .ToListAsync();

            var reservedTimes = await appointmentRepository.GetAppointmentsByDoctorAndDateAsync(request.DoctorId, dateInRequest.ToLocalTime());

            var availableTimes = new List<AvailableTime>();

            foreach (var time in treatmentTimes)
            {
                var actualMinute = time.GetStartInMinutes();
                while (actualMinute < time.GetEndInMinutes())
                {
                    var startDate = new DateTime(dateInRequest.Year, dateInRequest.Month, dateInRequest.Day, actualMinute / 60, actualMinute % 60, 0);
                    actualMinute += treatment.Duration;
                    var endDate = new DateTime(dateInRequest.Year, dateInRequest.Month, dateInRequest.Day, actualMinute / 60, actualMinute % 60, 0);

                    var intersect = reservedTimes.Where(reservedTime => reservedTime.StartDate < startDate && reservedTime.EndDate > startDate ||
                                                reservedTime.StartDate >= startDate && reservedTime.EndDate < endDate ||
                                                reservedTime.StartDate < endDate && reservedTime.EndDate > endDate ||
                                                reservedTime.StartDate <= startDate && reservedTime.EndDate >= endDate
                                                ).ToList();

                    if(intersect.Count == 0 && startDate > DateTime.Now.AddMinutes(10))
                    {
                        availableTimes.Add(new AvailableTime
                        {
                            Id = availableTimes.Count,
                            StartTime = startDate,
                            EndTime = endDate
                        });
                    }
                }
            }
                      
            return availableTimes;
        }
    }
}
