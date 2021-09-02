using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Veterinary.Application.Services;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;

namespace Veterinary.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext httpContext;
        private readonly VeterinaryDbContext veterinaryDbContext;

        public IdentityService(IHttpContextAccessor httpContextAccessor, VeterinaryDbContext veterinaryDbContext)
        {
            httpContext = httpContextAccessor.HttpContext;
            this.veterinaryDbContext = veterinaryDbContext;
        }        

        public Guid GetCurrentUserId()
        {
            var userId = httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            return Guid.Parse(userId);
        }
        public async Task<VeterinaryUser> GetCurrentUser()
        {
            return await veterinaryDbContext.Users.FindAsync(GetCurrentUserId());
        }
    }
}
