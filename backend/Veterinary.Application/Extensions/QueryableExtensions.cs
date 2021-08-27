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
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> list, PageData pageData)
        {
            return new PagedList<T>()
            {
                TotalCount = await list.CountAsync(),
                PageIndex = pageData.PageIndex,
                PageSize = pageData.PageSize,
                Items = await list.Skip(pageData.PageSize * pageData.PageIndex).Take(pageData.PageSize).ToListAsync()
            };

        }
    }
}
