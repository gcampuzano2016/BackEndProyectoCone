using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class EstadoFinancieroViewModel
    {
        public string CODIGO { get; set; }
        public string CUENTA { get; set; }
        public decimal PARCIAL { get; set; }
        public decimal SUBTOTAL { get; set; }
        public decimal TOTAL { get; set; }
    }
}
