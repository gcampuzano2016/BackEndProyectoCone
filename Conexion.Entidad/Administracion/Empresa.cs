using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class Empresa
    {
        public Int32 ID { get; set; }
        public string RUC  { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string NumResolucion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
}
