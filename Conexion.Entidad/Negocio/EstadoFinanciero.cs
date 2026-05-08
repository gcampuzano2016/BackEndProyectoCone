using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class EstadoFinanciero
    {
        public string CODIGO { get; set; }
        public string CUENTA { get; set; }
        public decimal PARCIAL { get; set; }
        public decimal SUBTOTAL { get; set; }
        public decimal TOTAL { get; set; }
    }
}
