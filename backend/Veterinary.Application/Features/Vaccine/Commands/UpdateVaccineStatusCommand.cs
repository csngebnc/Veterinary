using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        public UpdateVaccineStatusCommandHandler(IVaccineRepository vaccineRepository)
        {
            this.vaccineRepository = vaccineRepository;
        }

        public async Task<Unit> Handle(UpdateVaccineStatusCommand request, CancellationToken cancellationToken)
        {
            var vaccine = await vaccineRepository.FindAsync(request.Id);
            vaccine.IsInactive = !vaccine.IsInactive;

            await vaccineRepository.UpdateAsync(vaccine);
            return Unit.Value;
        }
    }
}
