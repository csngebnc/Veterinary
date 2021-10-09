using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Queries
{
    public class GetMedicalRecordTextTemplateQuery : IRequest<MedicalRecordTextTemplate>
    {
        public Guid TemplateId { get; set; }
    }

    public class GetMedicalRecordTextTemplateQueryHandler : IRequestHandler<GetMedicalRecordTextTemplateQuery, MedicalRecordTextTemplate>
    {
        private readonly IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository;

        public GetMedicalRecordTextTemplateQueryHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
        }

        public async Task<MedicalRecordTextTemplate> Handle(GetMedicalRecordTextTemplateQuery request, CancellationToken cancellationToken)
        {
            return await medicalRecordTextTemplateRepository.FindAsync(request.TemplateId);
        }
    }
}
