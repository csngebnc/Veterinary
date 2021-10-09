using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Queries
{
    public class GetMedicalRecordTextTemplatesQuery : IRequest<List<MedicalRecordTextTemplate>>
    {
    }

    public class GetMedicalRecordTextTemplatesQueryHandler : IRequestHandler<GetMedicalRecordTextTemplatesQuery, List<MedicalRecordTextTemplate>>
    {
        private readonly IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository;

        public GetMedicalRecordTextTemplatesQueryHandler(IMedicalRecordTextTemplateRepository medicalRecordTextTemplateRepository)
        {
            this.medicalRecordTextTemplateRepository = medicalRecordTextTemplateRepository;
        }

        public async Task<List<MedicalRecordTextTemplate>> Handle(GetMedicalRecordTextTemplatesQuery request, CancellationToken cancellationToken)
        {
            return await medicalRecordTextTemplateRepository.GetAllAsQueryable().ToListAsync();
        }
    }
}
