using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Domain.Constants;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Commands
{
    public class DeleteAnimalPhotoCommand : IRequest<string>
    {
        public Guid AnimalId { get; set; }
    }

    public class DeleteAnimalPhotoCommandHandler : IRequestHandler<DeleteAnimalPhotoCommand, string>
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IPhotoService photoService;
        private readonly IIdentityService identityService;

        public DeleteAnimalPhotoCommandHandler(IAnimalRepository animalRepository, IPhotoService photoService, IIdentityService identityService)
        {
            this.animalRepository = animalRepository;
            this.photoService = photoService;
            this.identityService = identityService;
        }

        public async Task<string> Handle(DeleteAnimalPhotoCommand request, CancellationToken cancellationToken)
        {
            var animal = await animalRepository.FindAsync(request.AnimalId);

            if (animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new Exception("Nem gazdi.");
            }

            if (photoService.RemovePhoto(animal.PhotoUrl))
            {
                animal.PhotoUrl = UrlConstants.AnimalPlaceholderPhotoUrl;
                await animalRepository.UpdateAsync(animal);
            }

            return animal.PhotoUrl;
        }
    }

    public class DeleteAnimalPhotoCommandValidator : AbstractValidator<DeleteAnimalPhotoCommand>
    {
        public DeleteAnimalPhotoCommandValidator(IAnimalRepository animalRepository)
        {
            RuleFor(x => x.AnimalId).NotNull()
                .MustAsync(async (animalId, cancellationToken) => await animalRepository.AnyByIdAsync(animalId))
                .WithMessage("A megadott azonosítóval nem létezik állat.");
        }
    }
}
