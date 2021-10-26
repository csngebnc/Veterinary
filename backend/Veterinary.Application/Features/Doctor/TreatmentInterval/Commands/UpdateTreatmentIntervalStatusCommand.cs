using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Commands
{
    public class UpdateTreatmentIntervalStatusCommand : IRequest
    {
        public Guid TreatmentIntervalId { get; set; }
    }

    public class UpdateTreatmentIntervalStatusCommandHandler : IRequestHandler<UpdateTreatmentIntervalStatusCommand, Unit>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IIdentityService identityService;

        public UpdateTreatmentIntervalStatusCommandHandler(
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateTreatmentIntervalStatusCommand request, CancellationToken cancellationToken)
        {
            var treatmentInterval = await treatmentIntervalRepository.FindAsync(request.TreatmentIntervalId);
            var treatment = await treatmentRepository.FindAsync(treatmentInterval.TreatmentId);
            
            if (treatment.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            treatmentInterval.IsInactive = !treatmentInterval.IsInactive;

            await treatmentIntervalRepository.UpdateAsync(treatmentInterval);

            return Unit.Value;
        }
    }
}
