using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class AprobarSolicitudViewModel
    {
        public Int64 IdVacaciones { get; set; }
        public Int64 IdEmpleado { get; set; }
        public Int64 IdTipoSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string Usuario { get; set; }
        public string MotivoAnulacion { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
