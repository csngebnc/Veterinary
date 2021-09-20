using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;

namespace Veterinary.Application.Features.Doctor.HolidayFeatures.Queries
{
    public class GetHolidaysForDoctorQuery : IRequest<PagedList<HolidayDto>>
    {
        public PageData PageData { get; set; }
    }

    public class GetHolidaysQueryHandler : IRequestHandler<GetHolidaysForDoctorQuery, PagedList<HolidayDto>>
    {
        private readonly IHolidayRepository HolidayRepository;

        public GetHolidaysQueryHandler(IHolidayRepository HolidayRepository)
        {
            this.HolidayRepository = HolidayRepository;
        }

        public async Task<PagedList<HolidayDto>> Handle(GetHolidaysForDoctorQuery request, CancellationToken cancellationToken)
        {
            return await HolidayRepository
                .GetAllAsQueryable()
                .Select(holiday => new HolidayDto
                {
                    Id = holiday.Id,
                    DoctorId = holiday.DoctorId,
                    StartDate = holiday.StartDate,
                    EndDate = holiday.EndDate
                })
                .ToPagedListAsync(request.PageData);
        }
    }
}
