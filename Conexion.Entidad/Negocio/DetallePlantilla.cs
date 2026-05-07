using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class DetallePlantilla
    {
        public Int64 IdDetalle { get; set; }
        public Int64 IdPlantilla { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public int Estado { get; set; }
    }
}
