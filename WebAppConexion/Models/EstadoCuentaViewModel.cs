using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class EstadoCuentaViewModel
    {
        public Int64 id { get; set; }
        public Int64 IdRegistro { get; set; }
        public string Fecha { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public string Concepto { get; set; }
        public decimal Saldo { get; set; }
    }
}
