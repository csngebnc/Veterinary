using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Application.Features.Manager.Commands;
using Veterinary.Application.Features.Manager.Queries;
using Veterinary.Application.Features.VeterinaryUserFeatures.Queries;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;

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

        [Authorize(Policy = "Doctor")]
        [HttpGet("user/{userId}")]
        public async Task<VeterinaryUserDto> GetUser(Guid userId)
        {
            return await mediator.Send(new GetVeterinaryUserQuery { UserId = userId });
        }

        [Authorize(Policy = "Manager")]
        [HttpGet("doctors")]
        public async Task<List<DoctorWithRoleDto>> GetDoctorsWithRole()
        {
            return await mediator.Send(new GetDoctorsWithRoleQuery());
        }

        [Authorize(Policy = "Manager")]
        [HttpPost("role")]
        public async Task ChangeUserRole(ChangeUserRoleCommand command)
        {
            await mediator.Send(command);
        }
    }
}
