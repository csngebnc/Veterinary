using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Dal.Data;
using Veterinary.Domain.Constants;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AnimalRepository;
using static Veterinary.Application.Features.AnimalFeatures.Commands.CreateAnimalCommand;

namespace Veterinary.Application.Features.AnimalFeatures.Commands
{

    public class CreateAnimalCommand : IRequest
    {

        public Guid UserId { get; set; }
        public CreateAnimalCommandData Data { get; set; }

        public class CreateAnimalCommandData
        {
            public string Name { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Sex { get; set; }
            public Guid SpeciesId { get; set; }
            public IFormFile? Photo { get; set; }
        }
    }


    public class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, Unit>
    {
        private readonly IAnimalRepository animalRepository;
        public IPhotoService photoService { get; }

        public CreateAnimalCommandHandler(IAnimalRepository animalRepository, IPhotoService photoService)
        {
            this.animalRepository = animalRepository;
            this.photoService = photoService;
        }


        public async Task<Unit> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = new Animal
            {
                Name = request.Data.Name,
                DateOfBirth = request.Data.DateOfBirth.Value,
                Sex = request.Data.Sex,
                SpeciesId = request.Data.SpeciesId,
                OwnerId = request.UserId,
                PhotoUrl = request.Data.Photo == null ?
                        UrlConstants.AnimalPlaceholderPhotoUrl
                        : await photoService.UploadPhoto("Animals", request.UserId.ToString(), request.Data.Photo)
            };

            await animalRepository.InsertAsync(animal);

            return Unit.Value;
        }

        public class CreateAnimalCommandValidator : AbstractValidator<CreateAnimalCommand>
        {
            public CreateAnimalCommandValidator(VeterinaryDbContext context)
            {
                RuleFor(x => x.UserId)
                    .NotEmpty()
                    .MustAsync(async (userId, cancellationToken) => await context.Users.AnyAsync(u => u.Id == userId));

                RuleFor(x => x.Data)
                    .NotNull()
                    .SetValidator(new CreateAnimalCommandDataValidator(context));
            }

            public class CreateAnimalCommandDataValidator : AbstractValidator<CreateAnimalCommandData>
            {
                public CreateAnimalCommandDataValidator(VeterinaryDbContext context)
                {
                    RuleFor(x => x.Name).NotEmpty();
                    RuleFor(x => x.DateOfBirth).NotNull().LessThanOrEqualTo(DateTime.Today.AddDays(1));
                    RuleFor(x => x.Sex).NotEmpty();
                    RuleFor(x => x.SpeciesId)
                        .NotEmpty()
                        .MustAsync(async (speciesId, cancellationToken) => await context.AnimalSpecies.AnyAsync(u => u.Id == speciesId));
                }
            }

        }

    }
}
