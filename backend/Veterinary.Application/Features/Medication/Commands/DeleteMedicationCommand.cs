using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Application.Features.MedicationFeatures.Commands
{
    public class DeleteMedicationCommand : IRequest
    {
        public Guid MedicationId { get; set; }
    }

    public class DeleteMedicationCommandHandler : IRequestHandler<DeleteMedicationCommand, Unit>
    {
        private readonly IMedicationRepository medicationRepository;
        private readonly IIdentityService identityService;

        public DeleteMedicationCommandHandler(IMedicationRepository medicationRepository, IIdentityService identityService)
        {
            this.medicationRepository = medicationRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteMedicationCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            if (!(await medicationRepository.CanBeDeleted(request.MedicationId)))
            {
                throw new MethodNotAllowedException("A gyógyszer nem törölhető, mert már rögzítették legalább egy kórlapon.");
            }

            await medicationRepository.DeleteAsync(request.MedicationId);

            return Unit.Value;
        }
    }
}
