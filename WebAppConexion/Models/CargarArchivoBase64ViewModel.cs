using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class CargarArchivoBase64ViewModel
    {
        public string ArchivoBase64 { get; set; }
        public string NombreArchivo { get; set; }

        public byte[] archivoBytes { get; set; }
    }
}
