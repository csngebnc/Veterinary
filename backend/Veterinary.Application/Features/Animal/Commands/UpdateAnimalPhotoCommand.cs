using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Shared.Constants;

namespace Veterinary.Application.Features.AnimalFeatures.Commands
{
    public class UpdateAnimalPhotoCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public UpdateAnimalPhotoCommandData Data { get; set; }

        public class UpdateAnimalPhotoCommandData
        {
            public Guid AnimalId { get; set; }
            public IFormFile Photo { get; set; }
        }
    }

    public class UpdateAnimalPhotoCommandHandler : IRequestHandler<UpdateAnimalPhotoCommand, string>
    {
        private readonly IPhotoService photoService;
        private readonly IIdentityService identityService;
        private readonly IAnimalRepository animalRepository;

        public UpdateAnimalPhotoCommandHandler(IPhotoService photoService, IIdentityService identityService, IAnimalRepository animalRepository)
        {
            this.photoService = photoService;
            this.identityService = identityService;
            this.animalRepository = animalRepository;
        }

        public async Task<string> Handle(UpdateAnimalPhotoCommand request, CancellationToken cancellationToken)
        {
            var animal = await animalRepository.FindAsync(request.Data.AnimalId);

            if(animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            if (photoService.RemovePhoto(animal.PhotoUrl))
            {
                var photoUrl = await photoService.UploadPhoto("Animals", request.UserId.ToString(), request.Data.Photo);
                if (photoUrl == null)
                {
                    photoUrl = UrlConstants.PlaceholderImage;
                }

                animal.PhotoUrl = photoUrl;
                await animalRepository.UpdateAsync(animal);
            }

            return animal.PhotoUrl;
        }
    }

    public class UpdateAnimalPhotoCommandValidator : AbstractValidator<UpdateAnimalPhotoCommand>
    {
        public UpdateAnimalPhotoCommandValidator(VeterinaryDbContext context, IAnimalRepository animalRepository)
        {
            RuleFor(x => x.UserId).NotNull()
                .MustAsync(async (userId, cancellationToken) => await context.Users.AnyAsync(u => u.Id == userId))
                .WithMessage("A megadott azonosítóval nem létezik felhasználó.");
            RuleFor(x => x.Data).NotNull().SetValidator(new UpdateAnimalPhotoCommandDataValidator(animalRepository));
        }

        public class UpdateAnimalPhotoCommandDataValidator : AbstractValidator<UpdateAnimalPhotoCommand.UpdateAnimalPhotoCommandData>
        {

            public UpdateAnimalPhotoCommandDataValidator(IAnimalRepository animalRepository)
            {
                RuleFor(x => x.AnimalId).NotNull()
                .MustAsync(async (animalId, cancellationToken) => await animalRepository.AnyByIdAsync(animalId))
                .WithMessage("A megadott azonosítóval nem létezik állat.");
                RuleFor(x => x.Photo).NotNull()
                    .WithMessage("Kép feltöltése kötelező.");

            }
        }
    }
}
