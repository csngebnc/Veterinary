using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Application.Features.VeterinaryUser.Queries;
using Veterinary.Application.Services;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.VeterinaryUser.BasePath)]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;

        public UserController(IMediator mediator, IIdentityService identityService)
        {
            this.mediator = mediator;
            this.identityService = identityService;
        }

        [Authorize(Policy = "Doctor")]
        [HttpGet("search")]
        public async Task<List<VeterinaryUserDto>> SearchUsers(string param)
        {
            return await mediator.Send(new SearchVeterinaryUserQuery { SearchParam = param });
        }
    }
}
