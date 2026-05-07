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
    public class ComboMediosController : Controller
    {
        private readonly ComboMediosRepository _repository;
        private readonly IConfiguration _config;
        public ComboMediosController(ComboMediosRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }
        [HttpPost("[action]")]
        public async Task<IEnumerable<ComboViewModel>> MostrarMedioRelacionado([FromBody] RelacionMedios relacionMedios)
        {
            var response = await _repository.GetByMostrarMedioRelacionado(relacionMedios);
            return response.Select(s => new ComboViewModel
            {
                IdProceso = s.IdProceso,
                Descripcion = s.Descripcion,
            });
        }
    }
}
