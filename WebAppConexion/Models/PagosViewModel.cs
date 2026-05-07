using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class PagosViewModel
    {
        public Int64 IdPago { get; set; }
        public Int64 IdCuentaPorPagar { get; set; }
        public string FormaPago { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public decimal Total { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
