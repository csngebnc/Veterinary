using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Veterinary.Domain.Entities;
using Veterinary.Domain.Entities.AnimalEntities;

namespace Veterinary.Dal.Data
{
    public class VeterinaryDbContext : IdentityDbContext<VeterinaryUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalSpecies> AnimalSpecies { get; set; }
        public VeterinaryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
