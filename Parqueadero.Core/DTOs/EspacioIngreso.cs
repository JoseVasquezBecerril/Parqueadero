using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Core.DTOs
{
    public class EspacioIngreso
    {
        public DateTime? FechaHoraIngreso { get; set; }
        public int NumeroEspacio { get; set; }
        public string Placa { get; set; }
    }
}
