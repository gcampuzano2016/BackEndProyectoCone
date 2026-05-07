using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class RegistroContableViewModel
    {
        public Int64 IdRegistro { get; set; }
        public Int64 IdProcesoContable { get; set; }
        public Int64 IdPlanCuenta { get; set; }
        public string TipoTransaccion { get; set; }
        public string Fecha { get; set; }
        public string Concepto { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal Valor { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Proceso { get; set; }
    }
}
