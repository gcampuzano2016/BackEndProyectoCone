using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ComboLlenarViewModel
    {
        public Int64 IdProceso { get; set; }
        public string Descripcion { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
