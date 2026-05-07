using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class UnionIngresoEgresoViewModel
    {
        public Int64 IdProcesoContable { get; set; }
        public Int64 Venta { get; set; }
        public Int64 Compra { get; set; }
        public Int64 Pagos { get; set; }
        public Int64 Cobros { get; set; }
        public string TipoTransaccion { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
