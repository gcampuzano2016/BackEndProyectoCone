using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ConsumoPautasViewModel
    {
        public string NumContrato { get; set; }
        public string Anunciante { get; set; }
        public decimal TotalNegocio { get; set; }
        public decimal TotalSegundos { get; set; }
        public decimal TotalNegocioConsumido { get; set; }
        public decimal TotalSegundosConsumido { get; set; }
        public decimal SaldoTotalNegocio { get; set; }
        public decimal SaldoTotalSegundos { get; set; }
    }
}
