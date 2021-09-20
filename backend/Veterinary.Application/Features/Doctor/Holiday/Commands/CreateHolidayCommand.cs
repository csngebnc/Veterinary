using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.HolidayFeatures.Commands
{
    public class CreateHolidayCommand : IRequest<HolidayDto>
    {
        public CreateHolidayCommandData Data { get; set; }
    }

    public class CreateHolidayCommandData
    {
        public Guid DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateHolidayCommandHandler : IRequestHandler<CreateHolidayCommand, HolidayDto>
    {
        private readonly IHolidayRepository holidayRepository;
        private readonly IIdentityService identityService;

        public CreateHolidayCommandHandler(IHolidayRepository holidayRepository, IIdentityService identityService)
        {
            this.holidayRepository = holidayRepository;
            this.identityService = identityService;
        }

        public async Task<HolidayDto> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
        {
            if(await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var holiday = new Holiday
            {
                DoctorId = request.Data.DoctorId,
                StartDate = request.Data.StartDate,
                EndDate = request.Data.EndDate
            };

            await holidayRepository.InsertAsync(holiday);

            return new HolidayDto
            {
                Id = holiday.Id,
                DoctorId = holiday.DoctorId,
                StartDate = holiday.StartDate,
                EndDate = holiday.EndDate
            };
        }
    }

    public class CreateHolidayCommanndValidator : AbstractValidator<CreateHolidayCommand>
    {
        public CreateHolidayCommanndValidator()
        {
            RuleFor(x => x.Data).NotNull().SetValidator(new CreateHolidayCommandDataValidator());
        }
    }

    public class CreateHolidayCommandDataValidator : AbstractValidator<CreateHolidayCommandData>
    {
        public CreateHolidayCommandDataValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty()
                .WithMessage("Az orvos azonosítójának megadása kötelező.");
            RuleFor(x => x.StartDate).NotEmpty()
                .WithMessage("A szabadság kezdődátumának megadása kötelező.")
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("A szabadság eleje nem lehet később, mint a vége.");
            RuleFor(x => x.EndDate).GreaterThan(DateTime.Today)
                .WithMessage("A szabadság utolsó napjának dátumát kötelező kitölteni.");
        }
    }
}
