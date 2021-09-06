using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Commands
{
    public class DeleteVaccineCommand : IRequest
    {
        public Guid VaccineId { get; set; }
    }

    public class DeleteAnimalSpeciesCommandHandler : IRequestHandler<DeleteVaccineCommand, Unit>
    {
        private readonly IVaccineRepository vaccineRepository;

        public DeleteAnimalSpeciesCommandHandler(IVaccineRepository vaccineRepository)
        {
            this.vaccineRepository = vaccineRepository;
        }

        public async Task<Unit> Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
        {
            if(!(await vaccineRepository.CanBeDeleted(request.VaccineId)))
            {
                throw new MethodNotAllowedException("Az oltástípus nem törölhető, mert már legalább egy állatnál rögzítésre került.");
            }

            await vaccineRepository.DeleteAsync(request.VaccineId);

            return Unit.Value;
        }
    }
}
