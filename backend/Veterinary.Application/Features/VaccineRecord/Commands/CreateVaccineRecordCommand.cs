using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineRecordFeatures.Commands
{
    public class CreateVaccineRecordCommand : IRequest<VaccineRecordDto>
    {
        public CreateVaccineRecordCommandData Data { get; set; }
    }

    public class CreateVaccineRecordCommandData
    {
        public DateTime Date { get; set; }
        public Guid AnimalId { get; set; }
        public Guid VaccineId { get; set; }
    }

    public class CreateVaccineRecordCommandHandler : IRequestHandler<CreateVaccineRecordCommand, VaccineRecordDto>
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IVaccineRepository vaccineRepository;
        private readonly IVaccineRecordRepository vaccineRecordRepository;
        private readonly IIdentityService identityService;

        public CreateVaccineRecordCommandHandler(
            IAnimalRepository animalRepository, 
            IVaccineRepository vaccineRepository, 
            IVaccineRecordRepository vaccineRecordRepository,
            IIdentityService identityService)
        {
            this.animalRepository = animalRepository;
            this.vaccineRepository = vaccineRepository;
            this.vaccineRecordRepository = vaccineRecordRepository;
            this.identityService = identityService;
        }

        public async Task<VaccineRecordDto> Handle(CreateVaccineRecordCommand request, CancellationToken cancellationToken)
        {
            var vaccine = await vaccineRepository.FindAsync(request.Data.VaccineId);
            var animal = await animalRepository.FindAsync(request.Data.AnimalId);

            if (animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var vaccineRecord = new VaccineRecord
            {
                Date = request.Data.Date,
                AnimalId = request.Data.AnimalId,
                VaccineId = request.Data.VaccineId
            };

            await vaccineRecordRepository.InsertAsync(vaccineRecord);

            return new VaccineRecordDto
            {
                Id = vaccineRecord.Id,
                Date = vaccineRecord.Date,
                AnimalId = vaccineRecord.AnimalId,
                VaccineId = vaccineRecord.VaccineId,
                VaccineName = vaccine.Name,
                AnimalName = animal.Name
            };
        }
    }

    public class CreateVaccineRecordCommandValidator : AbstractValidator<CreateVaccineRecordCommandData>
    {
        public CreateVaccineRecordCommandValidator(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, IIdentityService identityService)
        {
            RuleFor(x => x.Date).NotNull()
                .WithMessage("Dátum megadása kötelező.")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("A beadás időpontja nem lehet jövőbeli dátum.");
            RuleFor(x => x.AnimalId).NotNull()
                .WithMessage("Állat kiválasztása kötelező.");
        }
    }
}
