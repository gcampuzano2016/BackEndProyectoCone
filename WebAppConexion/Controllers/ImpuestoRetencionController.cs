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
    public class ImpuestoRetencionController : Controller
    {
        private readonly ImpuestoRetencionRepository _repository;
        private readonly IConfiguration _config;
        public ImpuestoRetencionController(ImpuestoRetencionRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ImpuestoViewModel>> MostrarImpuestos(Int32 IDImpuesto, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarImpuestos(IDImpuesto, Tipo);

            return response.Select(s => new ImpuestoViewModel
            {
                ID = s.ID,
                Codigo = s.Codigo,
                Nombre = s.Nombre,
                CodigoSRI = s.CodigoSRI,
                Porcentaje = s.Porcentaje,
            });

        }

    }
}
