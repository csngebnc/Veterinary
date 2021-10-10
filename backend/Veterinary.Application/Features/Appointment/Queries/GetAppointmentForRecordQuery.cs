using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.AppointmentEntities;

namespace Veterinary.Application.Features.AppointmentFeatures.Queries
{
    public class GetAppointmentForRecordQuery : IRequest<AppointmentForRecordDto>
    {
        public Guid AppointmentId { get; set; }
    }

    public class AppointmentForRecordDto
    {
        public Guid OwnerId { get; set; }
        public string OwnerEmail { get; set; }
        public Guid? AnimalId { get; set; }
        public string AnimalName { get; set; }

    }

    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentForRecordQuery, AppointmentForRecordDto>
    {
        private readonly IAppointmentRepository appointmentRepository;

        public GetAppointmentQueryHandler(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<AppointmentForRecordDto> Handle(GetAppointmentForRecordQuery request, CancellationToken cancellationToken)
        {
            var appointment = await appointmentRepository.GetAppointment(request.AppointmentId);

            return new AppointmentForRecordDto
            {
                OwnerEmail = appointment.Owner.Email,
                AnimalId = appointment.AnimalId,
                AnimalName = appointment.Animal?.Name,
                OwnerId = appointment.OwnerId
            };
        }
    }
}
