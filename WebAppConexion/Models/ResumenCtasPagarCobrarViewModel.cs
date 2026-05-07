using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ResumenCtasPagarCobrarViewModel
    {
        public Int64 IdProcesoContable { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string StrFechaRegistro { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
    }
}
