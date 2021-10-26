using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Features.MedicalRecordFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.MedicalRecordTests
{
    public class MedicalRecordTest : UnitTestBase
    {
        public MedicalRecordTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateMedicalRecord()
        {
            var command = new CreateMedicalRecordCommand
            {
                Data = new CreateMedicalRecordCommandData
                {
                    Date = new DateTime(),
                    OwnerId = identityServiceManager.GetCurrentUserId(),
                    OwnerEmail = "manager@veterinary.hu",
                    HtmlContent = "valami"
                }
            };

            var handler = new CreateMedicalRecordCommandHandler(mockedRepositories.MedicalRecordRepository, identityServiceManager);

            await handler.Handle(command, CancellationToken.None);

            var records = await mockedRepositories.MedicalRecordRepository
                .GetAllAsQueryable()
                .ToListAsync();

            Assert.NotEmpty(records);
            Assert.Single(records);
        }

        [Fact]
        public async Task Test_CreateMedicalRecord_ThrowsForbiddenException()
        {
            var command = new CreateMedicalRecordCommand
            {
                Data = new CreateMedicalRecordCommandData
                {
                    Date = new DateTime(),
                    OwnerEmail = "email@email.hu"
                }
            };

            var handler = new CreateMedicalRecordCommandHandler(mockedRepositories.MedicalRecordRepository, identityServiceUser);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateMedicalRecord()
        {
            var medicalRecord = new MedicalRecord
                {
                    Date = new DateTime(),
                    DoctorId = identityServiceManager.GetCurrentUserId(),
                    OwnerId = identityServiceManager.GetCurrentUserId(),
                    OwnerEmail = "manager@veterinary.hu",
                    HtmlContent = "valami"
            };

            await mockedRepositories.MedicalRecordRepository.InsertAsync(medicalRecord);

            var command = new UpdateMedicalRecordCommand
            {
                MedicalRecordId = medicalRecord.Id,
                Data = new UpdateMedicalRecordCommandData
                {
                    Date = new DateTime(),
                    OwnerId = identityServiceManager.GetCurrentUserId(),
                    OwnerEmail = "manager@veterinary.hu",
                    HtmlContent = "valami új"
                }
            };

            var handler = new UpdateMedicalRecordCommandHandler(mockedRepositories.MedicalRecordRepository, identityServiceManager);

            await handler.Handle(command, CancellationToken.None);

            var records = await mockedRepositories.MedicalRecordRepository
                .GetAllAsQueryable()
                .ToListAsync();

            Assert.NotEmpty(records);
            Assert.Single(records);
            Assert.Equal("valami új", medicalRecord.HtmlContent);
        }

        [Fact]
        public async Task Test_UpdateMedicalRecord_ThrowsForbiddenException()
        {
            var medicalRecord = new MedicalRecord
            {
                Date = new DateTime(),
                DoctorId = identityServiceManager.GetCurrentUserId(),
                OwnerId = identityServiceManager.GetCurrentUserId(),
                OwnerEmail = "manager@veterinary.hu",
                HtmlContent = "valami"
            };

            await mockedRepositories.MedicalRecordRepository.InsertAsync(medicalRecord);

            var command = new UpdateMedicalRecordCommand
            {
                MedicalRecordId = medicalRecord.Id,
                Data = new UpdateMedicalRecordCommandData
                {
                    Date = new DateTime(),
                    OwnerId = identityServiceManager.GetCurrentUserId(),
                    OwnerEmail = "manager@veterinary.hu",
                    HtmlContent = "valami új"
                }
            };

            var handler = new UpdateMedicalRecordCommandHandler(mockedRepositories.MedicalRecordRepository, identityServiceUser);

            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

    }
}
