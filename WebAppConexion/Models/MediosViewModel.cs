using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class MediosViewModel
    {
        public Int64 IdMedio { get; set; }
        public string Descripcion { get; set; }
        public string RuCedula { get; set; }
        public string CodPais { get; set; }
        public decimal ComisionAgencia { get; set; }
        public decimal ComisionCone { get; set; }
        public int Iva { get; set; }
        public string FormaPago { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
