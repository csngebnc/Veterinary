using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Extensions;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Shared.Enums;

namespace Veterinary.Application.Features.AppointmentFeatures.Commands
{
    public class CreateAppointmentCommand : IRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid TreatmentId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? AnimalId { get; set; }
        public string Reasons { get; set; }

    }

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Unit>
    {
        private readonly IIdentityService identityService;
        private readonly IVeterinaryUserRepository veterinaryUserRepository;
        private readonly IAnimalRepository animalRepository;
        private readonly ITreatmentRepository treatmentRepository;
        private readonly ITreatmentIntervalRepository treatmentIntervalRepository;
        private readonly IHolidayRepository holidayRepository;
        private readonly IAppointmentRepository appointmentRepository;

        public CreateAppointmentCommandHandler(
            IIdentityService identityService,
            IVeterinaryUserRepository veterinaryUserRepository,
            IAnimalRepository animalRepository,
            ITreatmentRepository treatmentRepository,
            ITreatmentIntervalRepository treatmentIntervalRepository,
            IHolidayRepository holidayRepository,
            IAppointmentRepository appointmentRepository)
        {
            this.identityService = identityService;
            this.veterinaryUserRepository = veterinaryUserRepository;
            this.animalRepository = animalRepository;
            this.treatmentRepository = treatmentRepository;
            this.treatmentIntervalRepository = treatmentIntervalRepository;
            this.holidayRepository = holidayRepository;
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<Unit> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var ownerId = request.OwnerId ?? identityService.GetCurrentUserId();
            if(ownerId != identityService.GetCurrentUserId() && await identityService.IsInRoleAsync("User"))
            {
                throw new ForbiddenException();
            }

            #region UserValidation
            if (request.OwnerId != null && !await veterinaryUserRepository.AnyByIdAsync(ownerId))
            {
                throw new EntityNotFoundException();
            }

            if (await identityService.IsInRoleAsync(request.DoctorId, "User"))
            {
                throw new EntityNotFoundException();
            }

            if (request.AnimalId != null)
            {
                if (!await animalRepository.AnyByIdAsync(request.AnimalId.Value))
                {
                    throw new EntityNotFoundException();
                }
                else
                {
                    var animal = await animalRepository.FindAsync(request.AnimalId.Value);
                    if (animal.OwnerId != ownerId)
                    {
                        throw new ForbiddenException();
                    }
                }
            }
            #endregion

            #region AppointmentValidation
            if (!await treatmentRepository.AnyByIdAsync(request.TreatmentId))
            {
                throw new EntityNotFoundException();
            }

            var treatment = await treatmentRepository.FindAsync(request.TreatmentId);
            if (treatment.DoctorId != request.DoctorId)
            {
                throw new ForbiddenException();
            }

            var startDate = request.StartDate.ToLocalTime();

            var endDate = request.EndDate.ToLocalTime();

            var treatmentIntervals = await treatmentIntervalRepository
                .GetAllAsQueryable()
                    .Where(treatmentInterval => treatmentInterval.TreatmentId == request.TreatmentId).ToListAsync();

            var treatmentInterval = treatmentIntervals.Where(interval =>
                interval.DayOfWeek == ((int)startDate.Date.DayOfWeek) &&
                startDate.GetMinutes() >= interval.GetStartInMinutes() &&
                           endDate.GetMinutes() <= interval.GetEndInMinutes()).FirstOrDefault();


            if (treatmentInterval == null)
            {
                throw new EntityNotFoundException();
            }

            if (startDate < DateTime.Now)
            {
                throw new Exception(""); // TODO: custom 400
            }

            if ((endDate - startDate).Minutes != treatment.Duration)
            {
                throw new Exception(""); // TODO: custom 400
            }

            var reservedTimes = await appointmentRepository.GetAppointmentsByDoctorAndDateAsync(request.DoctorId, startDate);

            var intersect = reservedTimes.Where(reservedTime => reservedTime.StartDate < startDate  && reservedTime.EndDate > startDate ||
                                                reservedTime.StartDate >= startDate && reservedTime.EndDate < endDate ||
                                                reservedTime.StartDate < endDate && reservedTime.EndDate > endDate ||
                                                reservedTime.StartDate <= startDate && reservedTime.EndDate >= endDate
                                                ).ToList();          

            if (intersect.Count > 0)
            {
                throw new Exception(""); // TODO: custom 400
            }

            var holidays = await holidayRepository.GetDoctorHolidaysByInterval(request.DoctorId, startDate, 0);
            if (holidays.Count > 0)
            {
                throw new Exception(""); // TODO: custom 400
            }

            #endregion

            var appointment = new Appointment
            {
                StartDate = startDate,
                EndDate = endDate,
                TreatmentId = request.TreatmentId,
                DoctorId = request.DoctorId,
                OwnerId = ownerId,
                AnimalId = request?.AnimalId,
                Reasons = request?.Reasons,
                Status = (AppointmentStatusEnum)0
            };

            await appointmentRepository.InsertAsync(appointment);

            return Unit.Value;
        }
    }

    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.StartDate.ToLocalTime()).NotEmpty()
                .WithMessage("Érvénytelen kezdő időpont.")
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("Érvénytelen kezdő időpont. Nem lehet múltbéli dátum.");
            RuleFor(x => x.EndDate).NotEmpty()
                .WithMessage("Érvénytelen záró időpont")
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("Érvénytelen záró időpont. Nem lehet korábbi, mint a kezdő dátum.");
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.TreatmentId).NotEmpty()
                .WithMessage("Kezelés választása kötelező.");            
            RuleFor(x => x.DoctorId).NotEmpty()
                .WithMessage("Orvos választása kötelező.");
        }


    }
}
