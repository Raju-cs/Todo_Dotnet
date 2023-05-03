using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> Complete();
    }
}