using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ConfiguracionArchivoViewModel
    {
        public Int64 IdContrato { get; set; }
        public Int64 IdForeCast { get; set; }
        public string RutaArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string NombreArchivoSalida { get; set; }
        public string Extencion { get; set; }
    }
}
