using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
   public class AprobarSolicitud
    {
        public Int64 IdVacaciones { get; set; }
        public Int64 IdEmpleado { get; set; }
        public Int64 IdTipoSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string Usuario { get; set; }
        public string MotivoAnulacion { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
