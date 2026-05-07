using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ComisionFinal
    {
        public decimal ValorCobrar { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ValorNeto { get; set; }
        public Int64 IdMedio { get; set; }
        public Int64 IdEmpleado { get; set; }
        public string NumDocumento { get; set; }
        public string NombresApellidos { get; set; }
        public string Medios { get; set; }
        public decimal Comision { get; set; }
        public decimal Porcentaje { get; set; }
        public string StrFechaRegistro { get; set; }
        public string NumContrato { get; set; }
        public string Anunciante { get; set; }
        public string EstadoPago { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }

    }
}
