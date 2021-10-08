using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Application.Features.Manager.Commands;
using Veterinary.Application.Features.Manager.Queries;
using Veterinary.Application.Features.VeterinaryUserFeatures.Commands;
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

        [Authorize(Policy = "User")]
        [HttpPost("get-photo-url/{userId}")]
        public async Task<string> GetPhotoUrl(Guid userId)
        {
            return await mediator.Send(new GetUserPhotoUrl { UserId = userId });
        }

        [Authorize(Policy = "User")]
        [HttpDelete("delete-photo/{userId}")]
        public async Task<string> DeletePhoto(Guid userId)
        {
            return await mediator.Send(new DeleteUserPhotoCommand { UserId = userId });
        }

        [Authorize(Policy = "User")]
        [HttpPost("upload-photo")]
        public async Task<string> UploadPhoto([FromForm]UploadUserPhotoCommand command)
        {
            return await mediator.Send(command);
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
