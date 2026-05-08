using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
   public class ImpuestoIva
    {
        public string ruc { get; set; }
        public string fechaemision { get; set; }
        public string serie { get; set; }
        public string secuencial { get; set; }
        public string claveacceso { get; set; }
        public decimal totalfactura { get; set; }
        public string porcentaje { get; set; }
        public decimal valorRetenido { get; set; }
    }
}
