using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicationEntities;

namespace Veterinary.Dal.Repositories.MedicationRepository
{
    public class MedicationRepository : GenericRepository<Medication>, IMedicationRepository
    {
        public MedicationRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            return await GetAllAsQueryable().AnyAsync(medication => medication.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CanBeDeleted(Guid id)
        {
            if (!(await GetAllAsQueryable().AnyAsync(medication => medication.Id == id)))
            {
                throw new EntityNotFoundException();
            }

            var canBeDeleted = await GetAllAsQueryable()
                .Where(x => x.Id == id)
                .Include(x => x.MedicineRecords)
                .Select(x => x.MedicineRecords)
                .SingleAsync();

            return canBeDeleted.Count == 0;
        }
    }
}
