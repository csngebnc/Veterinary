using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.Doctor.HolidayFeatures.Commands
{
    public class DeleteHolidayCommand : IRequest
    {
        public Guid HolidayId { get; set; }
    }

    public class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, Unit>
    {
        private readonly IHolidayRepository holidayRepository;
        private readonly IIdentityService identityService;

        public DeleteHolidayCommandHandler(IHolidayRepository holidayRepository, IIdentityService identityService)
        {
            this.holidayRepository = holidayRepository;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            var holiday = await holidayRepository.FindAsync(request.HolidayId);

            if(holiday.DoctorId != identityService.GetCurrentUserId() && !await identityService.IsInRoleAsync(RoleEnum.ManagerDoctor.Value()))
            {
                throw new ForbiddenException();
            }

            await holidayRepository.DeleteAsync(request.HolidayId);

            return Unit.Value;
        }
    }
}
