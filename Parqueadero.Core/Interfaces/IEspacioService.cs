using Parqueadero.Core.DTOs;
using Parqueadero.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parqueadero.Core.Interfaces
{
    public interface IEspacioService
    {
        Task<Espacio> IngresoVehiculo(Espacio espacio);
        Task<Espacio> SalidaVehiculo(Espacio espacio);
        Task<Espacio> ConsultaSaldo(Espacio espacio);
        Task<IEnumerable<Espacio>> ObtenerEspaciosbyTipoVehiculo(Espacio espacio);
    }
}
