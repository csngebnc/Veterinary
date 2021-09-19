using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands
{
    public class DeleteTreatmentCommand : IRequest
    {
        public Guid TreatmentId { get; set; }
    }

    public class DeleteTreatmentCommandHandler : IRequestHandler<DeleteTreatmentCommand, Unit>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IIdentityService identityService;

        public DeleteTreatmentCommandHandler(ITreatmentRepository treatmentRepository, IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = await treatmentRepository.FindAsync(request.TreatmentId);

            if(treatment.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync(RoleEnum.ManagerDoctor.Value()))
            {
                throw new ForbiddenException();
            }

            if(!await treatmentRepository.CanBeDeleted(request.TreatmentId))
            {
                throw new MethodNotAllowedException("A kezelés nem törölhető, mert van hozzá idősáv rögzítve.");
            }

            await treatmentRepository.DeleteAsync(request.TreatmentId);

            return Unit.Value;
        }
    }
}
