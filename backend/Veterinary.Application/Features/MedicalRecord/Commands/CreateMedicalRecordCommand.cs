using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Application.Services;
using System.Collections.Generic;
using System.Linq;
using Veterinary.Domain.Entities.MedicationEntities;
using Veterinary.Domain.Entities.TherapiaEntities;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Commands
{
    public class CreateMedicalRecordCommand : IRequest<Guid>
    {
        public CreateMedicalRecordCommandData Data { get; set; }
        public List<EditMedicationRecordDto> Medications { get; set; }
        public List<EditTherapiaRecordDto> Therapias { get; set; }
    }

    public class CreateMedicalRecordCommandData
    {
        public DateTime Date { get; set; }
        public string OwnerEmail { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? AnimalId { get; set; }
        public string HtmlContent { get; set; }
    }

    public class NewMedicationRecordDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
    }

    public class NewTherapiaRecordDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
    }

    public class CreateMedicalRecordCommandHandler : IRequestHandler<CreateMedicalRecordCommand, Guid>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;
        public IIdentityService identityService { get; }

        public CreateMedicalRecordCommandHandler(
            IMedicalRecordRepository medicalRecordRepository,
            IIdentityService identityService)
        {
            this.medicalRecordRepository = medicalRecordRepository;
            this.identityService = identityService;
        }


        public async Task<Guid> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            if(await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var medicalRecord = new MedicalRecord
            {
                Date = request.Data.Date.ToLocalTime(),
                DoctorId = identityService.GetCurrentUserId(),
                OwnerEmail = request.Data.OwnerEmail,
                OwnerId = request.Data.OwnerId,
                AnimalId = request.Data.AnimalId,
                HtmlContent = request.Data.HtmlContent
            };

            await medicalRecordRepository.InsertAsync(medicalRecord);

            medicalRecord.MedicationRecords = request.Medications?.Select(medication => new MedicationRecord
            {
                MedicalRecordId = medicalRecord.Id,
                Amount = medication.Amount,
                MedicationId = medication.Id
            }).ToList();

            medicalRecord.TherapiaRecords = request.Therapias?.Select(therapia => new TherapiaRecord
            {
                MedicalRecordId = medicalRecord.Id,
                Amount = therapia.Amount,
                TherapiaId = therapia.Id
            }).ToList();

            await medicalRecordRepository.UpdateAsync(medicalRecord);

            return medicalRecord.Id;
        }
    }

    public class CreateMedicalRecordCommandValidator : AbstractValidator<CreateMedicalRecordCommandData>
    {
        public CreateMedicalRecordCommandValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                    .WithMessage("A dátum megadása kötelező.");
            RuleFor(x => x.OwnerEmail)
                .NotEmpty()
                    .When(x => x.OwnerId == null)
                    .WithMessage("A gazdi e-mail címének megadása kötelező, ha nincs regisztrált fiókja.");
        }
    }
}
