using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ForeCastIntermedio
    {
        public string Canal { get; set; }
        public string Programa { get; set; }
        public string Franja { get; set; }
        public string Derecho { get; set; }
        public string Formato { get; set; }
        public string Unidad { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal TarifaSegundos { get; set; }
        public decimal TotalSegundos { get; set; }
    }
}
