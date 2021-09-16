using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineRecordFeatures.Commands
{
    public class UpdateVaccineRecordCommand : IRequest
    {
        public UpdateVaccineRecordCommandData Data { get; set; }
    }

    public class UpdateVaccineRecordCommandData
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid AnimalId { get; set; }
        public Guid VaccineId { get; set; }
    }

    public class UpdateVaccineRecordCommandHandler : IRequestHandler<UpdateVaccineRecordCommand, Unit>
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;

        public UpdateVaccineRecordCommandHandler(IVaccineRecordRepository vaccineRecordRepository)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
        }

        public async Task<Unit> Handle(UpdateVaccineRecordCommand request, CancellationToken cancellationToken)
        {
            var record = await vaccineRecordRepository.FindAsync(request.Data.Id);

            record.Date = request.Data.Date;
            record.AnimalId = request.Data.AnimalId;
            record.VaccineId = request.Data.VaccineId;

            await vaccineRecordRepository.UpdateAsync(record);

            return Unit.Value;
        }
    }

    public class UpdateVaccineRecordCommandDataValidator : AbstractValidator<UpdateVaccineRecordCommandData>
    {
        public UpdateVaccineRecordCommandDataValidator(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, IIdentityService identityService)
        {
            RuleFor(x => x.Id).NotNull()
                .WithMessage("Oltási alkalom kiválasztása kötelező.");
            RuleFor(x => x.Date).NotNull()
                .WithMessage("Dátum megadása kötelező.")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("A beadás időpontja nem lehet jövőbeli dátum.");
            RuleFor(x => x.AnimalId).NotNull()
                .WithMessage("Állat kiválasztása kötelező.")
                .MustAsync(async (animalId, cancellationToken) =>
                {
                    var animal = await animalRepository.FindAsync(animalId);
                    return animal.OwnerId == identityService.GetCurrentUserId() || !(await identityService.IsInRoleAsync("User"));
                })
                .WithMessage("Nincs jogosultsága a művelet végrehajtásához.");
    }
}
}
