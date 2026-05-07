using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ImpuestoViewModel
    {
        public int ID { get; set; }
        public int IDImpuesto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string CodigoSRI { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
