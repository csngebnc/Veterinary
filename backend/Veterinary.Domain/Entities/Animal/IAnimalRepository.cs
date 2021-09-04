using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.AnimalEntities;

namespace Veterinary.Domain.Entities.AnimalRepository
{
    public interface IAnimalRepository
    {
        IQueryable<Animal> GetAllAsQueryable();
        Task<Animal> GetByPredicateAsync(Expression<Func<Animal, bool>> predicate, Func<IQueryable<Animal>,
                         IIncludableQueryable<Animal, object>> include = null);
        Task<Animal> FindAsync(Guid id);
        Task<Animal> InsertAsync(Animal entity);
        Task UpdateAsync(Animal entity);
        Task DeleteAsync(Guid id);

        Task<Animal> GetAnimalWithSpeciesAsync(Guid animalId);
        Task<bool> AnyByIdAsync(Guid id);
    }
}
