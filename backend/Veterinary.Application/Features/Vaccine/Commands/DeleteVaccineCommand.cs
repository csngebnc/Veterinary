using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Commands
{
    public class DeleteVaccineCommand : IRequest
    {
        public Guid VaccineId { get; set; }
    }

    public class DeleteVaccineCommandHandler : IRequestHandler<DeleteVaccineCommand, Unit>
    {
        private readonly IVaccineRepository vaccineRepository;
        private readonly IIdentityService identityService;

        public DeleteVaccineCommandHandler(IVaccineRepository vaccineRepository, IIdentityService identityService)
        {
            this.vaccineRepository = vaccineRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            if (!(await vaccineRepository.CanBeDeleted(request.VaccineId)))
            {
                throw new MethodNotAllowedException("Az oltástípus nem törölhető, mert már legalább egy állatnál rögzítésre került.");
            }

            await vaccineRepository.DeleteAsync(request.VaccineId);

            return Unit.Value;
        }
    }
}
