using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ReferenciaClienteViewModel
    {
        public Int64 IdProceso { get; set; }
        public string Descripcion { get; set; }
        public string Contacto { get; set; }
        public Int64 Comision { get; set; }
    }
}
