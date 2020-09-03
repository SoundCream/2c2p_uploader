using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2P.FileUploader.Interfaces.Data
{
    public interface IUnitOfWork
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;

        void Insert<TEntity>(TEntity entity) where TEntity : class;

        void Insert<TEntity>(List<TEntity> entity) where TEntity : class;

        Task<int> SaveChangeAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
