using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using static Veterinary.Application.Shared.Dtos.MedicalRecordDto;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Queries
{
    public class GetMedicalRecordsForAnimalQuery : IRequest<PagedList<MedicalRecordDto>>
    {
        public Guid AnimalId { get; set; }
        public PageData PageData { get; set; }
    }

    public class GetMedicalRecordsForAnimalQueryHandler : IRequestHandler<GetMedicalRecordsForAnimalQuery, PagedList<MedicalRecordDto>>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;
        private readonly IIdentityService identityService;
        private readonly IAnimalRepository animalRepository;

        public GetMedicalRecordsForAnimalQueryHandler(
            IMedicalRecordRepository medicalRecordRepository, 
            IIdentityService identityService,
            IAnimalRepository animalRepository)
        {
            this.medicalRecordRepository = medicalRecordRepository;
            this.identityService = identityService;
            this.animalRepository = animalRepository;
        }

        public async Task<PagedList<MedicalRecordDto>> Handle(GetMedicalRecordsForAnimalQuery request, CancellationToken cancellationToken)
        {
            var animal = await animalRepository.FindAsync(request.AnimalId);
            if(animal.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var recordQuery = medicalRecordRepository.GetMedicalRecordsByAnimalIdQueryable(request.AnimalId);

            return await recordQuery.Select(record => new MedicalRecordDto
            {
                Id = record.Id,
                Date = record.Date,
                DoctorId = record.DoctorId,
                DoctorName = record.Doctor.Name,
                OwnerId = record.OwnerId,
                OwnerEmail = record.OwnerEmail,
                OwnerName = record.Owner == null ? "" : record.Owner.Name,
                AnimalId = record.AnimalId,
                AnimalName = record.Animal == null ? "" : record.Animal.Name,
                HtmlContent = record.HtmlContent,
                PhotoUrls = record.Photos.Select(photo => photo.PhotoUrl).ToList(),
                MedicationRecords = record.MedicationRecords.Select(medicationRecord => new MedicationRecordOnRecord
                {
                    MedicationId = medicationRecord.Medication.Id,
                    Name = medicationRecord.Medication.Name,
                    Amount = medicationRecord.Amount,
                    UnitName = medicationRecord.Medication.UnitName
                }).ToList(),
                TherapiaRecords = record.TherapiaRecords.Select(therapiaRecord => new TherapiaRecordOnRecord
                {
                    TherapiaId = therapiaRecord.Therapia.Id,
                    Name = therapiaRecord.Therapia.Name,
                    Amount = therapiaRecord.Amount
                }).ToList()

            }).ToPagedListAsync(request.PageData);
        }
    }

    public class GetMedicalRecordsForAnimalQueryValidator : AbstractValidator<GetMedicalRecordsForAnimalQuery>
    {
        public GetMedicalRecordsForAnimalQueryValidator()
        {
            RuleFor(x => x.PageData)
                .NotEmpty();
        }
    }
}
