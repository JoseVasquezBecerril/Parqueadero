using Parqueadero.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Core.DTOs
{
    public class EspacioSalida
    {
        public DateTime? FechaHoraSalida { get; set; }
        public decimal? ValorServicio { get; set; }
        public string Placa { get; set; }
        public TiposVehiculos TipoVehiculo { get; set; }
    }
}
