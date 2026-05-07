using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class CuentasPorPagar
    {
        public Int64 Id { get; set; }
        public Int64 IdProveedor { get; set; }
        public Int64 IdCuentaPorPagar { get; set; }
        public DateTime FechaAutorizacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string EstadoServicio { get; set; }
        public string RuCedula { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string AutorizacionSri { get; set; }
        public string NumDocumento { get; set; }
        public int PlazoVencimiento { get; set; }
        public decimal CompraTarifa0 { get; set; }
        public decimal CompraTarifa12 { get; set; }
        public decimal Iva { get; set; }
        public decimal ValorTotal { get; set; }
        public string TipoDocumento { get; set; }
        public string EstadoPago { get; set; }
        public string Mensaje { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public string jsonContable { get; set; }
        public string RutaDocumento { get; set; }
        public string stringArchivo64 { get; set; }
        public int PorRegistrar { get; set; }
        public decimal Saldo { get; set; }
    }
}
