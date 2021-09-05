using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Application.Features.AnimalSpeciesFeatures.Commands
{
    public class UpdateAnimalSpeciesStatusCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class UpdateAnimalSpeciesStatusCommandHandler : IRequestHandler<UpdateAnimalSpeciesStatusCommand, Unit>
    {
        private readonly IAnimalSpeciesRepository animalSpeciesRepository;

        public UpdateAnimalSpeciesStatusCommandHandler(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            this.animalSpeciesRepository = animalSpeciesRepository;
        }

        public async Task<Unit> Handle(UpdateAnimalSpeciesStatusCommand request, CancellationToken cancellationToken)
        {
            var species = await animalSpeciesRepository.FindAsync(request.Id);
            species.IsInactive = !species.IsInactive;

            await animalSpeciesRepository.UpdateAsync(species);
            return Unit.Value;
        }
    }
}
