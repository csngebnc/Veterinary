using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities.AppointmentEntities
{
    public interface IAppointmentRepository
    {
        IQueryable<Appointment> GetAllAsQueryable();
        Task<Appointment> GetByPredicateAsync(Expression<Func<Appointment, bool>> predicate, Func<IQueryable<Appointment>,
                         IIncludableQueryable<Appointment, object>> include = null);
        Task<Appointment> FindAsync(Guid id);
        Task<Appointment> InsertAsync(Appointment entity);
        Task UpdateAsync(Appointment entity);
        Task DeleteAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
        Task<List<Appointment>> GetAppointmentsByDoctorAndDateAsync(Guid doctorId, DateTime appointmentDate);
    }
}
