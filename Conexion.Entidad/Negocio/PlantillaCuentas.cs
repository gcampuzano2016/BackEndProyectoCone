using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
   public class PlantillaCuentas
    {
        public Int64 IdPlanCuenta { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DescripcionMovimiento { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string tipo { get; set; }
        public string NumDocumento { get; set; }
    }
}
