using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.AnimalFeatures.Commands;
using Veterinary.Application.Features.AnimalFeatures.Queries;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Animals.BasePath)]
    public class AnimalController : PublicControllerBase
    {
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;

        public AnimalController(IMediator mediator, IIdentityService identityService)
        {
            this.mediator = mediator;
            this.identityService = identityService;
        }

        [Authorize(Policy = "User")]
        [HttpGet("list/{userId}")]
        public async Task<PagedList<OwnedAnimalDto>> GetOwnedAnimals(Guid userId, [FromQuery] PageData pageData, [FromQuery] bool isArchived)
        {
            return await mediator.Send(new GetOwnedAnimalsQuery
            {
                OwnerId = userId,
                Archived = isArchived,
                PageData = pageData
            });
        }

        [Authorize(Policy = "User")]
        [HttpGet("list/{userId}/select")]
        public async Task<List<AnimalForSelectDto>> GetAnimalsForSelect(Guid userId)
        {
            return await mediator.Send(new GetActiveAnimalsForSelectByUserIdQuery
            {
                UserId = userId
            });
        }

        [Authorize(Policy = "User")]
        [HttpPost("{userId}")]
        public async Task CreateAnimal(Guid userId, [FromForm] CreateAnimalCommand.CreateAnimalCommandData data)
        {
            await mediator.Send(new CreateAnimalCommand
            {
                UserId = userId,
                Data = data
            });
        }

        [Authorize(Policy = "User")]
        [HttpPut("{animalId}")]
        public async Task UpdateAnimal(Guid animalId, UpdateAnimalCommand.UpdateAnimalCommandData data)
        {
            await mediator.Send(new UpdateAnimalCommand
            {
                AnimalId = animalId,
                Data = data
            });
        }

        [Authorize(Policy = "User")]
        [HttpGet("{animalId}")]
        public async Task<AnimalDto> GetAnimal(Guid animalId)
        {
            return await mediator.Send(new GetAnimalQuery
            {
                AnimalId = animalId,
            });
        }

        [Authorize(Policy = "User")]
        [HttpDelete("{animalId}/photo")]
        public async Task<string> DeleteAnimalPhoto(Guid animalId)
        {
            return await mediator.Send(new DeleteAnimalPhotoCommand
            {
                AnimalId = animalId
            });
        }

        [Authorize(Policy = "User")]
        [HttpPost("{userId}/photo")]
        public async Task<string> UpdateAnimalPhoto(Guid userId, [FromForm] UpdateAnimalPhotoCommand.UpdateAnimalPhotoCommandData data)
        {
            return await mediator.Send(new UpdateAnimalPhotoCommand
            {
                UserId = userId,
                Data = data
            });
        }

        [Authorize(Policy = "User")]
        [HttpPut("{animalId}/status")]
        public async Task UpdateAnimalArchiveStatus(Guid animalId)
        {
            await mediator.Send(new UpdateAnimalArchiveStatusCommand
            {
                AnimalId = animalId
            });
        }

        [Authorize(Policy = "User")]
        [HttpDelete("{animalId}")]
        public async Task DeleteAnimal(Guid animalId)
        {
            await mediator.Send(new DeleteAnimalCommand
            {
                AnimalId = animalId
            });
        }

    }
}
