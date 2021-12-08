using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Tests.IntegrationTests.Envinroment.Authentication
{
    public class IntegrationTestAuthenticationMiddleware : AuthenticationHandler<IntegrationTestAuthenticationOptions>
    {
        private readonly UserManager<VeterinaryUser> userManager;

        public IntegrationTestAuthenticationMiddleware(
            IOptionsMonitor<IntegrationTestAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserManager<VeterinaryUser> userManager) : base(options, logger, encoder, clock)
        {
            this.userManager = userManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeaderValue = Context.Request.Headers["Authorization"].ToString();
            var user = await userManager.FindByNameAsync(authorizationHeaderValue); 

            var role = user.UserName switch
            {
                "manager" => "ManagerDoctor",
                "doctor" => "NormalDoctor",
                _ => "User"
            };

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim("scope", "api-openid"),
                    new Claim(JwtClaimTypes.Role, role)
                }, "Bearer", JwtClaimTypes.Name, JwtClaimTypes.Role));

            return AuthenticateResult.Success(
                    new AuthenticationTicket(claimsPrincipal, "Bearer"));
        }

    }
}
