using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppConexion.Models
{
    public class DocumentoProcesadosViewModel
    {
        public int id { get; set; }
        public string ruc { get; set; }
        public string razonsocial { get; set; }
        public string estado { get; set; }
        public string claveacceso { get; set; }
        public DateTime fechaautorizacion { get; set; }
        public string autorizacionsri { get; set; }
        public decimal subtotalsinimpuesto { get; set; }
        public decimal iva { get; set; }
        public decimal totalfactura { get; set; }
        public string error { get; set; }
        public string ruta { get; set; }
        public string tipocomprobante { get; set; }
        public string stringArchivo64 { get; set; }
    }
}
