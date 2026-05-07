using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class FacturaContrato
    {
        public Int64 IdContrato { get; set; }
        public string NumContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ComiAgen { get; set; }
        public decimal ValorAgen { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorConex {get; set;}
        public string Anunciante { get; set; }
        public string Mes { get; set; }
        public int Anio { get; set; }
        public string NumOrden { get; set; }

        public Int64 IdPagocontrato { get; set; }
        public string EstadoPago { get; set; }
        public decimal ValorCobrar { get; set; }
        public decimal ValorNeto { get; set; }
        public decimal ValorRetencion { get; set; }
        public decimal Saldo { get; set; }
        public decimal ComisionVendedor { get; set; }
        public decimal ComisionPorcentaje { get; set; }
        public string NumDocumento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NombreMedio { get; set; }
        public string Vendedor { get; set; }
        public string Agencia { get; set; }
        public decimal ValorRenta { get; set; }
        public decimal ValorIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
    }
}
