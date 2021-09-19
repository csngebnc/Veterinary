using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands
{
    public class CreateTreatmentCommand : IRequest<TreatmentDto>
    {
        public CreateTreatmentCommandData Data { get; set; }
    }

    public class CreateTreatmentCommandData
    {
        public string Name { get; set; }
        public int Duration { get; set; }
    }

    public class CreateTreatmentCommandHandler : IRequestHandler<CreateTreatmentCommand, TreatmentDto>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IIdentityService identityService;

        public CreateTreatmentCommandHandler(ITreatmentRepository treatmentRepository, IIdentityService identityService)
        {
            this.treatmentRepository = treatmentRepository;
            this.identityService = identityService;
        }

        public async Task<TreatmentDto> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
        {
            if(await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var treatment = new Treatment
            {
                Name = request.Data.Name,
                Duration = request.Data.Duration,
                DoctorId = identityService.GetCurrentUserId()
            };

            await treatmentRepository.InsertAsync(treatment);

            return new TreatmentDto
            {
                Id = treatment.Id,
                Name = treatment.Name,
                Duration = treatment.Duration,
                IsInactive = treatment.IsInactive
            };
        }
    }

    public class CreateTreatmentCommanndValidator : AbstractValidator<CreateTreatmentCommand>
    {
        public CreateTreatmentCommanndValidator()
        {
            RuleFor(x => x.Data).NotNull().SetValidator(new CreateTreatmentCommandDataValidator());
        }
    }

    public class CreateTreatmentCommandDataValidator : AbstractValidator<CreateTreatmentCommandData>
    {
        public CreateTreatmentCommandDataValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("A kezelés nevének megadása kötelező.");
            RuleFor(x => x.Duration).GreaterThan(0)
                .WithMessage("A kezelés hosszának legalább 1 percnek kell lennie.");
        }
    }
}
