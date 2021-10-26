using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Commands
{
    public class UpdateVaccineCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateVaccineCommandHandler : IRequestHandler<UpdateVaccineCommand, Unit>
    {
        private readonly IVaccineRepository vaccineRepository;
        private readonly IIdentityService identityService;

        public UpdateVaccineCommandHandler(IVaccineRepository vaccineRepository, IIdentityService identityService)
        {
            this.vaccineRepository = vaccineRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateVaccineCommand request, CancellationToken cancellationToken)
        {
            if (!await identityService.IsInRoleAsync("ManagerDoctor"))
            {
                throw new ForbiddenException();
            }

            var vaccine = await vaccineRepository.FindAsync(request.Id);
            vaccine.Name = request.Name;

            await vaccineRepository.UpdateAsync(vaccine);
            return Unit.Value;
        }
    }

    public class UpdateAnimalSpeciesCommandValidator : AbstractValidator<UpdateVaccineCommand>
    {
        public UpdateAnimalSpeciesCommandValidator(IAnimalSpeciesRepository animalSpeciesRepository)
        {
            RuleFor(x => x.Name).NotNull()
                .WithMessage("Az oltás neve nem lehet üres.")
                .MustAsync(async (speciesName, cancellationToken) => !(await animalSpeciesRepository.AnyByNameAsync(speciesName)))
                .WithMessage("A megadott névvel már létezik oltástípus.");
        }
    }
}
