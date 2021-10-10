using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.TherapiaFeatures.Queries
{
    public class SearchTherapiaQuery : IRequest<List<TherapiaForSelectDto>>
    {
        public string SearchParam { get; set; }
    }

    public class TherapiaForSelectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class SearchTherapiaQueryHandler : IRequestHandler<SearchTherapiaQuery, List<TherapiaForSelectDto>>
    {
        private readonly ITherapiaRepository therapiaRepository;

        public SearchTherapiaQueryHandler(ITherapiaRepository therapiaRepository)
        {
            this.therapiaRepository = therapiaRepository;
        }

        public async Task<List<TherapiaForSelectDto>> Handle(SearchTherapiaQuery request, CancellationToken cancellationToken)
        {
            var therapias = await therapiaRepository.GetAllAsQueryable().Where(therapia => therapia.Name.Contains(request.SearchParam)).ToListAsync();
            return therapias.Select(therapia => new TherapiaForSelectDto
            {
                Id = therapia.Id,
                Name = therapia.Name,
            }).ToList();
        }
    }
}
