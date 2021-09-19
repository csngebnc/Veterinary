using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands
{
    public class UpdateTreatmentCommand : IRequest
    {
        public Guid TreatmentId { get; set; }
        public UpdateTreatmentCommandData Data { get; set; }
    }

    public class UpdateTreatmentCommandData
    {
        public string Name { get; set; }
        public int Duration { get; set; }
    }

    public class UpdateTreatmentCommandHandler : IRequestHandler<UpdateTreatmentCommand, Unit>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IIdentityService identityService;

        public UpdateTreatmentCommandHandler(ITreatmentRepository treatmentRepository, IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = await treatmentRepository.FindAsync(request.TreatmentId);

            if (treatment.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync(RoleEnum.ManagerDoctor.Value()))
            {
                throw new ForbiddenException();
            }

            treatment.Name = request.Data.Name;
            treatment.Duration = request.Data.Duration;

            await treatmentRepository.UpdateAsync(treatment);

            return Unit.Value;
        }
    }

    public class UpdateTreatmentCommandValidator : AbstractValidator<UpdateTreatmentCommand>
    {
        public UpdateTreatmentCommandValidator()
        {
            RuleFor(x => x.TreatmentId).NotNull()
                .WithMessage("A kezelés azonosítójának megadása kötelező.");
            RuleFor(x => x.Data).NotNull().SetValidator(new UpdateTreatmentCommandDataValidator());
        }
    }

    public class UpdateTreatmentCommandDataValidator : AbstractValidator<UpdateTreatmentCommandData>
    {
        public UpdateTreatmentCommandDataValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("A kezelés nevének megadása kötelező.");
            RuleFor(x => x.Duration).GreaterThan(0)
                .WithMessage("A kezelés hosszának legalább 1 percnek kell lennie.");
        }
    }
}
