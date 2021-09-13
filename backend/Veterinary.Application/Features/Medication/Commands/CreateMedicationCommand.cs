using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Application.Features.MedicationFeatures.Commands
{
    public class CreateMedicationCommand : IRequest<MedicationDto>
    {
        public CreateMedicationCommandData Data { get; set; }
    }

    public class CreateMedicationCommandData
    {
        public string Name { get; set; }
        public string UnitName { get; set; }
        public double Unit { get; set; }
        public double PricePerUnit { get; set; }
    }

    public class CreateMedicationCommandHandler : IRequestHandler<CreateMedicationCommand, MedicationDto>
    {
        private readonly IMedicationRepository medicationRepository;

        public CreateMedicationCommandHandler(IMedicationRepository medicationRepository)
        {
            this.medicationRepository = medicationRepository;
        }

        public async Task<MedicationDto> Handle(CreateMedicationCommand request, CancellationToken cancellationToken)
        {
            var medication = await medicationRepository.InsertAsync(new Medication
            {
                Name = request.Data.Name,
                UnitName = request.Data.UnitName,
                Unit = request.Data.Unit,
                PricePerUnit = request.Data.PricePerUnit
            });

            return new MedicationDto 
            { 
                Id = medication.Id, 
                Name = medication.Name, 
                Unit = medication.Unit, 
                UnitName = medication.UnitName, 
                PricePerUnit = medication.PricePerUnit,
                IsInactive = medication.IsInactive
            };
        }
    }

    public class CreateMedicationCommandValidator : AbstractValidator<CreateMedicationCommandData>
    {
        public CreateMedicationCommandValidator(IMedicationRepository medicationRepository)
        {
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
