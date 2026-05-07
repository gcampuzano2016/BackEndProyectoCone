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
    public class ProveedorController : Controller
    {
        private readonly ProveedorRepository _repository;
        private readonly IConfiguration _config;
        public ProveedorController(ProveedorRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ProveedorViewModel>> MostrarProveedor(Int64 IdProveedor, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarProveedor(IdProveedor, Tipo);
            return response.Select(s => new ProveedorViewModel
            {
                IdProveedor = s.IdProveedor,
                IdPlanCuenta = s.IdPlanCuenta,
                Descripcion = s.Descripcion,
                Nombre = s.Nombre ,
                NombreComercial = s.NombreComercial,
                RuCedula = s.RuCedula,
                Direccion = s.Direccion,
                Telefono = s.Telefono,
                Email = s.Email ,
                CodContable = s.CodContable,
                AutorizacionSri = s.AutorizacionSri,
                FechaAutorizacion = s.FechaAutorizacion,
                FechaCaducidad = s.FechaCaducidad,
                Estado = s.Estado 
            });

        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] ProveedorViewModel model)
        {
            Proveedor db = new Proveedor();
            db.IdProveedor = model.IdProveedor;
            db.Nombre = model.Nombre;
            db.NombreComercial = model.NombreComercial;
            db.RuCedula = model.RuCedula;
            db.Direccion = model.Direccion;
            db.Telefono = model.Telefono;
            db.Email = model.Email;
            db.CodContable = model.CodContable;
            db.AutorizacionSri = model.AutorizacionSri;
            db.FechaAutorizacion = model.FechaAutorizacion;
            db.FechaCaducidad = model.FechaCaducidad;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.Insert(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

    }
}
