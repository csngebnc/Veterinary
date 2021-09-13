using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.TherapiaEntities;

namespace Veterinary.Dal.Repositories.TherapiaRepository
{
    public class TherapiaRepository : GenericRepository<Therapia>, ITherapiaRepository
    {
        public TherapiaRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            return await GetAllAsQueryable().AnyAsync(therapia => therapia.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CanBeDeleted(Guid id)
        {
            if (!(await GetAllAsQueryable().AnyAsync(therapia => therapia.Id == id)))
            {
                throw new EntityNotFoundException();
            }

            var canBeDeleted = await GetAllAsQueryable()
                .Where(x => x.Id == id)
                .Include(x => x.TherapiaRecords)
                .Select(x => x.TherapiaRecords)
                .SingleAsync();

            return canBeDeleted.Count == 0;
        }
    }
}
