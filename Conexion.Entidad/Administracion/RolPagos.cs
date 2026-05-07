using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class RolPagos
    {
        public Int64 IdPagoRol { get; set; }
        public string RuCedula { get; set; }
        public string NombresApellidos { get; set; }
        public string CadenaValores { get; set; }
        public DateTime FechaPago { get; set; }
        public string ArchivoBase64 { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
