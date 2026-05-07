using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ReporteRelacionMediosViewModel
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
