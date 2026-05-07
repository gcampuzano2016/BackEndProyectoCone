using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class Sucursal
    {
        public int IDSU { get; set; }
        public int IDCI { get; set; }
        public int IDEmpresa { get; set; }
        public int IDocumento { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string PuntoEmision { get; set; }
        public string Establecimiento { get; set; }
        public int Secuencial { get; set; }
        public string  Administrador { get; set; }

    }
}
