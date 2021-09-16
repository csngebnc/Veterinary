using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Dal.Repositories.Vaccination
{
    public class VaccineRecordRepository : GenericRepository<VaccineRecord>, IVaccineRecordRepository
    {
        public VaccineRecordRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public IQueryable<VaccineRecord> GetVaccineRecordsByAnimalIdAsync(Guid id)
        {
            return GetAllAsQueryable()
                .Where(vr => vr.AnimalId == id)
                .Include(vr => vr.Animal)
                .Include(vr => vr.Vaccine)
                .OrderByDescending(vr => vr.Date);
        }

        public async Task<VaccineRecord> GetVaccineRecordWithDetailsAsync(Guid id)
        {
            return await GetAllAsQueryable()
                .Include(vr => vr.Animal)
                .Include(vr => vr.Vaccine)
                .SingleOrDefaultAsync(vr => vr.Id == id);
        }

    }
}
