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
using Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Commands;
using Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Queries;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Treatment.Intervals)]
    public class TreatmentIntervalController : PublicControllerBase
    {
        private readonly IMediator mediator;

        public TreatmentIntervalController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("list/{treatmentId}")]
        public async Task<PagedList<TreatmentIntervalDetailsDto>> GetTreatmentIntervalsWithDetails(Guid treatmentId, [FromQuery]PageData pageData)
        {
            return await mediator.Send(new GetTreatmentIntervalsWithDetailsQuery { TreatmentId = treatmentId, PageData = pageData });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPost]
        public async Task<TreatmentIntervalDetailsDto> CreateTreatmentInterval(Guid doctorId, CreateTreatmentIntervalCommandData data)
        {
            return await mediator.Send(new CreateTreatmentIntervalCommand
            {
                DoctorId = doctorId,
                Data = data
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPut]
        public async Task UpdateTreatmentInterval(Guid doctorId, UpdateTreatmentIntervalCommandData data)
        {
            await mediator.Send(new UpdateTreatmentIntervalCommand
            {
                DoctorId = doctorId,
                Data = data
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPatch]
        public async Task UpdateTreatmentIntervalStatus(Guid treatmentIntervalId)
        {
            await mediator.Send(new UpdateTreatmentIntervalStatusCommand
            {
                TreatmentIntervalId = treatmentIntervalId
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpDelete]
        public async Task DeleteTreatmentInterval(Guid vaccineId)
        {
            await mediator.Send(new DeleteTreatmentIntervalCommand
            {
                TreatmentIntervalId = vaccineId
            });
        }
    }
}
