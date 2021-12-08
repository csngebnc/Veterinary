using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Features.AnimalSpeciesFeatures.Commands;
using Veterinary.Application.Features.AnimalSpeciesFeatures.Queries;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.AnimalSpecies.BasePath)]
    public class SpeciesController : PublicControllerBase
    {
        private readonly IMediator mediator;

        public SpeciesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public Task<List<AnimalSpeciesDto>> GetAnimalSpecies()
        {
            return mediator.Send(new GetAnimalSpeciesQuery());
        }

        [Authorize(Policy = "Manager")]
        [HttpPost]
        public async Task<AnimalSpeciesDto> CreateAnimalSpecies(CreateAnimalSpeciesCommand command)
        {
            return await mediator.Send(command);
        }

        [Authorize(Policy = "Manager")]
        [HttpPut]
        public async Task UpdateAnimalSpecies(UpdateAnimalSpeciesCommand command)
        {
            await mediator.Send(command);
        }

        [Authorize(Policy = "Manager")]
        [HttpPatch]
        public async Task UpdateAnimalSpeciesStatus(Guid speciesId)
        {
            await mediator.Send(new UpdateAnimalSpeciesStatusCommand
            {
                Id = speciesId
            });
        }

        [Authorize(Policy = "Manager")]
        [HttpDelete]
        public async Task DeleteAnimalSpecies(Guid speciesId)
        {
            await mediator.Send(new DeleteAnimalSpeciesCommand
            {
                SpeciesId = speciesId
            });
        }
    }
}
