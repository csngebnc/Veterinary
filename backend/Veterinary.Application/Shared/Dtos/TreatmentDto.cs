using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Application.Shared.Dtos
{
    public class TreatmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool IsInactive { get; set; }
    }
}
