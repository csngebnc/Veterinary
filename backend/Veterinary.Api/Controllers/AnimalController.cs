using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.AnimalFeatures.Commands;
using Veterinary.Application.Features.AnimalFeatures.Queries;
using Veterinary.Application.Services;

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
        [HttpGet("{userId}")]
        public Task<PagedList<OwnedAnimalDto>> GetOwnedAnimals(string userId, [FromQuery]PageData pageData)
        {
            return mediator.Send(new GetActiveOwnedAnimalsQuery
            {
                OwnerId = Guid.Parse(userId),
                PageData = pageData
            });
        }
        
        [Authorize(Policy = "User")]
        [HttpPost("{userId}")]
        public Task CreateAnimal(string userId, [FromForm] CreateAnimalCommand.CreateAnimalCommandData data)
        {
            return mediator.Send(new CreateAnimalCommand
            {
                UserId = userId,
                Data = data
            });
        }
        
    }
}
