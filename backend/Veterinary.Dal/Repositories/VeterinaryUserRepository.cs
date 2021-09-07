using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;

namespace Veterinary.Dal.Repositories
{
    public class VeterinaryUserRepository : GenericRepository<VeterinaryUser>, IVeterinaryUserRepository
    {
        public VeterinaryUserRepository(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task<List<VeterinaryUser>> Search(string param)
        {
            var result = await Table.Where(u => u.Name.ToLower().Contains(param) ||
                        u.Email.ToLower().Contains(param) ||
                        u.Address.ToLower().Contains(param))
                .ToListAsync();

            return result;
        }
    }
}
