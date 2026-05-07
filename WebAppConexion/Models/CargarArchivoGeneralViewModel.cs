using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class CargarArchivoGeneralViewModel
    {
        public Int64 IdRutaDoc { get; set; }
        public Int64 IdContrato { get; set; }
        public Int64 IdForeCast { get; set; }
        public string ArchivoBase64 { get; set; }
        public string NombreArchivo { get; set; }
        public string TipoDocumento { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
