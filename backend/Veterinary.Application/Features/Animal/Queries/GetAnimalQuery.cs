using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Extensions;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Queries
{
    public class GetAnimalQuery : IRequest<AnimalDto>
    {
        public string AnimalId { get; set; }
    }

    public class GetAnimalQueryHandler : IRequestHandler<GetAnimalQuery, AnimalDto>
    {
        private readonly IAnimalRepository repository;

        public GetAnimalQueryHandler(IAnimalRepository repository)
        {
            this.repository = repository;
        }
        public async Task<AnimalDto> Handle(GetAnimalQuery request, CancellationToken cancellationToken)
        {
            var animal = await repository.FindAsync(Guid.Parse(request.AnimalId));
            return new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                DateOfBirth = animal.DateOfBirth,
                Age = animal.DateOfBirth.CalculateAge(),
                Sex = animal.Sex,
                SpeciesId = animal.SpeciesId,
                SubSpeciesName = animal.SubSpecies,
                PhotoUrl = animal.PhotoUrl
            };
        }
    }
}
