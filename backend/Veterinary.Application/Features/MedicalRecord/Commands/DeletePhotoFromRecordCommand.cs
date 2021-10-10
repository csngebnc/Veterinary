using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordFeatures.Commands
{
    public class DeletePhotoFromRecordCommand : IRequest
    {
        public Guid RecordId { get; set; }
        public Guid PhotoId { get; set; }
    }

    public class DeletePhotoFromRecordCommandHandler : IRequestHandler<DeletePhotoFromRecordCommand, Unit>
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;
        private readonly IPhotoService photoService;

        public DeletePhotoFromRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository, IPhotoService photoService)
        {
            this.medicalRecordRepository = medicalRecordRepository;
            this.photoService = photoService;
        }

        public async Task<Unit> Handle(DeletePhotoFromRecordCommand request, CancellationToken cancellationToken)
        {
            var record = await medicalRecordRepository.GetMedicalRecordWithDetailsAsync(request.RecordId);
            var photo = record.Photos.Where(photo => photo.Id == request.PhotoId).SingleOrDefault();

            if(photo == null)
            {
                throw new EntityNotFoundException();
            }

            record.Photos.Remove(photo);
            photoService.RemovePhoto(photo.PhotoUrl);

            await medicalRecordRepository.UpdateAsync(record);

            return Unit.Value;
        }
    }
}
