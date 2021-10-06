using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Shared.Enums;

namespace Veterinary.Application.Features.AppointmentFeatures.Queries
{
    public class GetUserAppointmentsQuery : IRequest<PagedList<AppointmentForUserDto>>
    {
        public Guid UserId { get; set; }
        public Guid? AnimalId { get; set; }
        public PageData PageData { get; set; }
    }

    public class AppointmentForUserDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }

        public Guid TreatmentId { get; set; }
        public string TreatmentName { get; set; }

        public Guid? AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string AnimalSpecies { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Reasons { get; set; }
        public AppointmentStatusEnum Status { get; set; }
    }

    public class GetUserAppointmentsHandler : IRequestHandler<GetUserAppointmentsQuery, PagedList<AppointmentForUserDto>>
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IIdentityService identityService;

        public GetUserAppointmentsHandler(IAppointmentRepository appointmentRepository, IIdentityService identityService)
        {
            this.appointmentRepository = appointmentRepository;
            this.identityService = identityService;
        }

        public async Task<PagedList<AppointmentForUserDto>> Handle(GetUserAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if(request.UserId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var query = appointmentRepository.GetAllAsQueryable()
                   .Where(appointment => appointment.OwnerId == request.UserId);

            if (request.AnimalId.HasValue)
            {
                query = query.Where(appointment => appointment.AnimalId != null && appointment.AnimalId == request.AnimalId.Value);
            }

            return await query
                .Include(appointment => appointment.Doctor)
                .Include(appointment => appointment.Animal)
                    .ThenInclude(animal => animal.Species)
                .Include(appointment => appointment.Treatment)
                .OrderByDescending(appointment => appointment.StartDate).Select(appointment => new AppointmentForUserDto
                {
                    Id = appointment.Id,
                    AnimalId = appointment.AnimalId == null ? null : appointment.AnimalId,
                    AnimalName = appointment.AnimalId == null ? "" : appointment.Animal.Name,
                    AnimalSpecies = appointment.AnimalId == null ? "" : appointment.Animal.Species.Name,
                    DoctorId = appointment.DoctorId,
                    DoctorName = appointment.Doctor.Name,
                    TreatmentId = appointment.TreatmentId,
                    TreatmentName = appointment.Treatment.Name,
                    StartDate = appointment.StartDate,
                    EndDate = appointment.EndDate,
                    Reasons = appointment.Reasons,
                    Status = appointment.Status
                    
                }).ToPagedListAsync(request.PageData);

        }
    }
}
