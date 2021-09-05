using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Application.Features.AnimalSpeciesFeatures.Commands
{
    public class UpdateAnimalSpeciesCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateAnimalSpeciesCommandHandler : IRequestHandler<UpdateAnimalSpeciesCommand, Unit>
    {
        private readonly IAnimalSpeciesRepository animalSpeciesRepository;

        public UpdateAnimalSpeciesCommandHandler(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            this.animalSpeciesRepository = animalSpeciesRepository;
        }

        public async Task<Unit> Handle(UpdateAnimalSpeciesCommand request, CancellationToken cancellationToken)
        {
            var species = await animalSpeciesRepository.FindAsync(request.Id);
            species.Name = request.Name;

            await animalSpeciesRepository.UpdateAsync(species);
            return Unit.Value;
        }
    }

    public class UpdateAnimalSpeciesCommandValidator : AbstractValidator<UpdateAnimalSpeciesCommand>
    {
        public UpdateAnimalSpeciesCommandValidator(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            RuleFor(x => x.Name).NotNull()
                .WithMessage("Az állatfaj neve nem lehet üres.")
                .MustAsync(async (speciesName, cancellationToken) => !(await animalSpeciesRepository.AnyByNameAsync(speciesName)))
                .WithMessage("A megadott névvel már létezik állatfaj.");
        }
    }
}
