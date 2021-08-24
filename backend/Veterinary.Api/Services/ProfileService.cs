using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<VeterinaryUser> userManager;

        public ProfileService(UserManager<VeterinaryUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);

            IList<string> roles = await userManager.GetRolesAsync(user);

            IList<Claim> roleClaims = new List<Claim>();
            foreach (string role in roles)
            {
                roleClaims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.IssuedClaims.AddRange(roleClaims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
