using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
   public  class ReferenciaCliente
    {
        public Int64 IdProceso { get; set; }
        public string Descripcion { get; set; }
        public string Contacto { get; set; }
        public Int64 Comision { get; set; }
    }
}
