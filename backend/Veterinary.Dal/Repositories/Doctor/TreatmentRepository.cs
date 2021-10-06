using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Dal.Repositories.Doctor
{
    public class TreatmentRepository : GenericRepository<Treatment>, ITreatmentRepository
    {
        public TreatmentRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<List<Treatment>> GetTreatmentsByDoctorId(Guid doctorId, bool getAll)
        {
            var treatmentsQuery = GetAllAsQueryable()
                .Where(treatment => treatment.DoctorId == doctorId);

            if (!getAll)
            {
                treatmentsQuery = treatmentsQuery
                    .Include(treatment => treatment.TreatmentIntervals)
                    .Where(treatment => !treatment.IsInactive && treatment.TreatmentIntervals.Count > 0);
            }

            return await treatmentsQuery.ToListAsync();
        }

        public async Task<bool> CanBeDeleted(Guid id)
        {
            if (!(await GetAllAsQueryable().AnyAsync(treatment => treatment.Id == id)))
            {
                throw new EntityNotFoundException();
            }

            var canBeDeleted = await GetAllAsQueryable()
                .Where(treatment => treatment.Id == id)
                .Include(x => x.TreatmentIntervals)
                .Select(x => x.TreatmentIntervals)
                .SingleAsync();

            return canBeDeleted.Count == 0;
        }
    }
}
