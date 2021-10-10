using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.TherapiaFeatures.Commands;
using Veterinary.Application.Features.TherapiaFeatures.Queries;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Therapia.BasePath)]
    public class TherapiaController : PublicControllerBase
    {
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;

        public TherapiaController(IMediator mediator, IIdentityService identityService)
        {
            this.mediator = mediator;
            this.identityService = identityService;
        }

        [Authorize(Policy = "Doctor")]
        [HttpGet("search")]
        public async Task<List<TherapiaForSelectDto>> SearchTherapia(string searchParam)
        {
            return await mediator.Send(new SearchTherapiaQuery
            {
                SearchParam = searchParam
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpGet]
        public async Task<PagedList<TherapiaDto>> GetTherapias([FromQuery] PageData pageData)
        {
            return await mediator.Send(new GetTherapiasQuery
            {
                PageData = pageData
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPost]
        public async Task<TherapiaDto> CreateTherapia(CreateTherapiaCommandData data)
        {
            return await mediator.Send(new CreateTherapiaCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPut]
        public async Task UpdateTherapia(UpdateTherapiaCommandData data)
        {
            await mediator.Send(new UpdateTherapiaCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpPatch]
        public async Task UpdateTherapiaStatus(Guid therapiaId)
        {
            await mediator.Send(new UpdateTherapiaStatusCommand
            {
                Id = therapiaId
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpDelete]
        public async Task DeleteTherapia(Guid therapiaId)
        {
            await mediator.Send(new DeleteTherapiaCommand
            {
                TherapiaId = therapiaId
            });
        }
    }
}
