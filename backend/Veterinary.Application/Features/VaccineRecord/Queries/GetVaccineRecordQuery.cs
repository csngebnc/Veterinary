using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineRecordFeatures.Queries
{
    public class GetVaccineRecordQuery : IRequest<VaccineRecordDto>
    {
        public Guid VaccineRecordId { get; set; }
    }

    public class GetVaccineRecordQueryHandler : IRequestHandler<GetVaccineRecordQuery, VaccineRecordDto>
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;

        public GetVaccineRecordQueryHandler(IVaccineRecordRepository vaccineRecordRepository)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
        }

        public async Task<VaccineRecordDto> Handle(GetVaccineRecordQuery request, CancellationToken cancellationToken)
        {
            var record = await vaccineRecordRepository.GetVaccineRecordWithDetailsAsync(request.VaccineRecordId);

            return new VaccineRecordDto
            {
                Id = record.Id,
                Date = record.Date,
                AnimalId = record.AnimalId,
                AnimalName = record.Animal.Name,
                VaccineId = record.VaccineId,
                VaccineName = record.Vaccine.Name
            };
        }
    }
}
