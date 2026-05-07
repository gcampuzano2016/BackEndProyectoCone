using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class ReporteRelacionMedios
    {
        public Int64 IdRelacion { get; set; }
        public string Medios { get; set; }
        public string Canal { get; set; }
        public string Programa { get; set; }
        public string Derecho { get; set; }
        public string Formato { get; set; }
        public string Unidad { get; set; }
        public string Generico { get; set; }
    }
}
