using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Administracion
{
    public class PresupuestoMedios
    {
        public Int64 IdPresupuesto { get; set; }
        public Int64 IdMedio { get; set; }
        public Int64 IdEmpleado { get; set; }
        public decimal ValorPresupuesto { get; set; }
        public DateTime AnioPresupuesto { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public string Vendedor { get; set; }
        public string Medios { get; set; }
        public string Generico { get; set; }
        public string JsonMedio { get; set; }
        public string JsonGenerico { get; set; }
        public string JsonEmpleado { get; set; }
        public string json { get; set; }
    }
}
