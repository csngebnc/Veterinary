using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Queries
{
    public class GetVeterinaryUserQuery : IRequest<VeterinaryUserDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetVeterinaryUserQueryHandler : IRequestHandler<GetVeterinaryUserQuery, VeterinaryUserDto>
    {
        private readonly IVeterinaryUserRepository veterinaryUserRepository;

        public GetVeterinaryUserQueryHandler(IVeterinaryUserRepository veterinaryUserRepository)
        {
            this.veterinaryUserRepository = veterinaryUserRepository;
        }

        public async Task<VeterinaryUserDto> Handle(GetVeterinaryUserQuery request, CancellationToken cancellationToken)
        {
            var user = await veterinaryUserRepository.FindAsync(request.UserId);
            
            return new VeterinaryUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                PhotoUrl = user.PhotoUrl
            };
        }
    }
}
