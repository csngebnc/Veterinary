using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.AppointmentEntities;

namespace Veterinary.Dal.Repositories.AppointmentRepository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public Task<List<Appointment>> GetAppointmentsByDoctorAndDateAsync(Guid doctorId, DateTime appointmentDate)
        {
            return GetAllAsQueryable()
                    .Where(appointment => appointment.DoctorId == doctorId &&
                                          appointment.StartDate.Year == appointmentDate.Year &&
                                          appointment.StartDate.DayOfYear == appointmentDate.DayOfYear
                    ).ToListAsync();
        }
    }
}
