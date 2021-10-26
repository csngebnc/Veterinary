using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Domain.Entities.MedicationEntities;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Commands
{
    public class UpdateMedicalRecordCommand : IRequest
    {
        public Guid MedicalRecordId { get; set; }
        public UpdateMedicalRecordCommandData Data { get; set; }
        public List<EditMedicationRecordDto> Medications { get; set; }
        public List<EditTherapiaRecordDto> Therapias { get; set; }
    }

    public class UpdateMedicalRecordCommandData
    {
        public DateTime Date { get; set; }
        public string OwnerEmail { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? AnimalId { get; set; }
        public string HtmlContent { get; set; }
    }

    public class EditMedicationRecordDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
    }

    public class EditTherapiaRecordDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
    }

    public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, Unit>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;
        private readonly IIdentityService identityService;

        public UpdateMedicalRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository, IIdentityService identityService)
        {
            this.medicalRecordRepository = medicalRecordRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            if (await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var record = await medicalRecordRepository.GetMedicalRecordWithDetailsAsync(request.MedicalRecordId);
            record.Date = request.Data.Date.ToLocalTime();
            record.OwnerEmail = request.Data.OwnerEmail;
            record.OwnerId = request.Data.OwnerId;
            record.AnimalId = request.Data.AnimalId;
            record.HtmlContent = request.Data.HtmlContent;

            record.MedicationRecords.Clear();
            record.TherapiaRecords.Clear();

            await medicalRecordRepository.UpdateAsync(record);

            if(request.Medications != null)
            {
                foreach (var medication in request.Medications)
                {
                    record.MedicationRecords.Add(new MedicationRecord
                    {
                        MedicalRecordId = record.Id,
                        Amount = medication.Amount,
                        MedicationId = medication.Id
                    });
                }
            }

            if(request.Therapias != null)
            {
                foreach (var therapia in request.Therapias)
                {
                    record.TherapiaRecords.Add(new TherapiaRecord
                    {
                        MedicalRecordId = record.Id,
                        Amount = therapia.Amount,
                        TherapiaId = therapia.Id
                    });
                }
            }

            await medicalRecordRepository.UpdateAsync(record);

            return Unit.Value;
        }
    }

    public class UpdateMedicalRecordCommandValidator : AbstractValidator<UpdateMedicalRecordCommandData>
    {
        public UpdateMedicalRecordCommandValidator()
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
