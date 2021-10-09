using Veterinary.Dal.Data;
using Veterinary.Domain.Entities.MedicalRecordEntities;

namespace Veterinary.Dal.Repositories.MedicalRecordTextTemplateRepository
{
    public class MedicalRecordTextTemplateRepository : GenericRepository<MedicalRecordTextTemplate>, IMedicalRecordTextTemplateRepository
    {
        public MedicalRecordTextTemplateRepository(VeterinaryDbContext context) : base(context)
        {
        }
    }
}