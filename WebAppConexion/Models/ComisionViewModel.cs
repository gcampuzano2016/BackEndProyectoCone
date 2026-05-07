using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ComisionViewModel
    {
        public Int64 IdComision { get; set; }
        public Int64 IdMedio { get; set; }
        public Int64 IdEmpleado { get; set; }
        public decimal CumpInicio { get; set; }
        public decimal CumpFinal { get; set; }
        public decimal Comisions { get; set; }
        public decimal Participacion { get; set; }
        public DateTime AnioComision { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public string Medios { get; set; }
        public string JsonMedio { get; set; }
        public string JsonEmpleado { get; set; }
    }
}
