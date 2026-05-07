using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class DetalleContratoViewModel
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
    }
}
