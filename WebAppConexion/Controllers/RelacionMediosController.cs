using Conexion.AccesoDatos.Repository.Administracion;
using Conexion.Entidad.Administracion;
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
    public class RelacionMediosController : Controller
    {
        private readonly RelacionMediosRepository _repository;
        private readonly IConfiguration _config;
        public RelacionMediosController(RelacionMediosRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] CargaMasivaViewModel model)
        {
            CargaMasiva db = new CargaMasiva();
            db.IdProceso = model.IdProceso;
            db.TipoRegistro = model.TipoRegistro;
            db.TipoAccion = model.TipoAccion;
            db.Descripcion = model.Descripcion;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.Insert(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarRelacion([FromBody] RelacionMediosViewModel model)
        {
            RelacionMedios db = new RelacionMedios();
            db.IdRelacion = model.IdRelacion;
            db.IdMedio = model.IdMedio;
            db.IdCanal = model.IdCanal;
            db.IdPrograma = model.IdPrograma;
            db.IdDerecho = model.IdDerecho;
            db.IdFormato = model.IdFormato;
            db.Generico = model.Generico;
            db.IdUnidad = model.IdUnidad;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertRelacion(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }


    }
}
