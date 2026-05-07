using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ForeCastReporteViewModel
    {
        public string Cliente { get; set; }
        public string NombreProyecto { get; set; }
        public string Medios { get; set; }
        public string Agencia { get; set; }
        public string Contacto { get; set; }
        public string Negocio { get; set; }
        public string FechaInicioPauta { get; set; }
        public string FechaFinalPauta { get; set; }
        public int Cantidad { get; set; }
        public decimal Monto { get; set; }
        public string Vendedor { get; set; }
    }
}
