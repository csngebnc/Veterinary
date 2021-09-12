using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities;
using Veterinary.Shared.Enums;
using Veterinary.Shared.Extensions;

namespace Veterinary.Application.Features.Manager.Commands
{
    public class ChangeUserRoleCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
    }

    public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, Unit>
    {
        private readonly IVeterinaryUserRepository veterinaryUserRepository;
        private readonly UserManager<VeterinaryUser> userManager;
        private readonly IIdentityService identityService;

        public ChangeUserRoleCommandHandler(
            IVeterinaryUserRepository veterinaryUserRepository, 
            UserManager<VeterinaryUser> userManager, 
            IIdentityService identityService)
        {
            this.veterinaryUserRepository = veterinaryUserRepository;
            this.userManager = userManager;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            if(request.UserId == identityService.GetCurrentUserId())
            {
                throw new MethodNotAllowedException("Saját magad szerepkörét nem változtathatod meg.");
            }

            var user = await veterinaryUserRepository.FindAsync(request.UserId);
            var userRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);
            await userManager.AddToRoleAsync(user, request.RoleName);

            return Unit.Value;
        }
    }

    public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
    {
        public ChangeUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull()
                .WithMessage("A felhasználó azonosítójának megadása kötelező.");
            RuleFor(x => x.RoleName).NotEmpty()
                .WithMessage("Az új szerepkör nem lehet üres.")
                .Must((roleName) => EnumExtensions.GetValues<RoleEnum>().Contains(roleName))
                .WithMessage("Nem létezik ilyen szerepkör.");
        }
    }
}
