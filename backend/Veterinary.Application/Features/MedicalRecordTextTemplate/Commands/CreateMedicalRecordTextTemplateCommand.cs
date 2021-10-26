using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Commands
{
    public class CreateMedicalRecordTextTemplateCommand : IRequest<MedicalRecordTextTemplate>
    {
        public string Name { get; set; }
        public string HtmlContent { get; set; }
    }

    public class CreateMedicalRecordTextTemplateCommandHandler : IRequestHandler<CreateMedicalRecordTextTemplateCommand, MedicalRecordTextTemplate>
    {
        private readonly IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository;
        private readonly IIdentityService identityService;

        public CreateMedicalRecordTextTemplateCommandHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository, IIdentityService identityService)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
            this.identityService = identityService;
        }

        public async Task<MedicalRecordTextTemplate> Handle(CreateMedicalRecordTextTemplateCommand request, CancellationToken cancellationToken)
        {
            if(!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var template = new MedicalRecordTextTemplate
            {
                Name = request.Name,
                HtmlContent = request.HtmlContent
            };

            await medicalRecordTextTemplateRepository.InsertAsync(template);

            return template;
        }
    }

    public class CreateMedicalRecordTextTemplateCommandValidator : AbstractValidator<CreateMedicalRecordTextTemplateCommand>
    {
        public CreateMedicalRecordTextTemplateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("A sablon neve nem lehet üres.");

            RuleFor(x => x.HtmlContent)
                .NotEmpty()
                    .WithMessage("A sablon tartalma nem lehet üres.");
        }
    }
}
