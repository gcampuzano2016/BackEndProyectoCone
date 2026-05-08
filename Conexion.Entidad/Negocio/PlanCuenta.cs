using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class PlanCuenta
    {
        public Int64 IdPlanCuenta { get; set; }
        public int IdPadre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal SaldoFinal { get; set; }
        public int Tipo { get; set; }
        public int Estado { get; set; }

        public DateTime FechaCierre { get; set; }

    }
}
