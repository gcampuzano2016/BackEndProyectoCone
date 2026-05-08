using Conexion.AccesoDatos.Repository.Administracion;
using Conexion.AccesoDatos.Repository.Negocio;
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
    public class VacacionesController : Controller
    {
        private readonly VacacionesRepository _repository;
        private readonly IConfiguration _config;
        public VacacionesController(VacacionesRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<GenericaVP>> GuardarVacaciones([FromBody] VacacionesViewModel model)
        {

            Vacaciones db = new Vacaciones();
            db.IdVacaciones = model.IdVacaciones;
            db.IdTipoSolicitud = model.IdTipoSolicitud;
            db.IdEmpleado = model.IdEmpleado;
            db.FechaRegistro = model.FechaRegistro;
            db.Cedula = model.Cedula;
            db.Colaborador = model.Colaborador;
            db.Departamento = model.Departamento;
            db.JefeInmediato = model.JefeInmediato;
            db.Remplazo = model.Remplazo;
            db.FechaDesde = model.FechaDesde;
            db.FechaHasta = model.FechaHasta;
            db.TotalDias = model.TotalDias;
            db.Feriado = model.Feriado;
            db.SaldoDias = model.SaldoDias;
            db.CargoVacaciones = model.CargoVacaciones;
            db.Horas = model.Horas;
            db.Actividad = model.Actividad;
            db.Observacion = model.Observacion;
            db.EstadoSolicitud = model.EstadoSolicitud;
            db.FechaAprobacion = model.FechaAprobacion;
            db.FechaRechazo = model.FechaRechazo;
            db.UsuarioAprobo = model.UsuarioAprobo;
            db.UsuarioRechazo = model.UsuarioRechazo;
            db.Estado = model.Estado;
            db.Ruta_Archivo = model.Ruta_Archivo;
            db.Descripcion_Archivo = model.Descripcion_Archivo;
            db.MotivoAnulacion = model.MotivoAnulacion;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertVacaciones(db);
            return responseResul.Select(s => new GenericaVP
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });

        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarSaldoVacaciones([FromBody] SaldoVacacionesViewModel model)
        {

            SaldoVacaciones db = new SaldoVacaciones();
            db.IdSaldoVacaciones = model.IdSaldoVacaciones;
            db.IdEmpleado = model.IdEmpleado;
            db.PeriodoInicio = model.PeriodoInicio;
            db.PeriodoFinal = model.PeriodoFinal;
            db.DiasGenerados = model.DiasGenerados;
            db.DiasTomados = model.DiasTomados;
            db.Saldo = model.Saldo;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertSaldoVacaciones(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });

        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarAprobacionSolicitud([FromBody] AprobarSolicitudViewModel model)
        {

            AprobarSolicitud db = new AprobarSolicitud();
            db.IdVacaciones = model.IdVacaciones;
            db.IdEmpleado = model.IdEmpleado;
            db.IdTipoSolicitud = model.IdTipoSolicitud;
            db.EstadoSolicitud = model.EstadoSolicitud;
            db.Usuario = model.Usuario;
            db.MotivoAnulacion = model.MotivoAnulacion;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertAprobacionSolicitud(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PrmTipoSolicitudViewModel>> MostrarPrmTipoSolicitud()
        {
            var response = await _repository.GetByMostrarPrmTipoSolicitud();
            return response.Select(s => new PrmTipoSolicitudViewModel
            {
                IdTipoSolicitud = s.IdTipoSolicitud,
                Descripcion = s.Descripcion,
                Estado = s.Estado,
            });

        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<PrmEstadoSolicitudViewModel>> MostrarPrmEstadoSolicitud()
        {
            var response = await _repository.GetByMostrarPrmEstadoSolicitud();
            return response.Select(s => new PrmEstadoSolicitudViewModel
            {
                IdEstadoSolicitud = s.IdEstadoSolicitud,
                Descripcion = s.Descripcion,
                Estado = s.Estado,
            });

        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<VacacionesViewModel>> MostrarVacaciones([FromBody] FiltroVacaciones model)
        {
            var response = await _repository.GetByMostrarVacaciones(model);
            return response.Select(s => new VacacionesViewModel
            {
                IdVacaciones=s.IdVacaciones,
                IdTipoSolicitud = s.IdTipoSolicitud,
                Descripcion = s.Descripcion,
                IdEmpleado = s.IdEmpleado,
                StrFechaRegistro = s.FechaRegistro.ToString("yyyy-MM-dd"),
                Cedula = s.Cedula,
                Colaborador = s.Colaborador,
                Departamento = s.Departamento,
                JefeInmediato = s.JefeInmediato,
                Remplazo = s.Remplazo,
                StrFechaDesde = s.FechaDesde.ToString("yyyy-MM-dd hh:mm:ss"),
                StrFechaHasta = s.FechaHasta.ToString("yyyy-MM-dd hh:mm:ss"),
                TotalDias =s.TotalDias,
                Feriado =s.Feriado,
                SaldoDias = s.SaldoDias,
                CargoVacaciones =s.CargoVacaciones ,
                Horas = s.Horas,
                Actividad = s.Actividad,
                Observacion = s.Observacion,
                EstadoSolicitud = s.EstadoSolicitud,
                StrFechaAprobacion =s.FechaAprobacion.ToString("yyyy-MM-dd"),
                StrFechaRechazo =s.FechaRechazo.ToString("yyyy-MM-dd"),
                UsuarioAprobo =s.UsuarioAprobo,
                UsuarioRechazo =s.UsuarioRechazo,
                Estado = s.Estado,
                Ruta_Archivo =s.Ruta_Archivo,
                Descripcion_Archivo = s.Descripcion_Archivo,
                MotivoAnulacion = s.MotivoAnulacion,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<SaldoVacacionesViewModel>> MostrarDiasVacacionesGenerados(Int64 IdEmpleado, int Tipo)
        {
            var response = await _repository.GetByMostrarDiasVacacionesGenerados(IdEmpleado,Tipo);
            return response.Select(s => new SaldoVacacionesViewModel
            {
                IdSaldoVacaciones = s.IdSaldoVacaciones,
                IdEmpleado = s.IdEmpleado,
                PeriodoInicio = s.PeriodoInicio,
                PeriodoFinal = s.PeriodoFinal,
                DiasGenerados = s.DiasGenerados,
                DiasTomados = s.DiasTomados,
                Saldo = s.Saldo,
                Estado = s.Estado,
            });

        }

    }
}
