using System;
using System.Threading.Tasks;

namespace Parqueadero.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEspacioRepository EspacioRepository { get; }
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
