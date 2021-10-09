using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Commands
{
    public class DeleteMedicalRecordTextTemplateCommand : IRequest
    {
        public Guid TemplateId { get; set; }
    }

    public class DeleteMedicalRecordTextTemplateCommandHandler : IRequestHandler<DeleteMedicalRecordTextTemplateCommand, Unit>
    {
        private readonly IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository;

        public DeleteMedicalRecordTextTemplateCommandHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
        }

        public async Task<Unit> Handle(DeleteMedicalRecordTextTemplateCommand request, CancellationToken cancellationToken)
        {
            await medicalRecordTextTemplateRepository.DeleteAsync(request.TemplateId);
            return Unit.Value;
        }
    }
}
