using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class SeguimientoForeCast
    {
        public Int64 IdForeCast { get; set; }
        public string NombresApellidos { get; set; }
        public string FechaSeguimiento { get; set; }
        public string Seguimientollamada { get; set; }
        public string SeguimientoVisita { get; set; }
        public string Cliente { get; set; }
        public string Agencia { get; set; }
        public string Negocio { get; set; }
        public decimal ValorTotalBruto { get; set; }

    }
}
