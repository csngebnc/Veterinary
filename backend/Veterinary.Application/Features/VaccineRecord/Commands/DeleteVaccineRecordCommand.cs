using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineRecordFeatures.Commands
{
    public class DeleteVaccineRecordCommand : IRequest
    {
        public Guid RecordId { get; set; }
    }

    public class DeleteVaccineRecordCommandHandler : IRequestHandler<DeleteVaccineRecordCommand, Unit>
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;
        private readonly IAnimalRepository animalRepository;
        private readonly IIdentityService identityService;

        public DeleteVaccineRecordCommandHandler(
            IVaccineRecordRepository vaccineRecordRepository,
            IAnimalRepository animalRepository,
            IIdentityService identityService)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
            this.animalRepository = animalRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteVaccineRecordCommand request, CancellationToken cancellationToken)
        {
            var record = await vaccineRecordRepository.FindAsync(request.RecordId);
            var animal = await animalRepository.FindAsync(record.AnimalId);

            if (animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            await vaccineRecordRepository.DeleteAsync(request.RecordId);
            return Unit.Value;
        }
    }

    public class DeleteVaccineRecordCommandValidator : AbstractValidator<DeleteVaccineRecordCommand>
    {
        public DeleteVaccineRecordCommandValidator()
        {
            RuleFor(x => x.RecordId).NotNull()
                .WithMessage("Oltási alkalom kiválasztása kötelező.");
        }
    }
}
