using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.TherapiaFeatures.Queries
{
    public class GetTherapiasQuery : IRequest<PagedList<TherapiaDto>>
    {
        public PageData PageData { get; set; }
    }

    public class GetTherapiasQueryHandler : IRequestHandler<GetTherapiasQuery, PagedList<TherapiaDto>>
    {
        private readonly ITherapiaRepository therapiaRepository;

        public GetTherapiasQueryHandler(ITherapiaRepository therapiaRepository)
        {
            this.therapiaRepository = therapiaRepository;
        }

        public async Task<PagedList<TherapiaDto>> Handle(GetTherapiasQuery request, CancellationToken cancellationToken)
        {
            return await therapiaRepository
                .GetAllAsQueryable()
                .Select(therapia => new TherapiaDto
                {
                    Id = therapia.Id,
                    Name = therapia.Name,
                    Price = therapia.Price,
                    IsInactive = therapia.IsInactive
                })
                .ToPagedListAsync(request.PageData);
        }
    }
}
