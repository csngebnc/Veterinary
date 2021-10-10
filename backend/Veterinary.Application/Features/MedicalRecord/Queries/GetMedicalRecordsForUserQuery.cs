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
using Veterinary.Domain.Entities.MedicalRecordEntities;
using static Veterinary.Application.Shared.Dtos.MedicalRecordDto;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Queries
{
    public class GetMedicalRecordsForUserQuery : IRequest<PagedList<MedicalRecordDto>>
    {
        public Guid UserId { get; set; }
        public PageData PageData { get; set; }
    }

    public class GetMedicalRecordsForUserQueryHandler : IRequestHandler<GetMedicalRecordsForUserQuery, PagedList<MedicalRecordDto>>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;
        private readonly IIdentityService identityService;

        public GetMedicalRecordsForUserQueryHandler(
            IMedicalRecordRepository medicalRecordRepository, IIdentityService identityService)
        {
            this.medicalRecordRepository = medicalRecordRepository;
            this.identityService = identityService;
        }

        public async Task<PagedList<MedicalRecordDto>> Handle(GetMedicalRecordsForUserQuery request, CancellationToken cancellationToken)
        {
            if(request.UserId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var recordQuery = medicalRecordRepository.GetMedicalRecordsByUserIdQueryable(request.UserId);

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

    public class GetMedicalRecordsForUserQueryValidator : AbstractValidator<GetMedicalRecordsForUserQuery>
    {
        public GetMedicalRecordsForUserQueryValidator()
        {
            RuleFor(x => x.PageData)
                .NotEmpty();
        }
    }
}
