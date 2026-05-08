using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class FiltroVacacionesViewModel
    {
        public Int64 IdEmpleado { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public Int64 IdTipoSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public int Filtro { get; set; }
        public int Tipo { get; set; }
    }
}
