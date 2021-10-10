using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Commands
{
    public class AddPhotoToRecordCommand : IRequest
    {
        public Guid RecordId { get; set; }
        public IFormFile Photo { get; set; }
    }

    public class AddPhotoToRecordCommandHandler : IRequestHandler<AddPhotoToRecordCommand, Unit>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;
        private readonly IPhotoService photoService;

        public AddPhotoToRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository, IPhotoService photoService)
        {
            this.medicalRecordRepository = medicalRecordRepository;
            this.photoService = photoService;
        }

        public async Task<Unit> Handle(AddPhotoToRecordCommand request, CancellationToken cancellationToken)
        {
            var record = await medicalRecordRepository.GetMedicalRecordWithDetailsAsync(request.RecordId);

            var photoPath = await photoService.UploadPhoto("MedicalRecords", request.RecordId.ToString(), request.Photo);

            record.Photos.Add(new MedicalRecordPhoto { MedicalRecordId = request.RecordId, PhotoUrl = photoPath });

            await medicalRecordRepository.UpdateAsync(record);

            return Unit.Value;
        }
    }

    public class AddPhotoToRecordCommandValidator : AbstractValidator<AddPhotoToRecordCommand>
    {
        public AddPhotoToRecordCommandValidator()
        {
            RuleFor(x => x.Photo)
                .NotNull()
                    .WithMessage("Kép csatolása kötelező.");
        }
    }
}
