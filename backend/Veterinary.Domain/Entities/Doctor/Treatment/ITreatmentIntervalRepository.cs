using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.Doctor.TreatmentEntities
{
    public interface ITreatmentIntervalRepository
    {
        IQueryable<TreatmentInterval> GetAllAsQueryable();
        Task<TreatmentInterval> GetByPredicateAsync(Expression<Func<TreatmentInterval, bool>> predicate, Func<IQueryable<TreatmentInterval>,
                         IIncludableQueryable<TreatmentInterval, object>> include = null);
        Task<TreatmentInterval> FindAsync(Guid id);
        Task<TreatmentInterval> InsertAsync(TreatmentInterval entity);
        Task UpdateAsync(TreatmentInterval entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);

        IQueryable<TreatmentInterval> GetTreatmentIntervalsByTreatmentIdAsQueryable(Guid treatmentId);
    }
}
