using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;

namespace Veterinary.Dal.Repositories.Doctor
{
    public class TreatmentIntervalRepository : GenericRepository<TreatmentInterval>, ITreatmentIntervalRepository
    {
        public TreatmentIntervalRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public IQueryable<TreatmentInterval> GetTreatmentIntervalsByTreatmentIdAsQueryable(Guid treatmentId)
        {
            return GetAllAsQueryable().Where(treatmentInterval => treatmentInterval.TreatmentId == treatmentId)
                .AsQueryable();
        }
    }
}
