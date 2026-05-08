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
    public class ComisionPresupuestoController : Controller
    {
        private readonly ComisionPresupuestoRepository _repository;
        private readonly IConfiguration _config;
        public ComisionPresupuestoController(ComisionPresupuestoRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarPresupuesto([FromBody] PresupuestoMediosViewModel model)
        {
            PresupuestoMedios db = new PresupuestoMedios();
            db.IdPresupuesto = model.IdPresupuesto;
            db.IdMedio = model.IdMedio;
            db.IdEmpleado = model.IdEmpleado;
            db.ValorPresupuesto = model.ValorPresupuesto;
            db.AnioPresupuesto = model.AnioPresupuesto;
            db.Generico = model.Generico;
            db.json = model.json;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertPresupuesto(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarComision([FromBody] ComisionViewModel model)
        {
            Comision db = new Comision();
            db.IdComision = model.IdComision;
            db.IdEmpleado = model.IdEmpleado;
            db.IdMedio = model.IdMedio;
            db.CumpInicio = model.CumpInicio;
            db.CumpFinal = model.CumpFinal;
            db.Comisions = model.Comisions;
            db.Participacion = model.Participacion;
            db.AnioComision = model.AnioComision;
            db.Generico = model.Generico;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertComision(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PresupuestoMediosViewModel>> MostrarPresupuesto(Int64 IdPresupuesto,Int64 IdMedio, Int64 IdEmpleado, Int32 mes, Int32 anio, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarPresupuestoMedios(IdPresupuesto, IdMedio, IdEmpleado, mes, anio, Tipo);

            return response.Select(s => new PresupuestoMediosViewModel
            {
                IdPresupuesto = s.IdPresupuesto,
                IdMedio = s.IdMedio,
                IdEmpleado = s.IdEmpleado,
                ValorPresupuesto = s.ValorPresupuesto,
                AnioPresupuesto = s.AnioPresupuesto,
                Estado = s.Estado,
                Medios = s.Medios,
                Vendedor = s.Vendedor,
                Generico = s.Generico,
                JsonMedio = s.JsonMedio,
                JsonEmpleado = s.JsonEmpleado,
                JsonGenerico =s.JsonGenerico,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ComisionViewModel>> MostrarComision(Int64 IdComision,Int64 IdMedio, Int64 IdEmpleado, Int32 mes, Int32 anio, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarComision(IdComision, IdMedio, IdEmpleado, mes, anio, Tipo);

            return response.Select(s => new ComisionViewModel
            {
                IdComision = s.IdComision,
                IdMedio = s.IdMedio,
                IdEmpleado = s.IdEmpleado,
                CumpInicio = s.CumpInicio,
                CumpFinal = s.CumpFinal,
                Comisions = s.Comisions,
                Participacion = s.Participacion ,
                AnioComision = s.AnioComision,
                Generico = s.Generico ,
                Estado = s.Estado,
                Medios = s.Medios,
                JsonMedio = s.JsonMedio,
                JsonEmpleado = s.JsonEmpleado,
                JsonGenerico = s.JsonGenerico,
                Vendedor =s.Vendedor,
            });

        }
    }
}
