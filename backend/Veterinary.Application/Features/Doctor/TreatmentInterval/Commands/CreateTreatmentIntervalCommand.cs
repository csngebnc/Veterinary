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
    public class CreateTreatmentIntervalCommand : IRequest<TreatmentIntervalDetailsDto>
    {
        public Guid DoctorId { get; set; }
        public CreateTreatmentIntervalCommandData Data { get; set; }
    }

    public class CreateTreatmentIntervalCommandData
    {
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int DayOfWeek { get; set; }

        public Guid TreatmentId { get; set; }
    }

    public class CreateTreatmentIntervalCommandHandler : IRequestHandler<CreateTreatmentIntervalCommand, TreatmentIntervalDetailsDto>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IIdentityService identityService;

        public CreateTreatmentIntervalCommandHandler(
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.identityService = identityService;
        }

        public async Task<TreatmentIntervalDetailsDto> Handle(CreateTreatmentIntervalCommand request, CancellationToken cancellationToken)
        {
            if (request.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var treatment = await treatmentRepository.FindAsync(request.Data.TreatmentId);
            if (treatment.DoctorId != request.DoctorId)
            {
                throw new ForbiddenException();
            }

            var treatmentInterval = new TreatmentInterval
            {
                TreatmentId = request.Data.TreatmentId,
                IsInactive = false,
                StartHour = request.Data.StartHour,
                StartMin = request.Data.StartMin,
                EndHour = request.Data.EndHour,
                EndMin = request.Data.EndMin,
                DayOfWeek = request.Data.DayOfWeek
            };

            await treatmentIntervalRepository.InsertAsync(treatmentInterval);

            return new TreatmentIntervalDetailsDto
            {
                Id = treatmentInterval.Id,
                TreatmentId = treatmentInterval.TreatmentId,
                IsInactive = treatmentInterval.IsInactive,
                StartHour = treatmentInterval.StartHour,
                StartMin = treatmentInterval.StartMin,
                EndHour = treatmentInterval.EndHour,
                EndMin = treatmentInterval.EndMin,
                DayOfWeek = treatmentInterval.DayOfWeek        
            };
        }
    }

    public class CreateTreatmentIntervalCommandValidator : AbstractValidator<CreateTreatmentIntervalCommand>
    {
        public CreateTreatmentIntervalCommandValidator()
        {
            RuleFor(x => x.DoctorId).NotNull()
                .WithMessage("Az állatorvos azonosítójának megadása kötelező.");
            RuleFor(x => x.Data).NotNull()
                .WithMessage("A mezők kitöltése kötelező.")
                .SetValidator(new CreateTreatmentIntervalCommandDataValidator());
        }

        public class CreateTreatmentIntervalCommandDataValidator : AbstractValidator<CreateTreatmentIntervalCommandData>
        {
            public CreateTreatmentIntervalCommandDataValidator()
            {
                RuleFor(x => x.TreatmentId).NotNull()
                    .WithMessage("A kezelés azonosítója nem lehet üres.");
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
