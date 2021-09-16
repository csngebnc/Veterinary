using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineRecordFeatures.Commands
{
    public class DeleteVaccineRecordCommand : IRequest
    {
        public Guid RecordId { get; set; }
    }

    public class DeleteVaccineRecordCommandHandler : IRequestHandler<DeleteVaccineRecordCommand, Unit>
    {
        private readonly IVaccineRecordRepository vaccineRecordRepository;

        public DeleteVaccineRecordCommandHandler(IVaccineRecordRepository vaccineRecordRepository)
        {
            this.vaccineRecordRepository = vaccineRecordRepository;
        }

        public async Task<Unit> Handle(DeleteVaccineRecordCommand request, CancellationToken cancellationToken)
        {
            await vaccineRecordRepository.DeleteAsync(request.RecordId);
            return Unit.Value;
        }
    }

    public class DeleteVaccineRecordCommandValidator : AbstractValidator<DeleteVaccineRecordCommand>
    {
        public DeleteVaccineRecordCommandValidator()
        {
            RuleFor(x => x.RecordId).NotNull()
                .WithMessage("Oltási alkalom kiválasztása kötelező.");
        }
    }
}
