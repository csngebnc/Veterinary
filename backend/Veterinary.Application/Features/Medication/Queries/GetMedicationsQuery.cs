using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Application.Features.MedicationFeatures.Queries
{
    public class GetMedicationsQuery : IRequest<PagedList<MedicationDto>>
    {
        public PageData PageData { get; set; }
    }

    public class GetMedicationsQueryHandler : IRequestHandler<GetMedicationsQuery, PagedList<MedicationDto>>
    {
        private readonly IMedicationRepository medicationRepository;

        public GetMedicationsQueryHandler(IMedicationRepository medicationRepository)
        {
            this.medicationRepository = medicationRepository;
        }

        public async Task<PagedList<MedicationDto>> Handle(GetMedicationsQuery request, CancellationToken cancellationToken)
        {
            return await medicationRepository
                .GetAllAsQueryable()
                .Select(medication => new MedicationDto
                {
                    Id = medication.Id,
                    Name = medication.Name,
                    Unit = medication.Unit,
                    UnitName = medication.UnitName,
                    PricePerUnit = medication.PricePerUnit,
                    IsInactive = medication.IsInactive
                })
                .ToPagedListAsync(request.PageData);
        }
    }
}
