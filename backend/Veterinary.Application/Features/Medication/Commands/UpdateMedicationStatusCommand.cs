using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        public UpdateMedicationStatusCommandHandler(IMedicationRepository medicationRepository)
        {
            this.medicationRepository = medicationRepository;
        }

        public async Task<Unit> Handle(UpdateMedicationStatusCommand request, CancellationToken cancellationToken)
        {
            var medication = await medicationRepository.FindAsync(request.Id);
            medication.IsInactive = !medication.IsInactive;

            await medicationRepository.UpdateAsync(medication);
            return Unit.Value;
        }
    }
}
