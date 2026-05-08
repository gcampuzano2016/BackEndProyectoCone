using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class CargarFactura
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
