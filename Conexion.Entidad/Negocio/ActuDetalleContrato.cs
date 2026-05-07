using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ActuDetalleContrato
    {
        public Int64 IdDetalleContrato { get; set; }
        public Int64 IdMaterialR { get; set; }
        public Int64 IdMaterialE { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
