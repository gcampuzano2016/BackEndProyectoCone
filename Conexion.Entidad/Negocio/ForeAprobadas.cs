using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ForeAprobadas
    {
        public Int64 IdForeCast { get; set; }
        public Int64 IdMedio { get; set; }
        public string Cliente { get; set; }
        public string Medios { get; set; }
        public string Agencia { get; set; }
        public string NombreProyecto { get; set; }
        public string Contacto { get; set; }
        public decimal ValorTotalNeto { get; set; }
        public decimal ValorTotalBruto { get; set; }
        public DateTime FechaInicioPauta { get; set; }
        public DateTime FechaFinalPauta { get; set; }
        public string NumContrato { get; set; }
        public int Estado { get; set; }
        public decimal TotalNegocio { get; set; }
        public decimal TotalSegundos { get; set; }
        public string RucVendedor { get; set; }
    }
}
