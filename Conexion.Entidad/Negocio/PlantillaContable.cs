using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.Entidad.Negocio
{
   public  class PlantillaContable
    {
        public Int64 IdPlantilla { get; set; }
        public string Descripcion { get; set; }
        public string json { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
    }
}
