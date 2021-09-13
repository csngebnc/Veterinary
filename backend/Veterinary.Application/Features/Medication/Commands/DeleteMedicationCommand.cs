using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        public DeleteMedicationCommandHandler(IMedicationRepository medicationRepository)
        {
            this.medicationRepository = medicationRepository;
        }

        public async Task<Unit> Handle(DeleteMedicationCommand request, CancellationToken cancellationToken)
        {
            if(!(await medicationRepository.CanBeDeleted(request.MedicationId)))
            {
                throw new MethodNotAllowedException("A gyógyszer nem törölhető, mert már rögzítették legalább egy kórlapon.");
            }

            await medicationRepository.DeleteAsync(request.MedicationId);

            return Unit.Value;
        }
    }
}
