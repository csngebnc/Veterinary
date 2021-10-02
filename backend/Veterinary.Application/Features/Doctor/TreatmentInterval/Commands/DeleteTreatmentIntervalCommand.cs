using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Commands
{
    public class DeleteTreatmentIntervalCommand : IRequest
    {
        public Guid TreatmentIntervalId { get; set; }
    }

    public class DeleteTreatmentIntervalCommandHandler : IRequestHandler<DeleteTreatmentIntervalCommand, Unit>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IIdentityService identityService;

        public DeleteTreatmentIntervalCommandHandler(
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteTreatmentIntervalCommand request, CancellationToken cancellationToken)
        {
            var treatmentInterval = await treatmentIntervalRepository.FindAsync(request.TreatmentIntervalId);
            var treatment = await treatmentRepository.FindAsync(treatmentInterval.TreatmentId);

            if (treatment.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            await treatmentIntervalRepository.DeleteAsync(treatmentInterval.Id);

            return Unit.Value;
        }
    }

}
