using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Abstractions;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AppointmentEntities;

namespace Veterinary.Application.Features.AppointmentFeatures.Queries
{
    public class GetDoctorsAppointmentsQuery : IRequest<PagedList<AppointmentForDoctorDto>>
    {
        public Guid DoctorId { get; set; }
        public bool FromToday { get; set; }
        public PageData PageData { get; set; }
    }

    public class GetDoctorsAppointmentsQueryHandler : IRequestHandler<GetDoctorsAppointmentsQuery, PagedList<AppointmentForDoctorDto>>
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IIdentityService identityService;

        public GetDoctorsAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IIdentityService identityService)
        {
            this.appointmentRepository = appointmentRepository;
            this.identityService = identityService;
        }

        public async Task<PagedList<AppointmentForDoctorDto>> Handle(GetDoctorsAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if(await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            var query = appointmentRepository.GetAllAsQueryable()
                .Where(appointment => appointment.DoctorId == request.DoctorId);

            if (request.FromToday)
            {
                query = query
                    .Where(appointment => appointment.StartDate >= DateTime.Now.AddMinutes(-15))
                    .OrderBy(appointment => appointment.StartDate);
            }
            else
            {
                query = query.OrderByDescending(appointment => appointment.StartDate);
            }

            return await query
                .Include(appointment => appointment.Owner)
                .Include(appointment => appointment.Animal)
                    .ThenInclude(animal => animal.Species)
                .Include(appointment => appointment.Treatment)
                .Select(appointment => new AppointmentForDoctorDto
                {
                    Id = appointment.Id,
                    AnimalId = appointment.AnimalId == null ? null : appointment.AnimalId,
                    AnimalName = appointment.AnimalId == null ? "" : appointment.Animal.Name,
                    AnimalSpecies = appointment.AnimalId == null ? "" : appointment.Animal.Species.Name,
                    UserId = appointment.OwnerId,
                    UserName = appointment.Owner.Name,
                    TreatmentId = appointment.TreatmentId,
                    TreatmentName = appointment.Treatment.Name,
                    StartDate = appointment.StartDate,
                    EndDate = appointment.EndDate,
                    Reasons = appointment.Reasons,
                    Status = appointment.Status
                })
                .ToPagedListAsync(request.PageData);

        }
    }

}
