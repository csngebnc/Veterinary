using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
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
        private readonly IIdentityService identityService;

        public DeleteMedicalRecordTextTemplateCommandHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository, IIdentityService identityService)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteMedicalRecordTextTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            await medicalRecordTextTemplateRepository.DeleteAsync(request.TemplateId);
            return Unit.Value;
        }
    }
}
