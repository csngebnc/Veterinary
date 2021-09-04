using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Commands
{
    public class DeleteAnimalCommand : IRequest
    {
        public Guid AnimalId { get; set; }
    }

    public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand, Unit>
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IIdentityService identityService;

        public DeleteAnimalCommandHandler(IAnimalRepository animalRepository, IIdentityService identityService)
        {
            this.animalRepository = animalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = await animalRepository.FindAsync(request.AnimalId);

            if (animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new Exception("Nincs jogosultság.");
            }

            animal.IsDeleted = true;
            await animalRepository.UpdateAsync(animal);

            return Unit.Value;
        }
    }

    public class DeleteAnimalCommandValidator : AbstractValidator<DeleteAnimalCommand>
    {
        public DeleteAnimalCommandValidator()
        {
            RuleFor(x => x.AnimalId).NotNull()
                .WithMessage("Az állat azonosítója nem lehet üres.");
        }
    }
}
