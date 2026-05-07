using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class RelacionMediosViewModel
    {
        public Int64 IdRelacion { get; set; }
        public Int64 IdMedio { get; set; }
        public Int64 IdCanal { get; set; }
        public Int64 IdPrograma { get; set; }
        public Int64 IdDerecho { get; set; }
        public Int64 IdFormato { get; set; }
        public Int64 IdUnidad { get; set; }
        public string Generico { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
