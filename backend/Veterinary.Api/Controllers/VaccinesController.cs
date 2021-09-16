using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.VaccineFeatures.Commands;
using Veterinary.Application.Features.VaccineFeatures.Queries;
using Veterinary.Application.Features.VaccineRecordFeatures.Commands;
using Veterinary.Application.Features.VaccineRecordFeatures.Queries;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Vaccine.BasePath)]
    public class VaccinesController : PublicControllerBase
    {
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;

        public VaccinesController(IMediator mediator, IIdentityService identityService)
        {
            this.mediator = mediator;
            this.identityService = identityService;
        }

        [Authorize(Policy = "User")]
        [HttpGet("search")]
        public async Task<List<VaccineSearchResultDto>> SearchVaccines(string param)
        {
            return await mediator.Send(new SearchVaccineQuery { SearchParam = param });
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<VaccineDto> GetVaccine(Guid vaccineId)
        {
            return await mediator.Send(new GetVaccineQuery { VaccineId = vaccineId });
        }

        [Authorize(Policy = "User")]
        [HttpGet("list")]
        public async Task<List<VaccineDto>> GetVaccines()
        {
            return await mediator.Send(new GetVaccinesQuery());
        }

        [Authorize(Policy = "Manager")]
        [HttpPost]
        public async Task<VaccineDto> CreateVaccine(string name)
        {
            return await mediator.Send(new CreateVaccineCommand
            {
                Name = name
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPut]
        public async Task UpdateVaccine(UpdateVaccineCommand command)
        {
            await mediator.Send(command);
        }

        [Authorize(Policy = "Manager")]
        [HttpPatch]
        public async Task UpdateVaccineStatus(Guid vaccineId)
        {
            await mediator.Send(new UpdateVaccineStatusCommand
            {
                Id = vaccineId
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpDelete]
        public async Task DeleteVaccine(Guid vaccineId)
        {
            await mediator.Send(new DeleteVaccineCommand
            {
                VaccineId = vaccineId
            });
        }

        [Authorize(Policy = "User")]
        [HttpGet("records/{animalId}")]
        public async Task<PagedList<VaccineRecordDto>> GetVaccineRecords(Guid animalId, [FromQuery] PageData pageData)
        {
            return await mediator.Send(new GetAnimalVaccineRecordsQuery
            {
                AnimalId = animalId,
                PageData = pageData
            });
        }

        [Authorize(Policy = "User")]
        [HttpGet("record/{recordId}")]
        public async Task<VaccineRecordDto> GetVaccineRecord(Guid recordId)
        {
            return await mediator.Send(new GetVaccineRecordQuery
            {
                VaccineRecordId = recordId
            });
        }

        [Authorize(Policy = "User")]
        [HttpPost("record")]
        public async Task<VaccineRecordDto> CreateVaccineRecord(CreateVaccineRecordCommandData data)
        {
            return await mediator.Send(new CreateVaccineRecordCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "User")]
        [HttpPut("record")]
        public async Task UpdateVaccineRecord(UpdateVaccineRecordCommandData data)
        {
            await mediator.Send(new UpdateVaccineRecordCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "User")]
        [HttpDelete("record")]
        public async Task DeleteVaccineRecord(Guid vaccineRecordId)
        {
            await mediator.Send(new DeleteVaccineRecordCommand
            {
                RecordId = vaccineRecordId
            });
        }
    }
}
