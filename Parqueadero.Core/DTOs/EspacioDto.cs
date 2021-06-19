using Parqueadero.Core.Enums;

namespace Parqueadero.Core.DTOs
{
    public class EspacioDto
    {
        public TiposVehiculos TipoVehiculo { get; set; }
        public string Placa { get; set; }

        public decimal Cilindraje { get; set; }
    }
}
