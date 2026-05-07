using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class PlantillaContableViewModel
    {
        public Int64 IdPlantilla { get; set; }
        public string Descripcion { get; set; }
        public string json { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
