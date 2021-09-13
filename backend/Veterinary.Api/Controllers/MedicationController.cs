using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.MedicationFeatures.Commands;
using Veterinary.Application.Features.MedicationFeatures.Queries;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Medication.BasePath)]
    public class MedicationController : PublicControllerBase
    {
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;

        public MedicationController(IMediator mediator, IIdentityService identityService)
        {
            this.mediator = mediator;
            this.identityService = identityService;
        }

        [Authorize(Policy = "Doctor")]
        [HttpGet]
        public async Task<PagedList<MedicationDto>> GetMedicationsWithDetails([FromQuery] PageData pageData)
        {
            return await mediator.Send(new GetMedicationsQuery
            {
                PageData = pageData
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPost]
        public async Task<MedicationDto> CreateMedication(CreateMedicationCommandData data)
        {
            return await mediator.Send(new CreateMedicationCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPut]
        public async Task UpdateMedication(UpdateMedicationCommandData data)
        {
            await mediator.Send(new UpdateMedicationCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPatch]
        public async Task UpdateMedicationStatus(Guid medicationId)
        {
            await mediator.Send(new UpdateMedicationStatusCommand
            {
                Id = medicationId
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpDelete]
        public async Task DeleteMedication(Guid medicationId)
        {
            await mediator.Send(new DeleteMedicationCommand
            {
                MedicationId = medicationId
            });
        }
    }
}
