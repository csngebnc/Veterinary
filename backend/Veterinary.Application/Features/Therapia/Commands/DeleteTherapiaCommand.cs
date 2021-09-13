using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.TherapiaFeatures.Commands
{
    public class DeleteTherapiaCommand : IRequest
    {
        public Guid TherapiaId { get; set; }
    }

    public class DeleteTherapiaCommandHandler : IRequestHandler<DeleteTherapiaCommand, Unit>
    {
        private readonly ITherapiaRepository therapiaRepository;

        public DeleteTherapiaCommandHandler(ITherapiaRepository therapiaRepository)
        {
            this.therapiaRepository = therapiaRepository;
        }

        public async Task<Unit> Handle(DeleteTherapiaCommand request, CancellationToken cancellationToken)
        {
            if(!(await therapiaRepository.CanBeDeleted(request.TherapiaId)))
            {
                throw new MethodNotAllowedException("A kezelés nem törölhető, mert már rögzítették legalább egy kórlapon.");
            }

            await therapiaRepository.DeleteAsync(request.TherapiaId);

            return Unit.Value;
        }
    }
}
