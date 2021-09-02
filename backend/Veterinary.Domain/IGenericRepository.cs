using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllAsQueryable();
        Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, 
                         IIncludableQueryable<T, object>> include = null);
        Task<T> FindAsync(Guid id);
        Task<T> InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
