using mySimpleMessageService.Common;
using Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
               where TEntity : class, IEntity
    {
        protected readonly MessageServiceContext _dbContext;

        public GenericRepository(MessageServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Update(int id, TEntity entity)
        {
            if (await GetById(id) == null)
            {
                throw new HttpResponseException
                {
                    Status = HttpStatusCode.NotFound,
                    Value = "Not found."
                };
            }
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                throw new HttpResponseException
                {
                    Status = HttpStatusCode.NotFound,
                    Value = "Not found."
                };
            }
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
