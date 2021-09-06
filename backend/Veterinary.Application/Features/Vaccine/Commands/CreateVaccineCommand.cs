using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Commands
{
    public class CreateVaccineCommand : IRequest<VaccineDto>
    {
        public string Name { get; set; }
    }

    public class CreateVaccineCommandHandler : IRequestHandler<CreateVaccineCommand, VaccineDto>
    {
        private readonly IVaccineRepository vaccineRepository;

        public CreateVaccineCommandHandler(IVaccineRepository vaccineRepository)
        {
            this.vaccineRepository = vaccineRepository;
        }

        public async Task<VaccineDto> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
        {
            var vaccine = await vaccineRepository.InsertAsync(new Vaccine
            {
                Name = request.Name,
            });

            return new VaccineDto { Id = vaccine.Id, Name = vaccine.Name };
        }
    }

    public class CreateVaccineCommandValidator : AbstractValidator<CreateVaccineCommand>
    {
        public CreateVaccineCommandValidator(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            RuleFor(x => x.Name).NotNull().NotEmpty()
                .WithMessage("Az oltás neve nem lehet üres.")
                .MustAsync(async (speciesName, cancellationToken) => !(await animalSpeciesRepository.AnyByNameAsync(speciesName)))
                .WithMessage("A megadott névvel már létezik oltástípus.");
        }
    }
}
