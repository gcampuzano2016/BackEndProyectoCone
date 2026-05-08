using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class ContratoExterior
    {
        public string NumContrato { get; set; }
        public string NombreProyecto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Anunciante { get; set; }
        public string Email { get; set; }
    }
}
