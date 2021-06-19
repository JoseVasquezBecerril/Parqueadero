using Parqueadero.Core.Interfaces;
using Parqueadero.Infrastructure.Data;
using System.Threading.Tasks;

namespace Parqueadero.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ParqueaderoContext _parqueaderoContext;
        private readonly IEspacioRepository _espacioRepository;
        public UnitOfWork(ParqueaderoContext parqueaderoContext)
        {
            _parqueaderoContext = parqueaderoContext;
        }
        public IEspacioRepository EspacioRepository => _espacioRepository ?? new EspacioRepository(_parqueaderoContext);
        public void Dispose()
        {
            if (_parqueaderoContext != null)
                _parqueaderoContext.Dispose();
        }

        public void SaveChanges()
        {
            _parqueaderoContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _parqueaderoContext.SaveChangesAsync();
        }
    }
}
