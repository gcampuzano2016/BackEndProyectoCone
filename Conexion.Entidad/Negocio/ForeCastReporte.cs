using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ForeCastReporte
    {
        public string Cliente { get; set; }
        public string NombreProyecto { get; set; }
        public string Medios { get; set; }
        public string Agencia { get; set; }
        public string Contacto { get; set; }
        public string Negocio { get; set; }
        public DateTime FechaInicioPauta { get; set; }
        public DateTime FechaFinalPauta { get; set; }
        public int Cantidad { get; set; }
        public decimal Monto { get; set; }
        public string Vendedor { get; set; }
        public decimal PorcentajeAgencia { get; set; }
    }
}
