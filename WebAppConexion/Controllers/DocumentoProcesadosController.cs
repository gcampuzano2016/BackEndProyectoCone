using Conexion.AccesoDatos.Repository.Negocio;
using Conexion.Entidad.Negocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppConexion.Models;

namespace WebAppConexion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoProcesadosController : Controller
    {
        private readonly DocumentoProcesadosRepository _repository;
        private readonly IConfiguration _config;
        public DocumentoProcesadosController(DocumentoProcesadosRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DocumentoProcesadosViewModel>> MostrarDocumentosProcesados(DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarDocumentosProcesados(FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new DocumentoProcesadosViewModel
            {
                id =s.id,
                ruc = s.ruc,
                razonsocial =s.razonsocial,
                estado = s.estado,
                claveacceso = s.claveacceso,
                fechaautorizacion = s.fechaautorizacion,
                autorizacionsri =s.autorizacionsri,
                subtotalsinimpuesto =s.subtotalsinimpuesto,
                iva = s.iva,
                totalfactura =s.totalfactura,
                error = s.error,
                ruta =s.ruta,
                tipocomprobante = s.tipocomprobante,
                stringArchivo64 = s.stringArchivo64 
            });

        }



    }
}
