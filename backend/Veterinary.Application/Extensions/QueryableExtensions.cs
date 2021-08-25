using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Veterinary.Application.Abstractions;

namespace Veterinary.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> list, int pageSize, int pageIndex)
        {
            return new PagedList<T>()
            {
                TotalCount = await list.CountAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = await list.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync()
            };
            
        }
    }
}
