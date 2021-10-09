using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities;
using Veterinary.Application.Extensions;

namespace Veterinary.Application.Features.VeterinaryUserFeatures.Queries
{
    public class SearchVeterinaryUserPagedQuery : IRequest<PagedList<VeterinaryUserDto>>
    {
        public string SearchParam { get; set; }
        public PageData PageData { get; set; }
    }

    public class SearchVeterinaryUserPagedQueryHandler : IRequestHandler<SearchVeterinaryUserPagedQuery, PagedList<VeterinaryUserDto>>
    {
        private readonly IVeterinaryUserRepository veterinaryUserRepository;

        public SearchVeterinaryUserPagedQueryHandler(IVeterinaryUserRepository veterinaryUserRepository)
        {
            this.veterinaryUserRepository = veterinaryUserRepository;
        }

        public async Task<PagedList<VeterinaryUserDto>> Handle(SearchVeterinaryUserPagedQuery request, CancellationToken cancellationToken)
        {
            var searchParam = request.SearchParam ?? "";
            var result = veterinaryUserRepository.SearchQueryable(searchParam.Trim().ToLower());

            return await result.Select(u => new VeterinaryUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Address = u.Address,
                PhoneNumber = u.PhoneNumber,
                PhotoUrl = u.PhotoUrl,
            }).ToPagedListAsync(request.PageData);
        }
    }
}
