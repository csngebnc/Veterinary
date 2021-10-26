using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.TherapiaFeatures.Commands
{
    public class UpdateTherapiaStatusCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class UpdateTherapiaStatusCommandHandler : IRequestHandler<UpdateTherapiaStatusCommand, Unit>
    {
        private readonly ITherapiaRepository therapiaRepository;
        private readonly IIdentityService identityService;

        public UpdateTherapiaStatusCommandHandler(ITherapiaRepository therapiaRepository, IIdentityService identityService)
        {
            this.therapiaRepository = therapiaRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateTherapiaStatusCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var therapia = await therapiaRepository.FindAsync(request.Id);
            therapia.IsInactive = !therapia.IsInactive;

            await therapiaRepository.UpdateAsync(therapia);
            return Unit.Value;
        }
    }
}
