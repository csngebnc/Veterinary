using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.MedicationEntities;
using Microsoft.EntityFrameworkCore;

namespace Veterinary.Application.Features.MedicationFeatures.Queries
{
    public class SearchMedicationQuery : IRequest<List<MedicationForSelectDto>>
    {
        public string SearchParam { get; set; }
    }

    public class MedicationForSelectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
    }

    public class SearchMedicationQueryHandler : IRequestHandler<SearchMedicationQuery, List<MedicationForSelectDto>>
    {
        private readonly IMedicationRepository medicationRepository;

        public SearchMedicationQueryHandler(IMedicationRepository medicationRepository)
        {
            this.medicationRepository = medicationRepository;
        }

        public async Task<List<MedicationForSelectDto>> Handle(SearchMedicationQuery request, CancellationToken cancellationToken)
        {
            var medications = await medicationRepository.GetAllAsQueryable().Where(medication => medication.Name.Contains(request.SearchParam)).ToListAsync();
            return medications.Select(medication => new MedicationForSelectDto
            {
                Id = medication.Id,
                Name = medication.Name,
                UnitName = medication.UnitName
            }).ToList();
        }
    }
}
