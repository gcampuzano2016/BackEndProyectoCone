using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ForeCast
    {
        public Int64 IdForeCast { get; set; }
        public string Agencia { get; set; }
        public Int64 IdMarca { get; set; }
        public Int64 IdMedio { get; set; }
        public Int64 IdCanal { get; set; }
        public Int64 IdPrograma { get; set; }
        public Int64 IdDerecho { get; set; }
        public Int64 IdNegocio { get; set; }
        public Int64 IdPropuesta { get; set; }
        public string NombreProyecto { get; set; }
        public string Contacto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int Cantidad { get; set; }
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
        public DateTime UltimaModificacion { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }

    }
}
