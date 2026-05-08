using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
    public class DetalleContrato
    {
        public Int64 IdDetalleContrato { get; set; }
        public Int64 IdMaterialE { get; set; }
        public Int64 IdMaterialR { get; set; }
        public string Canal { get; set; }
        public string Programa { get; set; }
        public string Duracion { get; set; }
        public string Franja { get; set; }
        public decimal Tarifa { get; set; }
        public decimal TotalSegundos { get; set; }
        public decimal ValorNegocio { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
        public string Versiones { get; set; }

    }
}
