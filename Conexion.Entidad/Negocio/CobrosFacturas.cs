using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class CobrosFacturas
    {
        public string Medios { get; set; }
        public decimal Valor { get; set; }
        public string NumDocumento { get; set; }
        public string EstadoPago { get; set; }
        public string Conex { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ValorNeto { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Comision { get; set; }
        public string Vendedor { get; set; }
        public string EstadoComision { get; set; }
        public DateTime FechaCobroFactura { get; set; }
    }
}
