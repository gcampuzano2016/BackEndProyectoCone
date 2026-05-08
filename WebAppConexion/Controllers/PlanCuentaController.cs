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
    public class PlanCuentaController : Controller
    {


        private readonly PlanCuentaRepository _repository;
        private readonly IConfiguration _config;
        public PlanCuentaController(PlanCuentaRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] PlanCuentaViewModel model)
        {
            PlanCuenta db = new PlanCuenta();
            db.IdPlanCuenta = model.IdPlanCuenta;
            db.IdPadre = model.IdPadre;
            db.Codigo = model.Codigo;
            db.Descripcion = model.Descripcion;
            db.SaldoInicial = model.SaldoInicial;
            db.Debe = model.Debe;
            db.Haber = model.Haber;
            db.SaldoFinal = model.SaldoFinal;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.Insert(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> CierrePlanCuentas([FromBody] PlanCuentaViewModel model)
        {
            PlanCuenta db = new PlanCuenta();
            db.FechaCierre = model.FechaCierre;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.CierrePlanCuentas(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PlanCuentaViewModel>> MostrarPlanCuentas(Int64 IdPlanCuenta, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarPlanCuentas(IdPlanCuenta, Tipo);
            return response.Select(s => new PlanCuentaViewModel
            {
                IdPlanCuenta = s.IdPlanCuenta,
                SaldoInicial = s.SaldoInicial,
                Descripcion=s.Descripcion,
                Codigo=s.Codigo,
                Debe = s.Debe,
                Haber = s.Haber,
                SaldoFinal = s.SaldoFinal,              
                Estado = s.Estado,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PlanCuentaViewModel>> MostrarPlanCuentasSaldoFinal(Int64 IdPlanCuenta,DateTime FechaInicio, Int32 Tipo)
        {
            var response = await _repository.GetByPlanCuentasSaldoFinal(IdPlanCuenta, FechaInicio, Tipo);
            return response.Select(s => new PlanCuentaViewModel
            {
                IdPlanCuenta = s.IdPlanCuenta,
                SaldoInicial = s.SaldoInicial,
                Descripcion = s.Descripcion,
                Codigo = s.Codigo,
                Debe = s.Debe,
                Haber = s.Haber,
                SaldoFinal = s.SaldoFinal,
                Estado = s.Estado,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PlanCuentaViewModel>> MostrarPlanCuentasSaldoFinalCero(Int64 IdPlanCuenta, DateTime FechaInicio, Int32 Tipo)
        {
            var response = await _repository.GetByPlanCuentasSaldoFinalCero(IdPlanCuenta, FechaInicio, Tipo);
            return response.Select(s => new PlanCuentaViewModel
            {
                IdPlanCuenta = s.IdPlanCuenta,
                SaldoInicial = s.SaldoInicial,
                Descripcion = s.Descripcion,
                Codigo = s.Codigo,
                Debe = s.Debe,
                Haber = s.Haber,
                SaldoFinal = s.SaldoFinal,
                Estado = s.Estado,
            });

        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<UnionIngresoEgresoViewModel>> MostrarUnionIngresoEgreso(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarUnionIngresoEgreso(IdProcesoContable, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new UnionIngresoEgresoViewModel
            {
                IdProcesoContable = s.IdProcesoContable,
                Venta = s.Venta,
                Compra = s.Compra,
                Pagos = s.Pagos,
                Cobros = s.Cobros,
                TipoTransaccion = s.TipoTransaccion,
                Descripcion = s.Descripcion,
                Valor =s.Valor,
                FechaRegistro = s.FechaRegistro,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EstadoCuentaViewModel>> MostrarReporteEstadoCuenta(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarReporteEstadoCuenta(IdPlanCuenta, FechaInicio, FechaFinal, Tipo);


            return response.Select(s => new EstadoCuentaViewModel
            {
                id = s.id,
                IdRegistro = s.IdRegistro,
                Fecha = s.Fecha,
                Debito = s.Debito,
                Credito = s.Credito,
                Concepto = s.Concepto,
                Saldo = s.Saldo,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<LibroMayorViewModel>> MostrarReporteEstadoCuentaLibroMayor(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarReporteEstadoCuentaLibroMayor(IdPlanCuenta, FechaInicio, FechaFinal, Tipo);


            return response.Select(s => new LibroMayorViewModel
            {
                FECHA = s.FECHA,
                CONCEPTO = s.CONCEPTO,
                DEBITO = s.DEBITO,
                CREDITO =s.CREDITO,
                SALDO =s.SALDO,
            });

        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<BalanceComprobacionViewModel>> MostrarReporteBalanceComprobacion(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarReporteBalanceComprobacion(IdPlanCuenta, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new BalanceComprobacionViewModel
            {
                IdPlanCuenta = s.IdPlanCuenta,
                CODIGO = s.CODIGO,
                CUENTA = s.CUENTA,
                INICIAL = s.INICIAL,
                DEBITOS = s.DEBITOS,
                CREDITOS = s.CREDITOS,
                SALDOFINAL = s.SALDOFINAL,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EstadoFinancieroViewModel>> MostrarReporteEstadoResultados(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarReporteEstadoResultados(IdPlanCuenta, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new EstadoFinancieroViewModel
            {
                CODIGO = s.CODIGO,
                CUENTA = s.CUENTA,
                PARCIAL = s.PARCIAL,
                SUBTOTAL = s.SUBTOTAL,
                TOTAL = s.TOTAL,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<RegistroContableViewModel>> MostrarRegistroContable(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarRegistroContable(IdProcesoContable, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new RegistroContableViewModel
            {
                IdRegistro = s.IdRegistro,
                IdProcesoContable = s.IdProcesoContable,
                IdPlanCuenta = s.IdPlanCuenta,
                Debe = s.Debe,
                Haber = s.Haber,
                Codigo =s.Codigo ,
                Descripcion = s.Descripcion,
                FechaRegistro = s.FechaRegistro,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<RegistroContableViewModel>> MostrarMovimientoContable(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarMovimientoContable(IdProcesoContable, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new RegistroContableViewModel
            {
                IdRegistro = s.IdRegistro,
                IdProcesoContable = s.IdProcesoContable,
                IdPlanCuenta = s.IdPlanCuenta,
                TipoTransaccion = s.TipoTransaccion,
                Fecha = s.Fecha,
                Concepto = s.Concepto,
                Debe = s.Debe,
                Haber = s.Haber,
                Codigo = s.Codigo,
                Descripcion = s.Descripcion,
                FechaRegistro = s.FechaRegistro,
                Proceso = s.Proceso,
            });

        }

    }
}
