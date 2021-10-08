using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities;
using Veterinary.Shared.Constants;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Commands
{
    public class UploadUserPhotoCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public IFormFile Photo { get; set; }
    }

    public class UploadUserPhotoCommandHandler : IRequestHandler<UploadUserPhotoCommand, string>
    {
        private readonly IPhotoService photoService;
        private readonly IIdentityService identityService;
        private readonly IVeterinaryUserRepository veterinaryUserRepository;

        public UploadUserPhotoCommandHandler(
            IPhotoService photoService, 
            IIdentityService identityService,
            IVeterinaryUserRepository veterinaryUserRepository)
        {
            this.photoService = photoService;
            this.identityService = identityService;
            this.veterinaryUserRepository = veterinaryUserRepository;
        }

        public async Task<string> Handle(UploadUserPhotoCommand request, CancellationToken cancellationToken)
        {
            if(request.UserId != identityService.GetCurrentUserId())
            {
                throw new ForbiddenException();
            }

            var user = await veterinaryUserRepository.FindAsync(request.UserId);

            if (photoService.RemovePhoto(user.PhotoUrl))
            {
                 var photoUrl = await photoService.UploadPhoto("Users", request.UserId.ToString(), request.Photo);
                if (photoUrl == null)
                {
                    photoUrl = UrlConstants.PlaceholderImage;
                }

                user.PhotoUrl = photoUrl;
                await veterinaryUserRepository.UpdateAsync(user);
            }

            return user.PhotoUrl;
        }
    }
}
