using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ContratoReporte
    {
        public string Agencia { get; set; }
        public string Anunciante { get; set; }
        public string NumContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public decimal ComiAgen { get; set; }
    }
}
