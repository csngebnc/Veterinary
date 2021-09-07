using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Application.Features.VeterinaryUser.Queries
{
    public class SearchVeterinaryUserQuery : IRequest<List<VeterinaryUserDto>>
    {
        public string SearchParam { get; set; }
    }

    public class VeterinaryUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
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
            var result = await veterinaryUserRepository.Search(request.SearchParam.Trim().ToLower());

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
