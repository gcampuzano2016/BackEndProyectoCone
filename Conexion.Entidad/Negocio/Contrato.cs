using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class Contrato
    {
        public Int64 IdContrato { get; set; }
        public Int64 IdForeCast { get; set; }
        public Int64 IdOrdenRecibida { get; set; }
        public Int64 IdOrdenEnviada { get; set; }
        public Int64 IdMaterialR { get; set; }
        public Int64 IdMaterialE { get; set; }
        public Int64 IdFacturado { get; set; }
        public Int64 IdCertificado { get; set; }
        public Int64 IdPagado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string NumContrato { get; set; }
        public string NumOrden { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public decimal ValorBruto  { get; set; }
        public decimal  ComiAgen { get; set; }
        public  decimal ValorAgen { get; set; }
        public decimal Valor { get; set; }
        public decimal ComiConex { get; set; }
        public decimal ValorConex { get; set; }
        public string RucVendedor { get; set; }
        public decimal ComiVendedor { get; set; }
        public string Anunciante { get; set; }
        public string Agencia { get; set; }
        public string Contacto { get; set; }
        public string Medio { get; set; }
        public decimal Facturado { get; set; }
        public DateTime FechaCobro { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public string NombreProyecto { get; set; }
    }
}
