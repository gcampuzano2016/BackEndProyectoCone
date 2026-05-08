using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class SaldoVacacionesViewModel
    {
        public Int64 IdSaldoVacaciones { get; set; }
        public Int64 IdEmpleado { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFinal { get; set; }
        public decimal DiasGenerados { get; set; }
        public decimal DiasTomados { get; set; }
        public decimal Saldo { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
