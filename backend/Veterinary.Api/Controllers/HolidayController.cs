using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.Doctor.HolidayFeatures.Commands;
using Veterinary.Application.Features.Doctor.HolidayFeatures.Queries;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Holiday.BasePath)]
    public class HolidayController : PublicControllerBase
    {
        private readonly IMediator mediator;

        public HolidayController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Policy = "User")]
        [HttpGet("list/{doctorId}")]
        public async Task<PagedList<HolidayDto>> GetHolidays(Guid doctorId, [FromQuery] PageData pageData)
        {
            return await mediator.Send(new GetHolidaysForDoctorQuery
            {
                PageData = pageData
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPost]
        public async Task<HolidayDto> CreateHoliday(CreateHolidayCommandData data)
        {
            return await mediator.Send(new CreateHolidayCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPut]
        public async Task UpdateHoliday(UpdateHolidayCommandData data)
        {
            await mediator.Send(new UpdateHolidayCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpDelete]
        public async Task DeleteHoliday(Guid HolidayId)
        {
            await mediator.Send(new DeleteHolidayCommand
            {
                HolidayId = HolidayId
            });
        }
    }
}
