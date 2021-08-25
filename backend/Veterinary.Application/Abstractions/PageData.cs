using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinary.Application.Abstractions
{
    public class PageData
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}
