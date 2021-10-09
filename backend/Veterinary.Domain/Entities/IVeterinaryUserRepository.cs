using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities
{
    public interface IVeterinaryUserRepository
    {
        IQueryable<VeterinaryUser> GetAllAsQueryable();
        Task<VeterinaryUser> GetByPredicateAsync(Expression<Func<VeterinaryUser, bool>> predicate, Func<IQueryable<VeterinaryUser>,
                         IIncludableQueryable<VeterinaryUser, object>> include = null);
        Task<VeterinaryUser> FindAsync(Guid id);
        Task<VeterinaryUser> InsertAsync(VeterinaryUser entity);
        Task UpdateAsync(VeterinaryUser entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);

        IQueryable<VeterinaryUser> SearchQueryable(string param);
    }
}
