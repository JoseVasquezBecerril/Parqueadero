using Parqueadero.Core.Enums;
using System;

namespace Parqueadero.Core.Entities
{
    public class Espacio : BaseEntity
    {
        public int NumeroEspacio { get; set; }
        public TiposVehiculos TipoVehiculo { get; set; }
        public EstadosEspacios Estado { get; set; }
        public string Placa { get; set; }
        public decimal? Cilindraje { get; set; }
        public DateTime? FechaHoraIngreso { get; set; }
        public DateTime? FechaHoraSalida { get; set; }
        public decimal? ValorServicio { get; set; }


    }
}
