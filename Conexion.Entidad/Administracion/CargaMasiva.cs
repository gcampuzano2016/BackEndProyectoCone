using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
   public class CargaMasiva
    {
        public Int64 IdProceso { get; set; }
        public string TipoRegistro { get; set; }
        public string TipoAccion { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
