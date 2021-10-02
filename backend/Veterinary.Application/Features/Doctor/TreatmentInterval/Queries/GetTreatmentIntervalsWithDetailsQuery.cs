using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Queries
{
    public class GetTreatmentIntervalsWithDetailsQuery : IRequest<PagedList<TreatmentIntervalDetailsDto>>
    {
        public PageData PageData { get; set; }
        public Guid TreatmentId { get; set; }
    }

    public class GetTreatmentIntervalsWithDetailsQueryHandler : IRequestHandler<GetTreatmentIntervalsWithDetailsQuery, PagedList<TreatmentIntervalDetailsDto>>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IIdentityService identityService;

        public GetTreatmentIntervalsWithDetailsQueryHandler(
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.identityService = identityService;
        }

        public async Task<PagedList<TreatmentIntervalDetailsDto>> Handle(GetTreatmentIntervalsWithDetailsQuery request, CancellationToken cancellationToken)
        {
            var treatment = await treatmentRepository.FindAsync(request.TreatmentId);

            if (treatment.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var treatmentIntervalsQuery = treatmentIntervalRepository.GetTreatmentIntervalsByTreatmentIdAsQueryable(request.TreatmentId);

            return await treatmentIntervalsQuery
                .OrderBy(interval => interval.DayOfWeek)
                .Select(treatmentInterval => new TreatmentIntervalDetailsDto
                {
                    Id = treatmentInterval.Id,
                    TreatmentId = treatmentInterval.TreatmentId,
                    IsInactive = treatmentInterval.IsInactive,
                    DayOfWeek = treatmentInterval.DayOfWeek,
                    StartHour = treatmentInterval.StartHour,
                    StartMin = treatmentInterval.StartMin,
                    EndHour = treatmentInterval.EndHour,
                    EndMin = treatmentInterval.EndMin
                }).ToPagedListAsync(request.PageData);
        }
    }
}
