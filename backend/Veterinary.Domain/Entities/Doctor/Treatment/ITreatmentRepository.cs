using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.Doctor.TreatmentEntities
{
    public interface ITreatmentRepository
    {
        IQueryable<Treatment> GetAllAsQueryable();
        Task<Treatment> GetByPredicateAsync(Expression<Func<Treatment, bool>> predicate, Func<IQueryable<Treatment>,
                         IIncludableQueryable<Treatment, object>> include = null);
        Task<Treatment> FindAsync(Guid id);
        Task<Treatment> InsertAsync(Treatment entity);
        Task UpdateAsync(Treatment entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
        Task<List<Treatment>> GetTreatmentsByDoctorId(Guid doctorId, bool getAll);
        Task<bool> CanBeDeleted(Guid id);
    }
}
