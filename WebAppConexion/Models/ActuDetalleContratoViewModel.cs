using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ActuDetalleContratoViewModel
    {
        public Int64 IdDetalleContrato { get; set; }
        public Int64 IdMaterialR { get; set; }
        public Int64 IdMaterialE { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
