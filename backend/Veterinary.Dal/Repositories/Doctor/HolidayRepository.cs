using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;

namespace Veterinary.Dal.Repositories.Doctor
{
    public class HolidayRepository : GenericRepository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<List<Holiday>> GetDoctorHolidaysByInterval(Guid doctorId, DateTime startDate, int duration)
        {
            var endDate = startDate.AddDays(duration);
            return await Table.Where(holiday => holiday.DoctorId == doctorId &&
                                     holiday.StartDate <= startDate && holiday.EndDate >= startDate ||
                                     holiday.StartDate >= startDate && holiday.EndDate <= endDate ||
                                     holiday.StartDate <= endDate && holiday.EndDate >= endDate ||
                                     holiday.StartDate <= startDate && holiday.EndDate >= endDate
                                    ).ToListAsync();
        }
    }
}