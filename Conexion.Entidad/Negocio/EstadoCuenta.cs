using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class EstadoCuenta
    {
        public Int64 id { get; set; }
        public Int64 IdRegistro { get; set; }
        public string Fecha { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public string Concepto { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
