using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Queries
{
    public class GetTreatmentIntervalDaysQuery : IRequest<List<int>>
    {
        public Guid TreatmentId { get; set; }
    }

    public class GetTreatmentIntervalDaysQueryHandler : IRequestHandler<GetTreatmentIntervalDaysQuery, List<int>>
    {
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;

        public GetTreatmentIntervalDaysQueryHandler(ITreatmentIntervalRepository treatmentIntervalRepository)
        {
            this.treatmentIntervalRepository = treatmentIntervalRepository;
        }

        public async Task<List<int>> Handle(GetTreatmentIntervalDaysQuery request, CancellationToken cancellationToken)
        {
            return await treatmentIntervalRepository.GetTreatmentIntervalsByTreatmentIdAsQueryable(request.TreatmentId)
                .Select(interval => interval.DayOfWeek)
                .Distinct()
                .ToListAsync();
        }
    }
}
