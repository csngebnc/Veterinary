using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
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
        private readonly IIdentityService identityService;

        public UpdateAnimalSpeciesStatusCommandHandler(IAnimalSpeciesRepository animalSpeciesRepository, IIdentityService identityService)
        {
            this.animalSpeciesRepository = animalSpeciesRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateAnimalSpeciesStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var species = await animalSpeciesRepository.FindAsync(request.Id);
            species.IsInactive = !species.IsInactive;

            await animalSpeciesRepository.UpdateAsync(species);
            return Unit.Value;
        }
    }
}
