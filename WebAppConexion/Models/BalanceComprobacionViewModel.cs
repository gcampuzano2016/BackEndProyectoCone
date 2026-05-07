using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class BalanceComprobacionViewModel
    {
        public Int64 IdPlanCuenta { get; set; }
        public string CODIGO { get; set; }
        public string CUENTA { get; set; }
        public Decimal INICIAL { get; set; }
        public Decimal DEBITOS { get; set; }
        public Decimal CREDITOS { get; set; }
        public Decimal SALDOFINAL { get; set; }
    }
}
