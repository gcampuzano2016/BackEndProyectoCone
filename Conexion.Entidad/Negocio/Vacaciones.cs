using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class Vacaciones
    {
        public Int64 IdVacaciones { get; set; }
        public Int64 IdTipoSolicitud { get; set; }
        public Int64 IdEmpleado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public String StrFechaRegistro { get; set; }
        public string Cedula { get; set; }
        public string Colaborador { get; set; }
        public string Departamento { get; set; }
        public string JefeInmediato { get; set; }
        public string Remplazo { get; set; }
        public DateTime FechaDesde { get; set; }
        public String StrFechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public String StrFechaHasta { get; set; }
        public decimal TotalDias { get; set; }
        public decimal Feriado { get; set; }
        public decimal SaldoDias { get; set; }
        public int CargoVacaciones { get; set; }
        public string Horas { get; set; }
        public string Actividad { get; set; }
        public string Observacion { get; set; }
        public string EstadoSolicitud { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public String StrFechaAprobacion { get; set; }
        public DateTime FechaRechazo{ get; set; }
        public String StrFechaRechazo { get; set; }
        public string UsuarioAprobo { get; set; }
        public string UsuarioRechazo { get; set; }
        public int Estado { get; set; }
        public string Ruta_Archivo { get; set; }
        public string Descripcion_Archivo { get; set; }
        public string MotivoAnulacion { get; set; }
        public int Tipo { get; set; }
        public string Descripcion { get; set; }
        public string CorreoJefe { get; set; }

    }
}
