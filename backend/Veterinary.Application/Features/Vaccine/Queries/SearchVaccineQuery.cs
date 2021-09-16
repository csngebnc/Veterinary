using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Application.Features.VaccineFeatures.Queries
{
    public class SearchVaccineQuery : IRequest<List<VaccineSearchResultDto>>
    {
        public string SearchParam { get; set; }
    }

    public class VaccineSearchResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class SearchVaccineQueryHandler : IRequestHandler<SearchVaccineQuery, List<VaccineSearchResultDto>>
    {
        private readonly IVaccineRepository vaccineRepository;

        public SearchVaccineQueryHandler(IVaccineRepository vaccineRepository)
        {
            this.vaccineRepository = vaccineRepository;
        }

        public async Task<List<VaccineSearchResultDto>> Handle(SearchVaccineQuery request, CancellationToken cancellationToken)
        {
            return (await vaccineRepository.Search(request.SearchParam))
                .Select(v => new VaccineSearchResultDto
                {
                    Id= v.Id,
                    Name= v.Name
                }).ToList();
        }
    }

    public class SearchVaccineQueryValidator : AbstractValidator<SearchVaccineQuery>
    {
        public SearchVaccineQueryValidator()
        {
            RuleFor(x => x.SearchParam).NotEmpty()
                .WithMessage("A szűrő tartalma nem lehet üres, vagy csupa szóköz.");
        }
    }
}
