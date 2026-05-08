using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class ForeCastJson
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
        public string Agencia { get; set; }
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
        public string MotivoRechazo { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public int Tipo { get; set; }
        public int Estado { get; set; }
        public string JsonMedio { get; set; }
        public string MedioSinJson { get; set; }

        public string JsonCanal { get; set; }
        public string CanalSinJson { get; set; }

        public string JsonPrograma { get; set; }
        public string ProgramaSinJson { get; set; }

        public string JsonDerecho { get; set; }
        public string DerechoSinJson { get; set; }
        public string JsonUnidad { get; set; }
        public string UnidadSinJson { get; set; }
        public string NombresApellidos { get; set; }
        public int Numcontrato { get; set; }
        public int NumPauta { get; set; }
        public int NumForeCast { get; set; }
        public decimal TotalNegocio { get; set; }
        public decimal TotalSegundos { get; set; }
    }
}
