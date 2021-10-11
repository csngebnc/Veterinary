using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Veterinary.Api.Common;
using Veterinary.Api.Common.BaseControllers;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Features.AppointmentFeatures.Queries;
using Veterinary.Application.Features.MedicalRecordFeatures.Commands;
using Veterinary.Application.Features.MedicalRecordFeatures.Queries;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;

namespace Veterinary.Api.Controllers
{
    [Route(ApiResources.MedicalRecord.BasePath)]
    public class MedicalRecordController : PublicControllerBase
    {
        private readonly IMediator mediator;
        private readonly PdfService pdfService;

        public MedicalRecordController(IMediator mediator, PdfService pdfService)
        {
            this.mediator = mediator;
            this.pdfService = pdfService;
        }

        [Authorize(Policy = "User")]
        [HttpGet("pdf/{recordId}")]
        public async Task<IActionResult> GeneratePDF(Guid recordId)
        {
            var bytes = await pdfService.GeneratePDF(recordId);
            var content = new MemoryStream(bytes);
            var contentType = "APPLICATION/octet-stream";
            return File(content, contentType);
        }

        [Authorize(Policy = "User")]
        [HttpGet("details/animal")]
        public async Task<PagedList<MedicalRecordDto>> GetMedicalRecordsForAnimal([FromQuery]GetMedicalRecordsForAnimalQuery query)
        {
            return await mediator.Send(query);
        }

        [Authorize(Policy = "User")]
        [HttpGet("details/user")]
        public async Task<PagedList<MedicalRecordDto>> GetMedicalRecordsForUser([FromQuery] GetMedicalRecordsForUserQuery query)
        {
            return await mediator.Send(query);
        }

        [Authorize(Policy = "User")]
        [HttpGet("details")]
        public async Task<MedicalRecordEditDto> GetMedicalRecordsDetails([FromQuery] GetMedicalRecordForEditQuery query)
        {
            return await mediator.Send(query);
        }

        [Authorize(Policy = "User")]
        [HttpDelete("photo")]
        public async Task RemovePhotoFromRecord([FromQuery] DeletePhotoFromRecordCommand command)
        {
            await mediator.Send(command);
        }


        [Authorize(Policy = "Doctor")]
        [HttpGet("appointment-details/{appointmentId}")]
        public async Task<AppointmentForRecordDto> GetAppointmentDetails(Guid appointmentId)
        {
            return await mediator.Send(new GetAppointmentForRecordQuery { AppointmentId = appointmentId });
        }

        [Authorize(Policy = "Doctor")]
        [HttpPost]
        public async Task<Guid> CreateMedicalRecord(CreateMedicalRecordCommand command)
        {
            return await mediator.Send(command);
        }

        [Authorize(Policy = "Doctor")]
        [HttpPut]
        public async Task UpdateMedicalRecord(UpdateMedicalRecordCommand command)
        {
            await mediator.Send(command);
        }        

        [Authorize(Policy = "Doctor")]
        [HttpPost("add-photo/{recordId}")]
        public async Task AddPhoto(Guid recordId, IFormFile photo)
        {
            await mediator.Send(new AddPhotoToRecordCommand { RecordId = recordId, Photo = photo });
        }
    }
}
