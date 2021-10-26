using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Commands
{
    public class UpdateVaccineStatusCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class UpdateVaccineStatusCommandHandler : IRequestHandler<UpdateVaccineStatusCommand, Unit>
    {
        private readonly IVaccineRepository vaccineRepository;
        private readonly IIdentityService identityService;

        public UpdateVaccineStatusCommandHandler(IVaccineRepository vaccineRepository, IIdentityService identityService)
        {
            this.vaccineRepository = vaccineRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateVaccineStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var vaccine = await vaccineRepository.FindAsync(request.Id);
            vaccine.IsInactive = !vaccine.IsInactive;

            await vaccineRepository.UpdateAsync(vaccine);
            return Unit.Value;
        }
    }
}
