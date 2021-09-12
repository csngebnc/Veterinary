using System.Collections.Generic;
using System.Threading.Tasks;

namespace Veterinary.Domain.Entities
{
    public interface IDoctorRepository
    {
        Task<List<(VeterinaryUser User, string RoleName)>> GetDoctors();
    }
}
