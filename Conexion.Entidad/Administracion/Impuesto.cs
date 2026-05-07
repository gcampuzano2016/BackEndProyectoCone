using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class Impuesto
    {
        public int ID { get; set; }
        public int IDImpuesto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string CodigoSRI { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
