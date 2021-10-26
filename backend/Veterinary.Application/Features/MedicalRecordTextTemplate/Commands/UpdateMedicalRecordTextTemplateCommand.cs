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
    public class UpdateMedicalRecordTextTemplateCommand : IRequest
    {
        public Guid TemplateId { get; set; }
        public string Name { get; set; }
        public string HtmlContent { get; set; }
    }

    public class UpdateMedicalRecordTextTemplateCommandHandler : IRequestHandler<UpdateMedicalRecordTextTemplateCommand, Unit>
    {
        private readonly IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository;
        private readonly IIdentityService identityService;

        public UpdateMedicalRecordTextTemplateCommandHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository, IIdentityService identityService)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateMedicalRecordTextTemplateCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var template = await medicalRecordTextTemplateRepository.FindAsync(request.TemplateId);

            template.Name = request.Name;
            template.HtmlContent = request.HtmlContent;

            await medicalRecordTextTemplateRepository.UpdateAsync(template);

            return Unit.Value;
        }
    }

    public class UpdateMedicalRecordTextTemplateCommandValidator : AbstractValidator<UpdateMedicalRecordTextTemplateCommand>
    {
        public UpdateMedicalRecordTextTemplateCommandValidator()
        {
            RuleFor(x => x.TemplateId)
                .NotEmpty()
                    .WithMessage("A sablon azonosítója nem lehet üres.");

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("A sablon neve nem lehet üres.");

            RuleFor(x => x.HtmlContent)
                .NotEmpty()
                    .WithMessage("A sablon tartalma nem lehet üres.");
        }
    }
}
