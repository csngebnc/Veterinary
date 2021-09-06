using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.Vaccination
{
    public interface IVaccineRepository
    {
        IQueryable<Vaccine> GetAllAsQueryable();
        Task<Vaccine> GetByPredicateAsync(Expression<Func<Vaccine, bool>> predicate, Func<IQueryable<Vaccine>,
                         IIncludableQueryable<Vaccine, object>> include = null);
        Task<Vaccine> FindAsync(Guid id);
        Task<Vaccine> InsertAsync(Vaccine entity);
        Task UpdateAsync(Vaccine entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);

        Task<bool> AnyByNameAsync(string name);
        Task<bool> CanBeDeleted(Guid id);
    }
}
