using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
   public class CargarArchivoBase64
    {
        public Int64 IdRutaDoc { get; set; }
        public Int64 IdContrato { get; set; }
        public Int64 IdForeCast { get; set; }
        public string ArchivoBase64 { get; set; }
        public string NombreArchivo { get; set; }
        public string TipoDocumento { get; set; }
        public byte[] archivoBytes { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
