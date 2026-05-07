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
    public class ClienteController : Controller
    {
        private readonly ClienteRepository _repository;
        private readonly IConfiguration _config;
        public ClienteController(ClienteRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }
        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] ComboLlenarViewModel model)
        {
            ComboLlenar db = new ComboLlenar();
            db.IdProceso = model.IdProceso;
            db.Descripcion = model.Descripcion;
            db.Contacto = model.Contacto;
            db.Telefono = model.Telefono;
            db.Email = model.Email;
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
        public async Task<IEnumerable<ClienteAgenciaViewModel>> MostrarClienteAgencia(Int64 IdCliente, Int64 IdAgencia, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarCliente(IdCliente, IdAgencia, Tipo);

            return response.Select(s => new ClienteAgenciaViewModel
            {
                IdCliente = s.IdCliente,
                Descripcion = s.Descripcion,
                Contacto = s.Contacto,
                Telefono = s.Telefono,
                Email = s.Email,
            });

        }

    }
}
