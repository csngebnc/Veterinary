using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Vaccination;

namespace Veterinary.Dal.Repositories.Vaccination
{
    public class VaccineRepository : GenericRepository<Vaccine>, IVaccineRepository
    {
        public VaccineRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            return await GetAllAsQueryable().AnyAsync(vaccine => vaccine.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CanBeDeleted(Guid id)
        {
            if (!(await GetAllAsQueryable().AnyAsync(vaccine => vaccine.Id == id)))
            {
                throw new EntityNotFoundException();
            }

            var canBeDeleted = await GetAllAsQueryable()
                .Where(vaccine => vaccine.Id == id)
                .Include(x => x.VaccinationRecords)
                .Select(x => x.VaccinationRecords)
                .SingleAsync();

            return canBeDeleted.Count == 0;
        }
    }
}