using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Queries
{
    public class GetMedicalRecordForEditQuery : IRequest<MedicalRecordEditDto>
    {
        public Guid RecordId { get; set; }
    }

    public class MedicalRecordEditDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid DoctorId { get; set; }
        public string OwnerEmail { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? AnimalId { get; set; }
        public string HtmlContent { get; set; }

        public List<MedicationForRecordEditDto> MedicationRecords { get; set; }
        public List<TherapiaForRecordEditDto> TherapiaRecords { get; set; }
        public List<PhotoOnRecord> PhotoUrls { get; set; }
    }

    public class PhotoOnRecord
    {
        public Guid Id { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class MedicationForRecordEditDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string UnitName { get; set; }
    }

    public class TherapiaForRecordEditDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }

    public class GetMedicalRecordForEditQueryHandler : IRequestHandler<GetMedicalRecordForEditQuery, MedicalRecordEditDto>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;

        public GetMedicalRecordForEditQueryHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            this.medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<MedicalRecordEditDto> Handle(GetMedicalRecordForEditQuery request, CancellationToken cancellationToken)
        {
            var medicalRecord = await medicalRecordRepository.GetMedicalRecordWithDetailsAsync(request.RecordId);

            return new MedicalRecordEditDto
            {
                Id = medicalRecord.Id,
                Date = medicalRecord.Date,
                HtmlContent = medicalRecord.HtmlContent,
                AnimalId = medicalRecord.AnimalId,
                DoctorId = medicalRecord.DoctorId,
                OwnerId = medicalRecord.OwnerId,
                OwnerEmail = medicalRecord.OwnerEmail,
                MedicationRecords = medicalRecord.MedicationRecords.Select(medicationRecord => new MedicationForRecordEditDto
                {
                    Id = medicationRecord.Id,
                    Name = medicationRecord.Medication.Name,
                    Amount = medicationRecord.Amount,
                    UnitName = medicationRecord.Medication.UnitName

                }).ToList(),
                TherapiaRecords = medicalRecord.TherapiaRecords.Select(therapiaRecord => new TherapiaForRecordEditDto
                {
                    Id = therapiaRecord.Id,
                    Name = therapiaRecord.Therapia.Name,
                    Amount = therapiaRecord.Amount
                }).ToList(),
                PhotoUrls = medicalRecord.Photos.Select(record => new PhotoOnRecord
                {
                    Id = record.Id,
                    PhotoUrl= record.PhotoUrl
                }).ToList()
            };
        }
    }
}
