using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class CargaMasivaViewModel
    {
        public Int64 IdProceso { get; set; }
        public string TipoRegistro { get; set; }
        public string TipoAccion { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
