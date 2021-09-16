using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Queries
{
    public class GetVaccineQuery : IRequest<VaccineDto>
    {
        public Guid VaccineId { get; set; }
    }

    public class GetVaccineQueryHandler : IRequestHandler<GetVaccineQuery, VaccineDto>
    {
        private readonly IVaccineRepository vaccineRepository;

        public GetVaccineQueryHandler(IVaccineRepository vaccineRepository)
        {
            this.vaccineRepository = vaccineRepository;
        }

        public async Task<VaccineDto> Handle(GetVaccineQuery request, CancellationToken cancellationToken)
        {
            var vaccine = await vaccineRepository.FindAsync(request.VaccineId);

            return new VaccineDto
            {
                Id = vaccine.Id,
                Name = vaccine.Name,
                IsInactive = vaccine.IsInactive
            };
        }
    }
}
