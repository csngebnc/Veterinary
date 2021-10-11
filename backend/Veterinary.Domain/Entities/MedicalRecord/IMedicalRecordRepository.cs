using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.MedicalRecordEntities
{
    public interface IMedicalRecordRepository
    {
        IQueryable<MedicalRecord> GetAllAsQueryable();
        Task<MedicalRecord> GetByPredicateAsync(Expression<Func<MedicalRecord, bool>> predicate, Func<IQueryable<MedicalRecord>,
                         IIncludableQueryable<MedicalRecord, object>> include = null);
        Task<MedicalRecord> FindAsync(Guid id);
        Task<MedicalRecord> InsertAsync(MedicalRecord entity);
        Task UpdateAsync(MedicalRecord entity);
        Task DeleteAsync(Guid id);
        IQueryable<MedicalRecord> GetMedicalRecordsByUserIdQueryable(Guid userId);
        IQueryable<MedicalRecord> GetMedicalRecordsByAnimalIdQueryable(Guid animalId);
        Task<MedicalRecord> GetMedicalRecordWithDetailsAsync(Guid recordId);
        Task<MedicalRecord> GetMedicalRecordForPDFAsync(Guid recordId);
    }
}
