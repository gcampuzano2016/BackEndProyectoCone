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
    public class CuentasPorPagarController : Controller
    {
        private readonly CuentasPorPagarRepository _repository;
        private readonly IConfiguration _config;
        public CuentasPorPagarController(CuentasPorPagarRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CuentasPorPagarViewModel>> MostrarDocumentoSRI(string AutorisacionSri)
        {
            var response = await _repository.GetByMostrarDocumentoSRI(AutorisacionSri);
            return response.Select(s => new CuentasPorPagarViewModel
            {
                IdCuentaPorPagar = s.IdCuentaPorPagar,
                FechaAutorizacion = s.FechaAutorizacion,
                FechaRegistro = s.FechaRegistro,
                EstadoServicio = s.EstadoServicio,
                RuCedula = s.RuCedula,
                RazonSocial = s.RazonSocial,
                Email =s.Email,
                AutorizacionSri =s.AutorizacionSri,
                NumDocumento = s.NumDocumento,
                PlazoVencimiento = s.PlazoVencimiento,
                CompraTarifa0 = s.CompraTarifa0,
                CompraTarifa12 = s.CompraTarifa12,
                Iva = s.Iva ,
                ValorTotal = s.ValorTotal 
            });
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] CuentasPorPagarViewModel model)
        {
            CuentasPorPagar db = new CuentasPorPagar();
            db.IdCuentaPorPagar = model.IdCuentaPorPagar;
            db.FechaAutorizacion = model.FechaAutorizacion;
            db.FechaRegistro = model.FechaRegistro;
            db.EstadoServicio = model.EstadoServicio;
            db.RuCedula = model.RuCedula;
            db.RazonSocial = model.RazonSocial;
            db.Email = model.Email;
            db.AutorizacionSri = model.AutorizacionSri;
            db.NumDocumento = model.NumDocumento;
            db.PlazoVencimiento = model.PlazoVencimiento;
            db.CompraTarifa0 = model.CompraTarifa0;
            db.CompraTarifa12 = model.CompraTarifa12;
            db.Iva = model.Iva;
            db.ValorTotal = model.ValorTotal;
            db.TipoDocumento = model.TipoDocumento;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;
            db.jsonContable = model.jsonContable;


            var responseResul = await _repository.Insert(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CuentasPorPagarViewModel>> MostrarFacturaPorPagarFecha(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarFacturaPorPagarFecha(IdEmpleado, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new CuentasPorPagarViewModel
            {
                Id = s.Id,
                IdCuentaPorPagar = s.IdCuentaPorPagar,
                FechaAutorizacion = s.FechaAutorizacion,
                FechaRegistro = s.FechaRegistro,
                strFechaRegistro = s.FechaRegistro.ToString("dd/MM/yyyy"),
                EstadoServicio = s.EstadoServicio,
                RuCedula = s.RuCedula,
                RazonSocial = s.RazonSocial,
                Email = s.Email,
                AutorizacionSri = s.AutorizacionSri,
                EstadoPago = s.EstadoPago,
                NumDocumento = s.NumDocumento,
                PlazoVencimiento = s.PlazoVencimiento,
                CompraTarifa0 = s.CompraTarifa0,
                CompraTarifa12 = s.CompraTarifa12,
                Iva = s.Iva,
                ValorTotal = s.ValorTotal,
                PorRegistrar =s.PorRegistrar,
                Saldo = s.Saldo,
                TipoDocumento = s.TipoDocumento ,
                IdProveedor = s.IdProveedor,
                RutaDocumento = s.RutaDocumento,
                stringArchivo64 =s.stringArchivo64
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PagosViewModel>> MostrarPagos(Int64 IdCuentaPorPagar, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarPagos(IdCuentaPorPagar, Tipo);

            return response.Select(s => new PagosViewModel
            {
                IdPago = s.IdPago,
                IdCuentaPorPagar = s.IdCuentaPorPagar,
                FormaPago = s.FormaPago,
                Valor = s.Valor,
                Saldo = s.Saldo,
                Total = s.Total,
                Observacion = s.Observacion,
                Descripcion = s.Descripcion,
                FechaPago = s.FechaPago
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PlantillaCuentasViewModel>> MostrarPlantillaCuentaPorPagar(Int64 IdCuentaPorPagar, string Proveedor, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarPlantillaCuentaPorPagar(IdCuentaPorPagar, Proveedor, Tipo);

            return response.Select(s => new PlantillaCuentasViewModel
            {
                IdPlanCuenta = s.IdPlanCuenta,
                Descripcion = s.Descripcion,
                DescripcionMovimiento =s.DescripcionMovimiento,
                Debe = s.Debe,
                Haber = s.Haber,
                tipo = s.tipo,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PlantillaCuentasViewModel>> BusquedaCuentasPorPagar(Int64 IdCuentaPorPagar, Int32 Tipo)
        {
            var response = await _repository.GetByBusquedaCuentasPorPagar(IdCuentaPorPagar, Tipo);

            return response.Select(s => new PlantillaCuentasViewModel
            {
                IdPlanCuenta = s.IdPlanCuenta,
                Descripcion = s.Descripcion,
                DescripcionMovimiento = s.DescripcionMovimiento,
                Debe = s.Debe,
                Haber = s.Haber,
                tipo = s.tipo,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<PlantillaCuentasViewModel>> DescripcionCuentasPorPagar(Int64 IdCuentaPorPagar, Int32 Tipo)
        {
            var response = await _repository.GetByDescripcionCuentasPorPagar(IdCuentaPorPagar, Tipo);

            return response.Select(s => new PlantillaCuentasViewModel
            {
                Descripcion = s.Descripcion,
                FechaRegistro = s.FechaRegistro,
            });

        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarArchivoGeneral([FromBody] CargarArchivoGeneralViewModel model)
        {
            CargarArchivoBase64 db = new CargarArchivoBase64();
            db.IdRutaDoc = model.IdRutaDoc;
            db.IdContrato = model.IdContrato;
            db.IdForeCast = model.IdForeCast;
            db.NombreArchivo = model.NombreArchivo;
            db.ArchivoBase64 = model.ArchivoBase64;
            db.TipoDocumento = model.TipoDocumento;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.CargaArchivoGeneral(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> ModificarAsientoContable(Int64 IdCuentaPorPagar, string JsonDatosFinal, int Tipo)
        {
            var responseResul = await _repository.ModificarAsientoContable(IdCuentaPorPagar, JsonDatosFinal, Tipo);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> ModificarAsientoContableDescripcion(Int64 IdCuentaPorPagar, string JsonDatosFinal, DateTime FechaPago, string Concepto, int Tipo)
        {
            var responseResul = await _repository.ModificarAsientoContableDescripcion(IdCuentaPorPagar, JsonDatosFinal, FechaPago, Concepto, Tipo);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> InsertarAsientoContable(string TipoTransaccion, string Descripcion, string JsonDatosFinal, decimal ValorProceso, int Tipo)
        {
            var responseResul = await _repository.InsertarAsientoContable(TipoTransaccion, Descripcion, JsonDatosFinal, ValorProceso, Tipo);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

    }
}
