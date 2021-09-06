using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Features.VaccineFeatures.Commands;
using Veterinary.Application.Features.VaccineFeatures.Queries;
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
        [HttpGet]
        public Task<List<VaccineDto>> GetVaccines()
        {
            return mediator.Send(new GetVaccinesQuery());
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
    }
}
