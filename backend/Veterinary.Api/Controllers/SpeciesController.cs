using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Features.AnimalSpeciesFeatures.Queries;

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
    }
}
