using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class EmpresaViewModel
    {
        public Int32 ID { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string NumResolucion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
}
