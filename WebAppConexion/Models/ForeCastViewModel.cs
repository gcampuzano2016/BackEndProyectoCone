using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class ForeCastViewModel
    {
        public Int64 IdForeCast { get; set; }
        public Int64 IdCliente { get; set; }
        public Int64 IdMarca { get; set; }
        public Int64 IdMedio { get; set; }
        public Int64 IdAgencia { get; set; }
        public string IdCanal { get; set; }
        public string IdPrograma { get; set; }
        public string IdDerecho { get; set; }
        public string IdUnidad { get; set; }
        public Int64 IdNegocio { get; set; }
        public Int64 IdPropuesta { get; set; }
        public Int64 IdEmpleado { get; set; }
        public Int64 IdTipoRechazo { get; set; }
        public Int64 IdContacto { get; set; }
        public string NombreProyecto { get; set; }
        public string Agencia { get; set; }
        public string Contacto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public Int32 Cantidad { get; set; }
        public decimal Monto { get; set; }
        public decimal ValorTotalBruto { get; set; }
        public decimal PorcentajeAgencia { get; set; }
        public decimal ValorAgencia { get; set; }
        public decimal ValorTotalNeto { get; set; }
        public DateTime FechaInicioPauta { get; set; }
        public DateTime FechaFinalPauta { get; set; }
        public DateTime FechaTope { get; set; }
        public DateTime FechaCierre { get; set; }
        public string Seguimientollamada { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public string SeguimientoVisita { get; set; }
        public DateTime FechaVisita { get; set; }
        public string Usuario { get; set; }
        public string MotivoRechazo { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public int Tipo { get; set; }
        public int Estado { get; set; }
        public string NombresApellidos { get; set; }
        public string Medios { get; set; }
        public byte[] Pdfs { get; set; }
        public int Numcontrato { get; set; }
        public int NumPauta { get; set; }
        public int NumForeCast { get; set; }

    }
}
