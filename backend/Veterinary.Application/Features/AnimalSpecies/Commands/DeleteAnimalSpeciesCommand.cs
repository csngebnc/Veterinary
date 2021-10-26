using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
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
        private readonly IIdentityService identityService;

        public DeleteAnimalSpeciesCommandHandler(IAnimalSpeciesRepository animalSpeciesRepository, IIdentityService identityService)
        {
            this.animalSpeciesRepository = animalSpeciesRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteAnimalSpeciesCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            if (!(await animalSpeciesRepository.CanBeDeleted(request.SpeciesId)))
            {
                throw new MethodNotAllowedException("Az állatfaj nem törölhető, mert már regisztráltak vele állatot.");
            }

            await animalSpeciesRepository.DeleteAsync(request.SpeciesId);

            return Unit.Value;
        }
    }
}
