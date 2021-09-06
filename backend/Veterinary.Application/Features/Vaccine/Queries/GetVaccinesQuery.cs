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
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Queries
{

    public class GetVaccinesQuery : IRequest<List<VaccineDto>>
    {
    }

    public class GetVaccinesQueryHandler : IRequestHandler<GetVaccinesQuery, List<VaccineDto>>
    {
        private readonly IVaccineRepository repository;

        public GetVaccinesQueryHandler(IVaccineRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<VaccineDto>> Handle(GetVaccinesQuery request, CancellationToken cancellationToken)
        {
            return await repository
                .GetAllAsQueryable()
                .Select(species => new VaccineDto
                {
                    Id = species.Id,
                    Name = species.Name,
                    IsInactive = species.IsInactive
                })
                .ToListAsync();
        }
    }
}
