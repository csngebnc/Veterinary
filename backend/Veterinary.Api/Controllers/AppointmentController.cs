using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.AppointmentFeatures.Commands;
using Veterinary.Application.Features.AppointmentFeatures.Queries;
using Veterinary.Application.Features.VeterinaryUserFeatures.Queries;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Shared;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Appointment.BasePath)]
    public class AppointmentController : PublicControllerBase
    {
        private readonly IMediator mediator;

        public AppointmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Policy = "User")]
        [HttpGet("statuses")]
        public List<LabelValuePair<AppointmentStatusEnum>> GetStatuses()
        {
            return EnumExtensions.GetLabelValuePairs<AppointmentStatusEnum>();
        }

        [Authorize(Policy = "User")]
        [HttpGet("doctors")]
        public async Task<List<DoctorForAppointmentDto>> GetDoctors()
        {
            return await mediator.Send(new GetDoctorsForAppointmentQuery());
        }

        [HttpGet("available-times")]
        public async Task<List<AvailableTime>> GetDoctorTreatmentAvailableTimes([FromQuery] DateTime date, [FromQuery] Guid doctorId, [FromQuery] Guid treatmentId)
        {
            return await mediator.Send(new GetDoctorAvailableTimesQuery
            {
                Date = date,
                DoctorId = doctorId,
                TreatmentId = treatmentId
            });
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task BookAnAppointment(CreateAppointmentCommand command)
        {
            await mediator.Send(command);
        }

        [Authorize(Policy = "Doctor")]
        [HttpGet("list/doctor")]
        public async Task<PagedList<AppointmentForDoctorDto>> GetAppointmentsForDoctor([FromQuery] GetDoctorsAppointmentsQuery query)
        {
            return await mediator.Send(query);
        }

        [Authorize(Policy = "User")]
        [HttpGet("list/user")]
        public async Task<PagedList<AppointmentForUserDto>> GetAppointmentsForUser([FromQuery] GetUserAppointmentsQuery query)
        {
            return await mediator.Send(query);
        }

        [Authorize(Policy = "User")]
        [HttpPut("status")]
        public async Task UpdateAppointmentStatus(UpdateAppointmentStatusCommand command)
        {
            await mediator.Send(command);
        }
    }
}
