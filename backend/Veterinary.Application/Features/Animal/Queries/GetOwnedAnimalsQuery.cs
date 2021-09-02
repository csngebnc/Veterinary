using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Queries
{

    public class GetActiveOwnedAnimalsQuery : IRequest<PagedList<OwnedAnimalDto>>
    {
        public Guid OwnerId { get; set; }
        public PageData PageData { get; set; }
    }

    public class GetActiveOwnedAnimalsQueryHandler : IRequestHandler<GetActiveOwnedAnimalsQuery, PagedList<OwnedAnimalDto>>
    {
        private readonly IAnimalRepository repository;
        private readonly IIdentityService identityService;

        public GetActiveOwnedAnimalsQueryHandler(IAnimalRepository repository, IIdentityService identityService)
        {
            this.repository = repository;
            this.identityService = identityService;
        }
        public async Task<PagedList<OwnedAnimalDto>> Handle(GetActiveOwnedAnimalsQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetAllAsQueryable()
                .Include(animal => animal.Species)
                .Where(animal => animal.OwnerId == request.OwnerId)
                .Select(animal =>
                    new OwnedAnimalDto
                    {
                        Id = animal.Id,
                        Name = animal.Name,
                        DateOfBirth = animal.DateOfBirth,
                        Age = animal.DateOfBirth.CalculateAge(),
                        Sex = animal.Sex,
                        SpeciesName = animal.Species.Name,
                        SubSpeciesName = animal.SubSpecies,
                        PhotoUrl = animal.PhotoUrl
                    })
                .ToPagedListAsync(request.PageData);
        }
    }
}
