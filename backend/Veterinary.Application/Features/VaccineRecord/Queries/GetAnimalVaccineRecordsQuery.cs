using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineRecordFeatures.Queries
{
    public class GetAnimalVaccineRecordsQuery : IRequest<PagedList<VaccineRecordDto>>
    {
        public Guid AnimalId { get; set; }
        public PageData PageData { get; set; }
    }

    public class GetAnimalVaccineRecordsQueryHandler : IRequestHandler<GetAnimalVaccineRecordsQuery, PagedList<VaccineRecordDto>>
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;

        public GetAnimalVaccineRecordsQueryHandler(IVaccineRecordRepository vaccineRecordRepository)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
        }

        public async Task<PagedList<VaccineRecordDto>> Handle(GetAnimalVaccineRecordsQuery request, CancellationToken cancellationToken)
        {
            return await vaccineRecordRepository.GetVaccineRecordsByAnimalIdAsync(request.AnimalId)
                .Select(vr =>  new VaccineRecordDto
                {
                    Id = vr.Id,
                    Date = vr.Date,
                    VaccineId = vr.VaccineId,
                    VaccineName = vr.Vaccine.Name,
                    AnimalId = vr.AnimalId,
                    AnimalName = vr.Animal.Name
                })
                .ToPagedListAsync(request.PageData);
        }
    }
}
