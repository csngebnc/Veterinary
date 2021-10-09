using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Queries
{
    public class SearchVeterinaryUserQuery : IRequest<List<VeterinaryUserDto>>
    {
        public string SearchParam { get; set; }
    }

    public class SearchVeterinaryUserQueryHandler : IRequestHandler<SearchVeterinaryUserQuery, List<VeterinaryUserDto>>
    {
        private readonly IVeterinaryUserRepository veterinaryUserRepository;

        public SearchVeterinaryUserQueryHandler(IVeterinaryUserRepository veterinaryUserRepository)
        {
            this.veterinaryUserRepository = veterinaryUserRepository;
        }

        public async Task<List<VeterinaryUserDto>> Handle(SearchVeterinaryUserQuery request, CancellationToken cancellationToken)
        {
            var result = await veterinaryUserRepository.SearchQueryable(request.SearchParam.Trim().ToLower()).ToListAsync();

            return result.Select(u => new VeterinaryUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Address = u.Address,
                PhoneNumber = u.PhoneNumber,
                PhotoUrl = u.PhotoUrl,
            }).ToList();
        }
    }

    public class SearchVeterinaryUserQueryValidator : AbstractValidator<SearchVeterinaryUserQuery>
    {
        public SearchVeterinaryUserQueryValidator()
        {
            RuleFor(x => x.SearchParam).NotEmpty()
                .WithMessage("A szűrő tartalma nem lehet üres, vagy csupa szóköz.");
        }
    }
}
