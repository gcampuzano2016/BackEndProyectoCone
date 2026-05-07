using Conexion.AccesoDatos.Repository.Negocio;
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
    public class ComboForeCastController : Controller
    {
        private readonly ComboForeCastRepository _repository;
        private readonly IConfiguration _config;
        public ComboForeCastController(ComboForeCastRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ComboViewModel>> MostrarComboForeCast(Int32 Tipo, Int64 IdCliente)
        {
            var response = await _repository.GetByMostrarComboForeCast(Tipo, IdCliente);
            return response.Select(s => new ComboViewModel
            {
                IdProceso = s.IdProceso,
                Descripcion = s.Descripcion,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ComboViewModel>> MostrarDescripcionCombo(Int32 Tipo, Int64 IdProceso, string Descripcion)
        {
            var response = await _repository.GetByMostrarDescripcionCombo(Tipo, IdProceso, Descripcion);
            return response.Select(s => new ComboViewModel
            {
                IdProceso = s.IdProceso,
                Descripcion = s.Descripcion,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ReferenciaClienteViewModel>> MostrarComboForeCastCliente(Int32 Tipo, Int64 IdCliente,Int64 IdMedio, Int64 IdAgencia)
        {
            var response = await _repository.GetByMostrarComboForeCastCliente(Tipo, IdCliente, IdMedio, IdAgencia);
            return response.Select(s => new ReferenciaClienteViewModel
            {
                IdProceso = s.IdProceso,
                Descripcion = s.Descripcion,
                Contacto=s.Contacto,
                Comision=s.Comision
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ComboViewModel>> MostrarComboForeCastClienteAgencia(Int32 Tipo, Int64 IdCliente, Int64 IdMedio, Int64 IdAgencia)
        {
            var response = await _repository.GetByMostrarComboForeCastClienteAgencia(Tipo, IdCliente, IdMedio, IdAgencia);
            return response.Select(s => new ComboViewModel
            {
                IdProceso = s.IdProceso,
                Descripcion = s.Descripcion,
            });
        }
    }
}
