using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AnimalRepository;

namespace Veterinary.Dal.Repositories.AnimalRepository
{
    public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
    {
        public AnimalRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<Animal> GetAnimalWithSpeciesAsync(Guid animalId)
        {
            return await Table
                    .Include(animal => animal.Species)
                    .SingleOrDefaultAsync(animal => animal.Id == animalId);
        }
    }
}
