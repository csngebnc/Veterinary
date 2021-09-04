using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Veterinary.Domain.Entities.AnimalEntities;

namespace Veterinary.Domain.Entities.AnimalSpeciesRepository
{
    public interface IAnimalSpeciesRepository
    {
        IQueryable<AnimalSpecies> GetAllAsQueryable();
        Task<AnimalSpecies> GetByPredicateAsync(Expression<Func<AnimalSpecies, bool>> predicate, Func<IQueryable<AnimalSpecies>,
                         IIncludableQueryable<AnimalSpecies, object>> include = null);
        Task<AnimalSpecies> FindAsync(Guid id);
        Task<AnimalSpecies> InsertAsync(AnimalSpecies entity);
        Task UpdateAsync(AnimalSpecies entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
    }
}
