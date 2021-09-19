using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentFeatures.Queries
{
    public class GetTreatmentsByDoctorIdQuery : IRequest<List<TreatmentDto>>
    {
        public Guid DoctorId { get; set; }
        public bool GetAll { get; set; }
    }

    public class GetTreatmentsByDoctorIdQueryHandler : IRequestHandler<GetTreatmentsByDoctorIdQuery, List<TreatmentDto>>
    {
        private readonly ITreatmentRepository treatmentRepository;

        public GetTreatmentsByDoctorIdQueryHandler(ITreatmentRepository treatmentRepository)
        {
            this.treatmentRepository = treatmentRepository;
        }

        public async Task<List<TreatmentDto>> Handle(GetTreatmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var treatments = await treatmentRepository.GetTreatmentsByDoctorId(request.DoctorId, request.GetAll);

            return treatments.Select(treatment => new TreatmentDto
            {
                Id = treatment.Id,
                Name = treatment.Name,
                Duration = treatment.Duration,
                IsInactive = treatment.IsInactive                
            }).ToList();
        }
    }
}
