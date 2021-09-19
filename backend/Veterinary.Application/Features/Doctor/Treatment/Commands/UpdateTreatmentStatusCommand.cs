using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands
{
    public class UpdateTreatmentStatusCommand : IRequest
    {
        public Guid TreatmentId { get; set; }
    }

    public class UpdateTreatmentStatusCommandHandler : IRequestHandler<UpdateTreatmentStatusCommand, Unit>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IIdentityService identityService;

        public UpdateTreatmentStatusCommandHandler(ITreatmentRepository treatmentRepository, IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateTreatmentStatusCommand request, CancellationToken cancellationToken)
        {
            var treatment = await treatmentRepository.FindAsync(request.TreatmentId);

            if (treatment.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync(RoleEnum.ManagerDoctor.Value()))
            {
                throw new ForbiddenException();
            }

            treatment.IsInactive = !treatment.IsInactive;

            await treatmentRepository.UpdateAsync(treatment);
            return Unit.Value;
        }
    }
}
