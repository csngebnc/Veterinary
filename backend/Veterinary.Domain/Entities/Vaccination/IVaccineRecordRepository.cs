using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.Vaccination
{
    public interface IVaccineRecordRepository
    {
        IQueryable<VaccineRecord> GetAllAsQueryable();
        Task<VaccineRecord> GetByPredicateAsync(Expression<Func<VaccineRecord, bool>> predicate, Func<IQueryable<VaccineRecord>,
                         IIncludableQueryable<VaccineRecord, object>> include = null);
        Task<VaccineRecord> FindAsync(Guid id);
        Task<VaccineRecord> InsertAsync(VaccineRecord entity);
        Task UpdateAsync(VaccineRecord entity);
        Task DeleteAsync(Guid id);

        IQueryable<VaccineRecord> GetVaccineRecordsByAnimalIdAsync(Guid id);
        Task<VaccineRecord> GetVaccineRecordWithDetailsAsync(Guid id);

    }
}
