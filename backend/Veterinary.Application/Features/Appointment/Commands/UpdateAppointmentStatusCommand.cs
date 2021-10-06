using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.AppointmentFeatures.Commands
{
    public class UpdateAppointmentStatusCommand : IRequest
    {
        public Guid AppointmentId { get; set; }
        public int StatusId { get; set; }
    }

    public class UpdateAppointmentStatusCommandHandler : IRequestHandler<UpdateAppointmentStatusCommand, Unit>
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IIdentityService identityService;

        public UpdateAppointmentStatusCommandHandler(IAppointmentRepository appointmentRepository, IIdentityService identityService)
        {
            this.appointmentRepository = appointmentRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            var appointment = await appointmentRepository.FindAsync(request.AppointmentId);
            if (appointment.OwnerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            if (appointment.OwnerId == identityService.GetCurrentUserId() &&
                await identityService.IsInRoleAsync("User"))
            {
                if (request.StatusId != (int)AppointmentStatusEnum.Resigned ||
                    appointment.Status != AppointmentStatusEnum.New)
                {
                    throw new ForbiddenException();
                }
            }

            appointment.Status = (AppointmentStatusEnum)request.StatusId;

            await appointmentRepository.UpdateAsync(appointment);

            return Unit.Value;
        }
    }

    public class UpdateAppointmentStatusCommandValidator : AbstractValidator<UpdateAppointmentStatusCommand>
    {
        public UpdateAppointmentStatusCommandValidator()
        {
            var statusCount = EnumExtensions.GetValues<AppointmentStatusEnum>().Count - 1;
            RuleFor(x => x.AppointmentId).NotEmpty()
                .WithMessage("Az időpont azonosítójának megadása kötelező.");
            RuleFor(x => x.StatusId).InclusiveBetween(0, statusCount)
                .WithMessage($"A változtatás okának megadása kötelező. (0-{statusCount}");
        }
    }
}
