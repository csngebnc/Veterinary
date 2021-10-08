using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities;
using Veterinary.Shared.Constants;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Commands
{
    public class DeleteUserPhotoCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
    }

    public class DeleteUserPhotoCommandHandler : IRequestHandler<DeleteUserPhotoCommand, string>
    {
        private readonly IPhotoService photoService;
        private readonly IIdentityService identityService;
        private readonly IVeterinaryUserRepository veterinaryUserRepository;

        public DeleteUserPhotoCommandHandler(
            IPhotoService photoService,
            IIdentityService identityService,
            IVeterinaryUserRepository veterinaryUserRepository)
        {
            this.photoService = photoService;
            this.identityService = identityService;
            this.veterinaryUserRepository = veterinaryUserRepository;
        }

        public async Task<string> Handle(DeleteUserPhotoCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != identityService.GetCurrentUserId())
            {
                throw new ForbiddenException();
            }

            var user = await veterinaryUserRepository.FindAsync(request.UserId);

            if (photoService.RemovePhoto(user.PhotoUrl))
            {
                user.PhotoUrl = UrlConstants.PlaceholderImage;
                
                await veterinaryUserRepository.UpdateAsync(user);
            }

            return user.PhotoUrl;
        }
    }
}
