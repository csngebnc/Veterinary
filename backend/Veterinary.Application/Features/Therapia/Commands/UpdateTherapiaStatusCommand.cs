using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        public UpdateTherapiaStatusCommandHandler(ITherapiaRepository therapiaRepository)
        {
            this.therapiaRepository = therapiaRepository;
        }

        public async Task<Unit> Handle(UpdateTherapiaStatusCommand request, CancellationToken cancellationToken)
        {
            var therapia = await therapiaRepository.FindAsync(request.Id);
            therapia.IsInactive = !therapia.IsInactive;

            await therapiaRepository.UpdateAsync(therapia);
            return Unit.Value;
        }
    }
}
