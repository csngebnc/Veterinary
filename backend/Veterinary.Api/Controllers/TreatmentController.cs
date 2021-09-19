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
using Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands;
using Veterinary.Application.Features.Doctor.TreatmentFeatures.Queries;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.Treatment.BasePath)]
    public class TreatmentController : PublicControllerBase
    {
        private readonly IMediator mediator;

        public TreatmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Policy = "User")]
        [HttpGet("user/{doctorId}")]
        public Task<List<TreatmentDto>> GetTreatmentsByDoctorId(Guid doctorId)
        {
            return mediator.Send(new GetTreatmentsByDoctorIdQuery
            {
                DoctorId = doctorId,
                GetAll = false
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpGet("doctor/{doctorId}")]
        public Task<List<TreatmentDto>> GetAllTreatmentsByDoctorId(Guid doctorId)
        {
            return mediator.Send(new GetTreatmentsByDoctorIdQuery
            {
                DoctorId = doctorId,
                GetAll = true
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPost]
        public async Task<TreatmentDto> CreateTreatment(CreateTreatmentCommandData data)
        {
            return await mediator.Send(new CreateTreatmentCommand
            {
                Data = data
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPut("update/{treatmentId}")]
        public async Task UpdateTreatment(Guid treatmentId, UpdateTreatmentCommandData data)
        {
            await mediator.Send(new UpdateTreatmentCommand
            {
                TreatmentId = treatmentId,
                Data = data
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPatch]
        public async Task UpdateTreatmentStatus(Guid treatmentId)
        {
            await mediator.Send(new UpdateTreatmentStatusCommand
            {
                TreatmentId = treatmentId
            });
        }

        [Authorize(Policy = "Doctor")]
        [HttpDelete]
        public async Task DeleteTreatment(Guid treatmentId)
        {
            await mediator.Send(new DeleteTreatmentCommand
            {
                TreatmentId = treatmentId
            });
        }
    }
}