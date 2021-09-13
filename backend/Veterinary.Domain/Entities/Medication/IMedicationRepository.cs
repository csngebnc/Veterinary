using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.MedicationEntities
{
    public interface IMedicationRepository
    {
        IQueryable<Medication> GetAllAsQueryable();
        Task<Medication> GetByPredicateAsync(Expression<Func<Medication, bool>> predicate, Func<IQueryable<Medication>,
                         IIncludableQueryable<Medication, object>> include = null);
        Task<Medication> FindAsync(Guid id);
        Task<Medication> InsertAsync(Medication entity);
        Task UpdateAsync(Medication entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);

        Task<bool> AnyByNameAsync(string name);
        Task<bool> CanBeDeleted(Guid id);
    }
}
