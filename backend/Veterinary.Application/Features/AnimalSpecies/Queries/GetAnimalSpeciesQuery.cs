using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Application.Features.AnimalSpeciesFeatures.Queries
{

    public class GetAnimalSpeciesQuery : IRequest<List<AnimalSpeciesDto>>
    {
    }

    public class GetAnimalSpeciesQueryHandler : IRequestHandler<GetAnimalSpeciesQuery, List<AnimalSpeciesDto>>
    {
        private readonly IAnimalSpeciesRepository repository;

        public GetAnimalSpeciesQueryHandler(IAnimalSpeciesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<AnimalSpeciesDto>> Handle(GetAnimalSpeciesQuery request, CancellationToken cancellationToken)
        {
            return await repository
                .GetAllAsQueryable()
                .Select(species => new AnimalSpeciesDto
                {
                    Id = species.Id,
                    Name = species.Name,
                    IsInactive = species.IsInactive
                })
                .ToListAsync();
        }
    }
}
