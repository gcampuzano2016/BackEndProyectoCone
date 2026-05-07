using Conexion.AccesoDatos.Repository.Administracion;
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
    public class EmpresaController : Controller
    {
        private readonly EmpresaRepository _repository;
        private readonly IConfiguration _config;
        public EmpresaController(EmpresaRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EmpresaViewModel>> MostrarEmpresa(Int64 IdEmpresa, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarEmpresa(IdEmpresa, Tipo);

            return response.Select(s => new EmpresaViewModel
            {
                ID = s.ID,
                RUC = s.RUC,
                RazonSocial = s.RazonSocial,
                NombreComercial = s.NombreComercial,
                NumResolucion = s.NumResolucion,
                Direccion = s.Direccion,
                Telefono = s.Telefono,
            });

        }

    }
}
