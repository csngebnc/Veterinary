using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
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

        public CreateMedicalRecordTextTemplateCommandHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
        }

        public async Task<MedicalRecordTextTemplate> Handle(CreateMedicalRecordTextTemplateCommand request, CancellationToken cancellationToken)
        {
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
