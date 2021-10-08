using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Queries
{
    public class GetUserPhotoUrl : IRequest<string>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserPhotoUrlHandler : IRequestHandler<GetUserPhotoUrl, string>
    {
        private readonly IVeterinaryUserRepository veterinaryUserRepository;

        public GetUserPhotoUrlHandler(IVeterinaryUserRepository veterinaryUserRepository)
        {
            this.veterinaryUserRepository = veterinaryUserRepository;
        }

        public async Task<string> Handle(GetUserPhotoUrl request, CancellationToken cancellationToken)
        {
            var user = await veterinaryUserRepository.FindAsync(request.UserId);
            return user.PhotoUrl;
        }
    }
}
