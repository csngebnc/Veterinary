using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Application.Features.AnimalSpeciesFeatures.Commands
{
    public class CreateAnimalSpeciesCommand : IRequest<AnimalSpeciesDto>
    {
        public string Name { get; set; }
    }

    public class CreateAnimalSpeciesCommandHandler : IRequestHandler<CreateAnimalSpeciesCommand, AnimalSpeciesDto>
    {
        private readonly IAnimalSpeciesRepository animalSpeciesRepository;
        private readonly IIdentityService identityService;

        public CreateAnimalSpeciesCommandHandler(IAnimalSpeciesRepository animalSpeciesRepository, IIdentityService identityService)
        {
            this.animalSpeciesRepository = animalSpeciesRepository;
            this.identityService = identityService;
        }

        public async Task<AnimalSpeciesDto> Handle(CreateAnimalSpeciesCommand request, CancellationToken cancellationToken)
        {
            if(!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var species = await animalSpeciesRepository.InsertAsync(new AnimalSpecies
            {
                Name = request.Name,
            });

            return new AnimalSpeciesDto { Id = species.Id, Name = species.Name };
        }
    }

    public class CreateAnimalSpeciesCommandValidator : AbstractValidator<CreateAnimalSpeciesCommand>
    {
        public CreateAnimalSpeciesCommandValidator(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            RuleFor(x => x.Name).NotNull().NotEmpty()
                .WithMessage("Az állatfaj neve nem lehet üres.")
                .MustAsync(async (speciesName, cancellationToken) => !(await animalSpeciesRepository.AnyByNameAsync(speciesName)))
                .WithMessage("A megadott névvel már létezik állatfaj.");
        }
    }
}
