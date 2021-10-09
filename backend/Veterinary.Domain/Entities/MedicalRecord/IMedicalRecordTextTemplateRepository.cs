using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.MedicalRecordEntities
{
    public interface IMedicalRecordTextTemplateRepository
    {
        IQueryable<MedicalRecordTextTemplate> GetAllAsQueryable();
        Task<MedicalRecordTextTemplate> GetByPredicateAsync(Expression<Func<MedicalRecordTextTemplate, bool>> predicate, Func<IQueryable<MedicalRecordTextTemplate>,
                         IIncludableQueryable<MedicalRecordTextTemplate, object>> include = null);
        Task<MedicalRecordTextTemplate> FindAsync(Guid id);
        Task<MedicalRecordTextTemplate> InsertAsync(MedicalRecordTextTemplate entity);
        Task UpdateAsync(MedicalRecordTextTemplate entity);
        Task DeleteAsync(Guid id);
    }
}
