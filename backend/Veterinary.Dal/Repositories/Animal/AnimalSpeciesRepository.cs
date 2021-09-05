using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Dal.Repositories.AnimalSpeciesRepository
{
    public class AnimalSpeciesRepository : GenericRepository<AnimalSpecies>, IAnimalSpeciesRepository
    {
        public AnimalSpeciesRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            return await GetAllAsQueryable().AnyAsync(species => species.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CanBeDeleted(Guid id)
        {
            if(!(await GetAllAsQueryable().AnyAsync(species => species.Id == id)))
            {
                throw new EntityNotFoundException();
            }

            var canBeDeleted = await GetAllAsQueryable()
                .Include(x => x.Animals)
                .Where(x => x.Id == id)
                .Select(x => x.Animals)
                .CountAsync();

            return canBeDeleted == 0;
        }
    }
}
