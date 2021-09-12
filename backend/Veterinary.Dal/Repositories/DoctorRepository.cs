using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;

namespace Veterinary.Dal.Repositories
{
    public class DoctorRepository : GenericRepository<IdentityRole<Guid>>, IDoctorRepository
    {

        public DoctorRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<List<(VeterinaryUser User, string RoleName)>> GetDoctors()
        {
            return await Table
                .Where(r => r.Name.Equals("ManagerDoctor") || r.Name.Equals("NormalDoctor"))
                .Join(context.UserRoles, r => r.Id, ur => ur.RoleId, (Roles, UserRoles) => new { UserRoles.UserId, Roles.Name })
                .Join(context.Users, UserRole => UserRole.UserId, User => User.Id, (Role, User) => new { Role.Name, User })
                .Select(r => ValueTuple.Create(r.User, r.Name))
                .ToListAsync();

        }
    }
}
