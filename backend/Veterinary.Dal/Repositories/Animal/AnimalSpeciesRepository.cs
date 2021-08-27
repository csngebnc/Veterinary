using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;

namespace Veterinary.Dal.Repositories.AnimalSpeciesRepository
{
    class AnimalSpeciesRepository : GenericRepository<AnimalSpecies>, IAnimalSpeciesRepository
    {
        public AnimalSpeciesRepository(VeterinaryDbContext context) : base(context)
        {
        }
    }
}
