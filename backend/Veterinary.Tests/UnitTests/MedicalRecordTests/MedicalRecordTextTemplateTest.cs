using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Features.MedicalRecordTextTemplateFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.MedicalRecordTests
{
    public class MedicalRecordTextTemplateTest : UnitTestBase
    {
        public MedicalRecordTextTemplateTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateTemplate()
        {
            // Arrange
            var command = new CreateMedicalRecordTextTemplateCommand
            {
                Name = "Sablon",
                HtmlContent = "<b>hello</b>"
            };

            var handler = new CreateMedicalRecordTextTemplateCommandHandler(mockedRepositories.MedicalRecordTextTemplateRepository, identityServiceManager);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var templates = await mockedRepositories.MedicalRecordTextTemplateRepository.GetAllAsQueryable().ToListAsync();

            Assert.NotEmpty(templates);
            Assert.Single(templates);
            Assert.Equal("Sablon", templates.First().Name);
        }

        [Fact]
        public async Task Test_CreateTemplate_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateMedicalRecordTextTemplateCommand
            {
                Name = "Sablon",
                HtmlContent = "<b>hello</b>"
            };

            var handler = new CreateMedicalRecordTextTemplateCommandHandler(mockedRepositories.MedicalRecordTextTemplateRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateTemplate()
        {
            // Arrange
            var template = await CreateTemplate_ForArrange();
            var command = new UpdateMedicalRecordTextTemplateCommand
            {
                TemplateId = template.Id,
                Name = "Új sablon",
                HtmlContent = template.HtmlContent
            };

            var handler = new UpdateMedicalRecordTextTemplateCommandHandler(mockedRepositories.MedicalRecordTextTemplateRepository, identityServiceManager);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var templates = await mockedRepositories.MedicalRecordTextTemplateRepository.GetAllAsQueryable().ToListAsync();

            Assert.NotEmpty(templates);
            Assert.Single(templates);
            Assert.Equal("Új sablon", templates.First().Name);
        }

        [Fact]
        public async Task Test_UpdateTemplate_ThrowsForbiddenException()
        {
            // Arrange
            var template = await CreateTemplate_ForArrange();
            var command = new UpdateMedicalRecordTextTemplateCommand
            {
                TemplateId = template.Id,
                Name = "Új sablon",
                HtmlContent = template.HtmlContent
            };

            var handler = new UpdateMedicalRecordTextTemplateCommandHandler(mockedRepositories.MedicalRecordTextTemplateRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteTemplate()
        {
            // Arrange
            var template = await CreateTemplate_ForArrange();
            var command = new DeleteMedicalRecordTextTemplateCommand
            {
                TemplateId = template.Id
            };

            var handler = new DeleteMedicalRecordTextTemplateCommandHandler(mockedRepositories.MedicalRecordTextTemplateRepository, identityServiceManager);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var templates = await mockedRepositories.MedicalRecordTextTemplateRepository.GetAllAsQueryable().ToListAsync();

            Assert.Empty(templates);
        }

        [Fact]
        public async Task Test_DeleteTemplate_ThrowsForbiddenException()
        {
            // Arrange
            var template = await CreateTemplate_ForArrange();
            var command = new DeleteMedicalRecordTextTemplateCommand
            {
                TemplateId = template.Id
            };

            var handler = new DeleteMedicalRecordTextTemplateCommandHandler(mockedRepositories.MedicalRecordTextTemplateRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        private async Task<MedicalRecordTextTemplate> CreateTemplate_ForArrange()
        {
            var template = new MedicalRecordTextTemplate
            {
                Name = "Sablon",
                HtmlContent = "<b>hello</b>"
            };

            await mockedRepositories.MedicalRecordTextTemplateRepository.InsertAsync(template);

            return template;
        }
    }
}
