using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Commands
{
    public class UpdateAnimalArchiveStatusCommand : IRequest
    {
        public Guid AnimalId { get; set; }
    }

    public class UpdateAnimalArchiveStatusCommandHandler : IRequestHandler<UpdateAnimalArchiveStatusCommand, Unit>
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IIdentityService identityService;

        public UpdateAnimalArchiveStatusCommandHandler(IAnimalRepository animalRepository, IIdentityService identityService)
        {
            this.animalRepository = animalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateAnimalArchiveStatusCommand request, CancellationToken cancellationToken)
        {
            var animal = await animalRepository.FindAsync(request.AnimalId);

            if(animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            animal.IsArchived = !animal.IsArchived;
            await animalRepository.UpdateAsync(animal);

            return Unit.Value;
        }
    }

    public class UpdateAnimalArchiveStatusCommandValidator : AbstractValidator<UpdateAnimalArchiveStatusCommand>
    {
        public UpdateAnimalArchiveStatusCommandValidator()
        {
            RuleFor(x => x.AnimalId).NotNull()
                .WithMessage("Az állat azonosítója nem lehet üres.");
        }
    }
}
