using Microsoft.EntityFrameworkCore;
using Parqueadero.Core.DTOs;
using Parqueadero.Core.Entities;
using Parqueadero.Core.Enums;
using Parqueadero.Core.Interfaces;
using Parqueadero.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parqueadero.Infrastructure.Repositories
{
    public class EspacioRepository : BaseRepository<Espacio>, IEspacioRepository
    {
        public EspacioRepository(ParqueaderoContext context) : base(context) { }

        public async Task<IEnumerable<Espacio>> ObtenerEspaciosbyTipoVehiculo(TiposVehiculos tipoVehiculo)
        {
            return await _entities.Where(a => a.TipoVehiculo == tipoVehiculo).OrderBy(a => a.NumeroEspacio).ToListAsync();
        }

        public async Task<Espacio> ObtenerEspaciobyPlaca(string placa)
        {
            return await _entities.Where(x => x.Placa.Contains(placa)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Espacio>> ObtnenerDisponibilidad(Espacio espacio)
        {
            var disponibles = await _entities.Where(x => x.Estado == EstadosEspacios.Disponible && x.TipoVehiculo == espacio.TipoVehiculo).ToListAsync();

            return disponibles;
        }
    }
}
