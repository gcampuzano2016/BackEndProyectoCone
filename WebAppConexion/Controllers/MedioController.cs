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
    public class MedioController : Controller
    {
        private readonly MediosRepository _repository;
        private readonly IConfiguration _config;
        public MedioController(MediosRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] MediosViewModel model)
        {
            Medios db = new Medios();
            db.IdMedio = model.IdMedio;
            db.Descripcion = model.Descripcion;
            db.RuCedula = model.RuCedula;
            db.CodPais = model.CodPais;
            db.ComisionAgencia = model.ComisionAgencia;
            db.ComisionCone = model.ComisionCone;
            db.Iva = model.Iva;
            db.FormaPago = model.FormaPago;
            db.Direccion = model.Direccion;
            db.Telefono = model.Telefono;
            db.Contacto = model.Contacto;
            db.Correo = model.Correo.ToLower();
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
        public async Task<IEnumerable<MediosViewModel>> MostrarMedio(Int64 IdMedio, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarMedio(IdMedio, Tipo);

            return response.Select(s => new MediosViewModel
            {
                IdMedio = s.IdMedio,
                Descripcion = s.Descripcion,
                RuCedula=s.RuCedula,
                CodPais=s.CodPais,
                ComisionAgencia=s.ComisionAgencia,
                ComisionCone=s.ComisionCone,
                Iva=s.Iva,
                FormaPago=s.FormaPago ,
                Direccion =s.Direccion ,
                Telefono = s.Telefono,
                Contacto = s.Contacto,
                Correo = s.Correo,
                Estado =s.Estado ,
                Tipo =s.Tipo
            });

        }
    }
}
