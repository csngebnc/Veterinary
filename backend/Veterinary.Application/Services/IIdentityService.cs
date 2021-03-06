using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Domain.Entities;

namespace Veterinary.Application.Services
{
    public interface IIdentityService
    {
        Guid GetCurrentUserId();
        Task<VeterinaryUser> GetCurrentUser();
        Task<bool> IsInRoleAsync(string role);
        Task<bool> IsInRoleAsync(Guid userId, string role);
    }
}
