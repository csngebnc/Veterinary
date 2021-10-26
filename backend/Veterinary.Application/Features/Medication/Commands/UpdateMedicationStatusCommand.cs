using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Application.Features.MedicationFeatures.Commands
{
    public class UpdateMedicationStatusCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class UpdateMedicationStatusCommandHandler : IRequestHandler<UpdateMedicationStatusCommand, Unit>
    {
        private readonly IMedicationRepository medicationRepository;
        private readonly IIdentityService identityService;

        public UpdateMedicationStatusCommandHandler(IMedicationRepository medicationRepository, IIdentityService identityService)
        {
            this.medicationRepository = medicationRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateMedicationStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var medication = await medicationRepository.FindAsync(request.Id);
            medication.IsInactive = !medication.IsInactive;

            await medicationRepository.UpdateAsync(medication);
            return Unit.Value;
        }
    }
}
