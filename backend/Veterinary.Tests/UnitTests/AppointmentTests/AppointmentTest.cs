using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Features.AppointmentFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AppointmentEntities;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Shared.Enums;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.AppointmentTests
{
    public class AppointmentTest : UnitTestBase
    {
        public AppointmentTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateAppointment_NotAllowedForOtherUser()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                OwnerId = identityServiceManager.GetCurrentUserId(),
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0).AddMinutes(treatment.Duration),
                Reasons = ""
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceUser,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_CreateAppointment_NotExistingUser()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                OwnerId = Guid.NewGuid(),
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0).AddMinutes(treatment.Duration),
                Reasons = ""
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task Test_CreateAppointment_AnimalNotExists()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                OwnerId = identityServiceManager.GetCurrentUserId(),
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0).AddMinutes(treatment.Duration),
                Reasons = "",
                AnimalId = Guid.NewGuid()
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task Test_CreateAppointment_BadOwner()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                OwnerId = identityServiceUser.GetCurrentUserId(),
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0).AddMinutes(treatment.Duration),
                Reasons = "",
                AnimalId = animal.Id
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_CreateAppointment_BadDoctor()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                OwnerId = identityServiceUser.GetCurrentUserId(),
                DoctorId = identityServiceDoctor.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, interval.StartHour, interval.StartMin, 0).AddMinutes(treatment.Duration),
                Reasons = "",
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }
        /*
        // Only works if time is atleast 120 mins before midnight.
        [Fact]
        public async Task Test_CreateAppointment_BadInterval()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, 20, 30, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, 21, 01, 0),
                Reasons = "",
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }
        */
        [Fact]
        public async Task Test_CreateAppointment_StartIsInPast()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, 20, 30, 0).AddDays(-7),
                EndDate = new DateTime(today.Year, today.Month, today.Day, 21, 00, 0).AddDays(-7),
                Reasons = "",
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<Exception>(action);
        }
        /*
        [Fact]
        public async Task Test_CreateAppointment_AlreadyInUse()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);
            var appointment = await CreateAppointment_ForArrange(treatment, identityServiceManager.GetCurrentUserId());

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, 20, 30, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, 21, 00, 0),
                Reasons = "",
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<Exception>(action);
        }
        */
        [Fact]
        public async Task Test_CreateAppointment_DoctorOnHoliday()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);
            var holiday = await CreateHoliday_ForArrange(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, 20, 30, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, 21, 00, 0),
                Reasons = "",
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<Exception>(action);
        }

        [Fact]
        public async Task Test_CreateAppointment()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);

            var today = DateTime.Today;

            var command = new CreateAppointmentCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, 20, 30, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, 21, 00, 0),
                Reasons = "",
            };

            var handler = new CreateAppointmentCommandHandler(
                identityServiceManager,
                mockedRepositories.VeterinaryUserRepository,
                mockedRepositories.AnimalRepository,
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                mockedRepositories.HolidayRepository,
                mockedRepositories.AppointmentRepository);

            await handler.Handle(command, CancellationToken.None);

            var appointmentsForUser = await mockedRepositories.AppointmentRepository
                .GetAllAsQueryable()
                .Where(appointment => appointment.OwnerId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.NotEmpty(appointmentsForUser);
            Assert.Single(appointmentsForUser);
        }

        [Fact]
        public async Task Test_UpdateAppointmentStatus()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);
            var appointment = await CreateAppointment_ForArrange(treatment, identityServiceManager.GetCurrentUserId());

            var command = new UpdateAppointmentStatusCommand
            {
                AppointmentId = appointment.Id,
                StatusId = (int)
                AppointmentStatusEnum.Closed
            };
            var handler = new UpdateAppointmentStatusCommandHandler(mockedRepositories.AppointmentRepository, identityServiceManager);

            await handler.Handle(command, CancellationToken.None);

            var appointmentsForUser = await mockedRepositories.AppointmentRepository
                .GetAllAsQueryable()
                .Where(appointment => appointment.OwnerId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.Single(appointmentsForUser);
            Assert.Equal(AppointmentStatusEnum.Closed, appointmentsForUser.First().Status);
        }

        [Fact]
        public async Task Test_UpdateAppointmentStatus_ThrowsForbiddenException()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);
            var appointment = await CreateAppointment_ForArrange(treatment, identityServiceManager.GetCurrentUserId());

            var command = new UpdateAppointmentStatusCommand
            {
                AppointmentId = appointment.Id,
                StatusId = (int)
                AppointmentStatusEnum.Closed
            };
            var handler = new UpdateAppointmentStatusCommandHandler(mockedRepositories.AppointmentRepository, identityServiceUser);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateAppointmentStatus_Resign()
        {
            var species = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(species.Id);
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, 0, 23);
            var appointment = await CreateAppointment_ForArrange(treatment, identityServiceUser.GetCurrentUserId());

            var command = new UpdateAppointmentStatusCommand
            {
                AppointmentId = appointment.Id,
                StatusId = (int)
                AppointmentStatusEnum.Resigned
            };
            var handler = new UpdateAppointmentStatusCommandHandler(mockedRepositories.AppointmentRepository, identityServiceUser);

            await handler.Handle(command, CancellationToken.None);

            Assert.Equal(AppointmentStatusEnum.Resigned, appointment.Status);
        }

        private async Task<Appointment> CreateAppointment_ForArrange(Treatment treatment, Guid ownerId)
        {
            var today = DateTime.Today;

            var appointment = new Appointment
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                TreatmentId = treatment.Id,
                StartDate = new DateTime(today.Year, today.Month, today.Day, 22, 30, 0),
                EndDate = new DateTime(today.Year, today.Month, today.Day, 23, 00, 0),
                Reasons = "",
                OwnerId = ownerId
            };

            await mockedRepositories.AppointmentRepository.InsertAsync(appointment);

            return appointment;
        }

        private async Task<AnimalSpecies> CreateAnimalSpecies_ForArrange()
        {
            var bunny = new AnimalSpecies
            {
                Id = Guid.NewGuid(),
                Name = "nyúl"
            };
            await mockedRepositories.AnimalSpeciesRepository.InsertAsync(bunny);

            return bunny;
        }

        private async Task<Animal> CreateAnimal_ForArrange(Guid speciesId, string ownerId = "85b4a2a3-aa22-40e4-9b91-c72e96ab8e07")
        {
            var animal = new Animal
            {
                OwnerId = Guid.Parse(ownerId),
                Name = "Madzag",
                DateOfBirth = new DateTime(2018, 4, 23),
                Sex = "hím",
                SpeciesId = speciesId
            };
            await mockedRepositories.AnimalRepository.InsertAsync(animal);

            return animal;
        }

        private async Task<Holiday> CreateHoliday_ForArrange(DateTime start, DateTime end)
        {
            var holiday = new Holiday
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                StartDate = start,
                EndDate = end
            };
            await mockedRepositories.HolidayRepository.InsertAsync(holiday);

            return holiday;
        }

        private async Task<Treatment> CreateTreatment_ForArrange(int duration = 30, bool isInactive = false)
        {
            var treatment = new Treatment
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Name = "Kezelés",
                Duration = duration,
                IsInactive = isInactive
            };

            await mockedRepositories.TreatmentRepository.InsertAsync(treatment);

            return treatment;
        }

        private async Task<TreatmentInterval> CreateTreatmentInterval_ForArrange(Guid treatmentId, int startHour = 10, int endHour = 18, bool isInactive = false)
        {
            var interval = new TreatmentInterval
            {
                TreatmentId = treatmentId,
                DayOfWeek = (int)DateTime.Today.DayOfWeek,
                StartHour = startHour,
                StartMin = 0,
                EndHour = endHour,
                EndMin = 0,
                IsInactive = isInactive
            };

            await mockedRepositories.TreatmentIntervalRepository.InsertAsync(interval);

            return interval;
        }
    }
}
