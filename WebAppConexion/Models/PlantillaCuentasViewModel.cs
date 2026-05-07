using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class PlantillaCuentasViewModel
    {
        public Int64 IdPlanCuenta { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DescripcionMovimiento { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string tipo { get; set; }
    }
}
