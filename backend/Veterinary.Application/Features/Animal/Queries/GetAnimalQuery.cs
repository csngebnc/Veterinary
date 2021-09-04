using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Queries
{
    public class GetAnimalQuery : IRequest<AnimalDto>
    {
        public Guid AnimalId { get; set; }
    }

    public class GetAnimalQueryHandler : IRequestHandler<GetAnimalQuery, AnimalDto>
    {
        private readonly IAnimalRepository repository;
        private readonly IIdentityService identityService;

        public GetAnimalQueryHandler(IAnimalRepository repository, IIdentityService identityService)
        {
            this.repository = repository;
            this.identityService = identityService;
        }
        public async Task<AnimalDto> Handle(GetAnimalQuery request, CancellationToken cancellationToken)
        {
            var animal = await repository.FindAsync(request.AnimalId);

            if(animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            return new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                DateOfBirth = animal.DateOfBirth,
                Age = animal.DateOfBirth.CalculateAge(),
                Sex = animal.Sex,
                Weight = animal.Weight,
                SpeciesId = animal.SpeciesId,
                SubSpeciesName = animal.SubSpecies,
                PhotoUrl = animal.PhotoUrl,
                OwnerId = animal.OwnerId
            };
        }
    }
}
