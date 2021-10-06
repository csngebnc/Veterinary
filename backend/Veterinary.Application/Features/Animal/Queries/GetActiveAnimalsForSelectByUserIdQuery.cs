using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Application.Features.AnimalFeatures.Queries
{
    public class GetActiveAnimalsForSelectByUserIdQuery : IRequest<List<AnimalForSelectDto>>
    {
        public Guid UserId { get; set; }
    }

    public class AnimalForSelectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GetActiveAnimalsForSelectByUserIdQueryHandler : IRequestHandler<GetActiveAnimalsForSelectByUserIdQuery, List<AnimalForSelectDto>>
    {
        private readonly IAnimalRepository animalRepository;
        private readonly IIdentityService identityService;

        public GetActiveAnimalsForSelectByUserIdQueryHandler(IAnimalRepository animalRepository, IIdentityService identityService)
        {
            this.animalRepository = animalRepository;
            this.identityService = identityService;
        }

        public async Task<List<AnimalForSelectDto>> Handle(GetActiveAnimalsForSelectByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            return await animalRepository.GetAllAsQueryable()
                .Where(animal => animal.OwnerId == request.UserId && !animal.IsDeleted && !animal.IsArchived)
                .Select(animal => new AnimalForSelectDto
                {
                    Id = animal.Id,
                    Name = animal.Name
                })
                .ToListAsync();
        }
    }
}
