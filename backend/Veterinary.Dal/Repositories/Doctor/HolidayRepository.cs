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
    }
}