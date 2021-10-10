using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
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

        public async Task<Appointment> GetAppointment(Guid id)
        {
            var appointment = await GetAllAsQueryable()
                .Include(appointment => appointment.Owner)
                .Include(appointment => appointment.Animal)
                .SingleOrDefaultAsync(appointment => appointment.Id == id);

            return appointment ?? throw new EntityNotFoundException();
        }
    }
}
