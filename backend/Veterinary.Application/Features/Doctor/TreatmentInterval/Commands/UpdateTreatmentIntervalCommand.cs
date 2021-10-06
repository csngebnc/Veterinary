using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Commands
{
    public class UpdateTreatmentIntervalCommand : IRequest
    {
        public Guid DoctorId { get; set; }
        public UpdateTreatmentIntervalCommandData Data { get; set; }
    }

    public class UpdateTreatmentIntervalCommandData
    {
        public Guid Id { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int DayOfWeek { get; set; }
    }

    public class UpdateTreatmentIntervalCommandHandler : IRequestHandler<UpdateTreatmentIntervalCommand, Unit>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IIdentityService identityService;

        public UpdateTreatmentIntervalCommandHandler(
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateTreatmentIntervalCommand request, CancellationToken cancellationToken)
        {
            if (request.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var treatmentInterval = await treatmentIntervalRepository.FindAsync(request.Data.Id);

            var treatment = await treatmentRepository.FindAsync(treatmentInterval.TreatmentId);
            if (treatment.DoctorId != request.DoctorId)
            {
                throw new ForbiddenException();
            }

            treatmentInterval.StartHour = request.Data.StartHour;
            treatmentInterval.StartMin = request.Data.StartMin;
            treatmentInterval.EndHour = request.Data.EndHour;
            treatmentInterval.EndMin = request.Data.EndMin;
            treatmentInterval.DayOfWeek = request.Data.DayOfWeek;

            await treatmentIntervalRepository.UpdateAsync(treatmentInterval);

            return Unit.Value;
        }
    }

    public class UpdateTreatmentIntervalCommandValidator : AbstractValidator<UpdateTreatmentIntervalCommand>
    {
        public UpdateTreatmentIntervalCommandValidator()
        {
            RuleFor(x => x.DoctorId).NotNull()
                .WithMessage("Az állatorvos azonosítójának megadása kötelező.");
            RuleFor(x => x.Data).NotNull()
                .WithMessage("A mezők kitöltése kötelező.")
                .SetValidator(new UpdateTreatmentIntervalCommandDataValidator());
        }

        public class UpdateTreatmentIntervalCommandDataValidator : AbstractValidator<UpdateTreatmentIntervalCommandData>
        {
            public UpdateTreatmentIntervalCommandDataValidator()
            {
                RuleFor(x => x.Id).NotNull()
                    .WithMessage("A kezelési idősáv azonosítója nem lehet üres.");
                RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6)
                    .WithMessage("A hét napjának száma 0 (vasárnap) és 6 (szombat) között lévő érték kell legyen.");
                RuleFor(x => x.StartHour).InclusiveBetween(0, 23)
                    .WithMessage("Az időpont óra értéke 0 és 23 között kell, hogy legyen.");
                RuleFor(x => x.StartMin).InclusiveBetween(0, 59)
                    .WithMessage("Az óra perc értéke 0 és 59 között kell, hogy legyen.");
                RuleFor(x => x.EndHour).InclusiveBetween(0, 23)
                    .WithMessage("Az időpont óra értéke 0 és 23 között kell, hogy legyen.");
                RuleFor(x => x.EndMin).InclusiveBetween(0, 59)
                    .WithMessage("Az óra perc értéke 0 és 59 között kell, hogy legyen.");
            }
        }
    }
}
