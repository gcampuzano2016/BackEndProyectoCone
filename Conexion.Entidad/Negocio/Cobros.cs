using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class Cobros
    {
        public Int64 IdCobro { get; set; }
        public Int64 IdPagocontrato { get; set; }
        public string FormaPago { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public decimal Total { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
