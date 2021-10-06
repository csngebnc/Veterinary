using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;

namespace Veterinary.Application.Features.Doctor.HolidayFeatures.Queries
{
    public class GetDoctorHolidaysByIntervalQuery : IRequest<List<HolidayDto>>
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }

    public class GetDoctorHolidaysByIntervalQueryHandler : IRequestHandler<GetDoctorHolidaysByIntervalQuery, List<HolidayDto>>
    {
        private readonly IHolidayRepository holidayRepository;

        public GetDoctorHolidaysByIntervalQueryHandler(IHolidayRepository holidayRepository)
        {
            this.holidayRepository = holidayRepository;
        }

        public async Task<List<HolidayDto>> Handle(GetDoctorHolidaysByIntervalQuery request, CancellationToken cancellationToken)
        {
            var holidays = await holidayRepository.GetDoctorHolidaysByInterval(request.DoctorId, request.Date, request.Duration);
            return holidays
                .Select(holiday => new HolidayDto
                {
                    Id = holiday.Id,
                    StartDate = holiday.StartDate,
                    EndDate = holiday.EndDate,
                    DoctorId = holiday.DoctorId
                }).ToList();
        }
    }

    public class GetDoctorHolidaysByIntervalQueryValidator : AbstractValidator<GetDoctorHolidaysByIntervalQuery>
    {
        public GetDoctorHolidaysByIntervalQueryValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty()
                .WithMessage("Az állatorvos kiválasztása kötelező.");
            RuleFor(x => x.Date).NotEmpty()
                .WithMessage("A kezdődátum megadása kötelező.");
        }
    }
}
