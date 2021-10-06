using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.Doctor.HolidayEntities
{
    public interface IHolidayRepository
    {
        IQueryable<Holiday> GetAllAsQueryable();
        Task<Holiday> GetByPredicateAsync(Expression<Func<Holiday, bool>> predicate, Func<IQueryable<Holiday>,
                         IIncludableQueryable<Holiday, object>> include = null);
        Task<Holiday> FindAsync(Guid id);
        Task<Holiday> InsertAsync(Holiday entity);
        Task UpdateAsync(Holiday entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
        Task<List<Holiday>> GetDoctorHolidaysByInterval(Guid doctorId, DateTime dateTime, int duration);
    }
}
