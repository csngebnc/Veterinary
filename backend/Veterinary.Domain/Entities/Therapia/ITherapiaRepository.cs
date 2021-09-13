using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.TherapiaEntities
{
    public interface ITherapiaRepository
    {
        IQueryable<Therapia> GetAllAsQueryable();
        Task<Therapia> GetByPredicateAsync(Expression<Func<Therapia, bool>> predicate, Func<IQueryable<Therapia>,
                         IIncludableQueryable<Therapia, object>> include = null);
        Task<Therapia> FindAsync(Guid id);
        Task<Therapia> InsertAsync(Therapia entity);
        Task UpdateAsync(Therapia entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);

        Task<bool> AnyByNameAsync(string name);
        Task<bool> CanBeDeleted(Guid id);
    }
}
