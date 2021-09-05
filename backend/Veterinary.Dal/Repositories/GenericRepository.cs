using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Dal.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain;

namespace Veterinary.Dal.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly VeterinaryDbContext context;
        protected DbSet<T> Table;

        public GenericRepository(VeterinaryDbContext context)
        {
            this.context = context;
            this.Table = context.Set<T>();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return Table;
        }

        public Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = Table;

            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> InsertAsync(T entity)
        {
            Table.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            Table.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            T entity = Table.Find(id);
            Table.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<T> FindAsync(Guid id)
        {
            var entity = await Table.FindAsync(id);
            return entity ?? throw new EntityNotFoundException();
        }

        public async Task<bool> AnyByIdAsync(Guid id)
        {
            return await Table.FindAsync(id) != null;
        }
    }
}
