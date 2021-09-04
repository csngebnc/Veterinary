using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Commands
{
    public class UpdateAnimalCommand : IRequest
    {
        public Guid AnimalId { get; set; }
        public UpdateAnimalCommandData Data { get; set; }
        public class UpdateAnimalCommandData
        {
            public string Name { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Sex { get; set; }
            public Guid SpeciesId { get; set; }
            public string SubSpecies { get; set; }
            public double Weight { get; set; }
        }
    }

    public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, Unit>
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IIdentityService identityService;

        public UpdateAnimalCommandHandler(
            IAnimalRepository animalRepository,
            IIdentityService identityService)
        {
            this.animalRepository = animalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = await animalRepository.FindAsync(request.AnimalId);

            if(animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            animal.Name = request.Data.Name;
            animal.DateOfBirth = request.Data.DateOfBirth.Value;
            animal.Sex = request.Data.Sex;
            animal.SpeciesId = request.Data.SpeciesId;
            animal.SubSpecies = request.Data.SubSpecies;
            animal.Weight = request.Data.Weight;

            await animalRepository.UpdateAsync(animal);

            return Unit.Value;
        }
    }

    public class UpdateAnimalCommandValidator : AbstractValidator<UpdateAnimalCommand>
    {
        public UpdateAnimalCommandValidator(VeterinaryDbContext context)
        {
            RuleFor(x => x.AnimalId).NotNull()
                .WithMessage("Az állat azonosítója nem lehet üres.");
            RuleFor(x => x.Data).SetValidator(new UpdateAnimalCommandDataValidator(context));
        }

        public class UpdateAnimalCommandDataValidator : AbstractValidator<UpdateAnimalCommand.UpdateAnimalCommandData>
        {
            public UpdateAnimalCommandDataValidator(VeterinaryDbContext context)
            {
                RuleFor(x => x.Name).NotEmpty()
                    .WithMessage("Az állat nevének megadása kötelező.");
                RuleFor(x => x.DateOfBirth).NotNull().LessThanOrEqualTo(DateTime.Today.AddDays(1))
                    .WithMessage("Az állat születési kötelező, illetve nem lehet jövőbeli dátum.");
                RuleFor(x => x.Sex).NotEmpty().Must(x => x.Equals("hím") || x.Equals("nőstény"))
                    .WithMessage("Az állat neme kötelező. (hím / nőstény)");
                RuleFor(x => x.SpeciesId).NotEmpty()
                    .MustAsync(async (speciesId, cancellationToken) => await context.AnimalSpecies.AnyAsync(u => u.Id == speciesId))
                    .WithMessage("Nem létezik ilyen állatfaj.");
                RuleFor(x => x.Weight).GreaterThanOrEqualTo(0)
                    .WithMessage("Az állat súlya nem lehet negatív érték.");
            }
        }
    }
}
