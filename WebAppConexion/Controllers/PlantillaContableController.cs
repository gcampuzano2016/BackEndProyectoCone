using Conexion.AccesoDatos.Repository.Administracion;
using Conexion.Entidad.Administracion;
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
    public class PlantillaContableController : Controller
    {
        private readonly PlantillaContableRepository _repository;
        private readonly IConfiguration _config;
        public PlantillaContableController(PlantillaContableRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] PlantillaContableViewModel model)
        {
            PlantillaContable db = new PlantillaContable();
            db.IdPlantilla = model.IdPlantilla;
            db.Descripcion = model.Descripcion;
            db.json = model.json;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.Insert(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetallePlantillaViewModel>> MostrarPlantilla(Int64 IdPlantilla, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarPlantilla(IdPlantilla, Tipo);
            return response.Select(s => new DetallePlantillaViewModel
            {
                IdDetalle = s.IdDetalle,
                IdPlantilla = s.IdPlantilla,
                Descripcion = s.Descripcion,
                Tipo = s.Tipo,
                Estado = s.Estado

            });

        }


    }
}
