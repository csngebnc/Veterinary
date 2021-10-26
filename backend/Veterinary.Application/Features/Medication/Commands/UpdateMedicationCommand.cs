using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Application.Features.MedicationFeatures.Commands
{
    public class UpdateMedicationCommand : IRequest
    {
        public UpdateMedicationCommandData Data { get; set; }
    }

    public class UpdateMedicationCommandData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public double Unit { get; set; }
        public double PricePerUnit { get; set; }
    }

    public class UpdateMedicationCommandHandler : IRequestHandler<UpdateMedicationCommand, Unit>
    {
        private readonly IMedicationRepository medicationRepository;
        private readonly IIdentityService identityService;

        public UpdateMedicationCommandHandler(IMedicationRepository medicationRepository, IIdentityService identityService)
        {
            this.medicationRepository = medicationRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateMedicationCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var medication = await medicationRepository.FindAsync(request.Data.Id);

            medication.Name = request.Data.Name;
            medication.Unit = request.Data.Unit;
            medication.UnitName = request.Data.UnitName;
            medication.PricePerUnit = request.Data.PricePerUnit;

            await medicationRepository.UpdateAsync(medication);
            return Unit.Value;
        }
    }

    public class UpdateMedicationCommandDataValidator : AbstractValidator<UpdateMedicationCommandData>
    {
        public UpdateMedicationCommandDataValidator(IMedicationRepository medicationRepository)
        {
            RuleFor(x => x.Id).NotNull()
                .WithMessage("A gyógyszer azonosítójának megadása kötelező.");
            RuleFor(x => x.Name).NotNull()
                .WithMessage("A gyógyszer neve nem lehet üres.")
                .MustAsync(async (medicationName, cancellationToken) => !(await medicationRepository.AnyByNameAsync(medicationName)))
                .WithMessage("A megadott névvel már létezik gyógyszer.");
            RuleFor(x => x.Unit).NotNull()
                .WithMessage("A gyógyszer mennyiségének megadása kötelező.")
                .GreaterThanOrEqualTo(0.0)
                .WithMessage("A mennyiség értéke nem lehet negatív érték.");
            RuleFor(x => x.UnitName).NotEmpty()
                .WithMessage("Az egység nevének megadása kötelező.");
            RuleFor(x => x.PricePerUnit).NotNull()
                .WithMessage("Az ár megadása kötelező.")
                .GreaterThanOrEqualTo(0.0)
                .WithMessage("A gyógyszer egységenkénti ára nem lehet negatív érték.");
        }
    }
}
