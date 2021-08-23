using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Veterinary.Model.Entities;

namespace Veterinary.Dal.Data
{
    public class VeterinaryDbContext : IdentityDbContext<VeterinaryUser, IdentityRole<Guid>, Guid>
    {
        public VeterinaryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
