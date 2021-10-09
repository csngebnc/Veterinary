using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Commands;
using Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Queries;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.MedicalRecordTextTemplate.BasePath)]
    public class TemplateController : PublicControllerBase
    {
        private readonly IMediator mediator;

        public TemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("list")]
        [Authorize(Policy = "Doctor")]
        public async Task<List<MedicalRecordTextTemplate>> GetTemplates()
        {
            return await mediator.Send(new GetMedicalRecordTextTemplatesQuery());
        }

        [HttpGet("{templateId}")]
        [Authorize(Policy = "Doctor")]
        public async Task<MedicalRecordTextTemplate> GetTemplate(Guid templateId)
        {
            return await mediator.Send(new GetMedicalRecordTextTemplateQuery { TemplateId = templateId });
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<MedicalRecordTextTemplate> CreateTemplate(CreateMedicalRecordTextTemplateCommand command)
        {
            return await mediator.Send(command);
        }

        [HttpPut]
        [Authorize(Policy = "Manager")]
        public async Task UpdateTemplate(UpdateMedicalRecordTextTemplateCommand command)
        {
            await mediator.Send(command);
        }

        [HttpDelete("{templateId}")]
        [Authorize(Policy = "Manager")]
        public async Task DeleteTemplate(Guid templateId)
        {
            await mediator.Send(new DeleteMedicalRecordTextTemplateCommand { TemplateId = templateId });
        }
    }
}
