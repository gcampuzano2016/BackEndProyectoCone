using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class CargarFacturaViewModel
    {
        public decimal ValorBruto { get; set; }
        public decimal ValorNeto { get; set; }
        public decimal ValorCobrar { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string NumDocumento { get; set; }
        public string IdContrato { get; set; }
    }
}
