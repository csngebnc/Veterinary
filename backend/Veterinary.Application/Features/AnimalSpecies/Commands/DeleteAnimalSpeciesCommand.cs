using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Application.Features.AnimalSpeciesFeatures.Commands
{
    public class DeleteAnimalSpeciesCommand : IRequest
    {
        public Guid SpeciesId { get; set; }
    }

    public class DeleteAnimalSpeciesCommandHandler : IRequestHandler<DeleteAnimalSpeciesCommand, Unit>
    {
        private readonly IAnimalSpeciesRepository animalSpeciesRepository;

        public DeleteAnimalSpeciesCommandHandler(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            this.animalSpeciesRepository = animalSpeciesRepository;
        }

        public async Task<Unit> Handle(DeleteAnimalSpeciesCommand request, CancellationToken cancellationToken)
        {
            if(!(await animalSpeciesRepository.CanBeDeleted(request.SpeciesId)))
            {
                throw new MethodNotAllowedException("Az állatfaj nem törölhető, mert már regisztráltak vele állatot.");
            }

            await animalSpeciesRepository.DeleteAsync(request.SpeciesId);

            return Unit.Value;
        }
    }
}
