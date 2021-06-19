using Parqueadero.Core.DTOs;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parqueadero.Core.Interfaces
{
    public interface IEspacioRepository : IRepository<Espacio>
    {
        Task<IEnumerable<Espacio>> ObtnenerDisponibilidad(Espacio espacio);
        Task<Espacio> ObtenerEspaciobyPlaca(string placa);
        Task<IEnumerable<Espacio>> ObtenerEspaciosbyTipoVehiculo(TiposVehiculos tipoVehiculo);
    }
}
