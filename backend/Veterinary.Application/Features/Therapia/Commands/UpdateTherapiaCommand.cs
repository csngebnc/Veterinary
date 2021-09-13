using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Application.Features.TherapiaFeatures.Commands
{
    public class UpdateTherapiaCommand : IRequest
    {
        public UpdateTherapiaCommandData Data { get; set; }
    }

    public class UpdateTherapiaCommandData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class UpdateTherapiaCommandHandler : IRequestHandler<UpdateTherapiaCommand, Unit>
    {
        private readonly ITherapiaRepository therapiaRepository;

        public UpdateTherapiaCommandHandler(ITherapiaRepository therapiaRepository)
        {
            this.therapiaRepository = therapiaRepository;
        }

        public async Task<Unit> Handle(UpdateTherapiaCommand request, CancellationToken cancellationToken)
        {
            var therapia = await therapiaRepository.FindAsync(request.Data.Id);

            therapia.Name = request.Data.Name;
            therapia.Price = request.Data.Price;

            await therapiaRepository.UpdateAsync(therapia);
            return Unit.Value;
        }
    }

    public class UpdateTherapiaCommandDataValidator : AbstractValidator<UpdateTherapiaCommandData>
    {
        public UpdateTherapiaCommandDataValidator(ITherapiaRepository therapiaRepository)
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
