using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.TherapiaFeatures.Commands
{
    public class CreateTherapiaCommand : IRequest<TherapiaDto>
    {
        public CreateTherapiaCommandData Data { get; set; }
    }

    public class CreateTherapiaCommandData
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class CreateTherapiaCommandHandler : IRequestHandler<CreateTherapiaCommand, TherapiaDto>
    {
        private readonly ITherapiaRepository therapiaRepository;

        public CreateTherapiaCommandHandler(ITherapiaRepository therapiaRepository)
        {
            this.therapiaRepository = therapiaRepository;
        }

        public async Task<TherapiaDto> Handle(CreateTherapiaCommand request, CancellationToken cancellationToken)
        {
            var therapia = await therapiaRepository.InsertAsync(new Therapia
            {
                Name = request.Data.Name,
                Price = request.Data.Price
                
            });

            return new TherapiaDto
            { 
                Id = therapia.Id, 
                Name = therapia.Name,
                Price = therapia.Price,
                IsInactive = therapia.IsInactive
            };
        }
    }

    public class CreateTherapiaCommandValidator : AbstractValidator<CreateTherapiaCommandData>
    {
        public CreateTherapiaCommandValidator(ITherapiaRepository therapiaRepository)
        {
            RuleFor(x => x.Name).NotNull()
                .WithMessage("A gyógyszer neve nem lehet üres.")
                .MustAsync(async (therapiaName, cancellationToken) => !(await therapiaRepository.AnyByNameAsync(therapiaName)))
                .MustAsync(async (therapiaName, cancellationToken) => !(await therapiaRepository.AnyByNameAsync(therapiaName)))
                .WithMessage("A megadott névvel már létezik gyógyszer.");
            RuleFor(x => x.Price).NotNull()
                .WithMessage("Az ár megadása kötelező.")
                .GreaterThanOrEqualTo(0.0)
                .WithMessage("A gyógyszer egységenkénti ára nem lehet negatív érték.");
        }
    }
}
