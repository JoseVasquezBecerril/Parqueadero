using Microsoft.EntityFrameworkCore;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Interfaces;
using Parqueadero.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parqueadero.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ParqueaderoContext _parqueaderoContext;
        protected readonly DbSet<T> _entities;
        public BaseRepository(ParqueaderoContext parqueaderoContext)
        {
            _parqueaderoContext = parqueaderoContext;
            _entities = parqueaderoContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
            await _parqueaderoContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
            _parqueaderoContext.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
            await _parqueaderoContext.SaveChangesAsync();

        }
    }
}
