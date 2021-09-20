using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.Doctor.HolidayFeatures.Commands
{
    public class UpdateHolidayCommand : IRequest
    {
        public UpdateHolidayCommandData Data { get; set; }
    }

    public class UpdateHolidayCommandData
    {
        public Guid HolidayId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateHolidayCommandHandler : IRequestHandler<UpdateHolidayCommand, Unit>
    {
        private readonly IHolidayRepository holidayRepository;
        private readonly IIdentityService identityService;

        public UpdateHolidayCommandHandler(IHolidayRepository holidayRepository, IIdentityService identityService)
        {
            this.holidayRepository = holidayRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
        {
            var holiday = await holidayRepository.FindAsync(request.Data.HolidayId);

            if (holiday.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync(RoleEnum.ManagerDoctor.Value()))
            {
                throw new ForbiddenException();
            }

            holiday.StartDate = request.Data.StartDate;
            holiday.EndDate = request.Data.EndDate;

            await holidayRepository.UpdateAsync(holiday);

            return Unit.Value;
        }
    }

    public class UpdateHolidayCommandValidator : AbstractValidator<UpdateHolidayCommand>
    {
        public UpdateHolidayCommandValidator()
        {
            RuleFor(x => x.Data).NotNull().SetValidator(new UpdateHolidayCommandDataValidator());
        }
    }

    public class UpdateHolidayCommandDataValidator : AbstractValidator<UpdateHolidayCommandData>
    {
        public UpdateHolidayCommandDataValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty()
                .WithMessage("A szabadság kezdődátumának megadása kötelező.")
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("A szabadság eleje nem lehet később, mint a vége.");
            RuleFor(x => x.EndDate).GreaterThan(DateTime.Today)
                .WithMessage("A szabadság utolsó napjának dátumát kötelező kitölteni.");
        }
    }
}
