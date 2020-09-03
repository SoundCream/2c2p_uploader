using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2C2P.FileUploader.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace _2C2P.FileUploader.Data
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction _dbContextTransaction;

        public UnitOfWork(T dbContext)
        {
            _dbContext = dbContext;
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Insert<TEntity>(List<TEntity> entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().AddRange(entity);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.RollbackAsync();
            }
        }
    }
}
