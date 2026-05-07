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
    public class ContratoController : Controller
    {
        private readonly ContratoRepository _repository;
        private readonly IConfiguration _config;
        public ContratoController(ContratoRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] ContratoViewModel model)
        {
            Contrato db = new Contrato();
            db.IdContrato = model.IdContrato;
            db.IdForeCast = model.IdForeCast;
            db.IdOrdenRecibida = model.IdOrdenRecibida;
            db.IdOrdenEnviada  = model.IdOrdenEnviada;
            db.IdMaterialR = model.IdMaterialR;
            db.IdMaterialE = model.IdMaterialE;
            db.IdFacturado = model.IdFacturado;
            db.IdCertificado = model.IdCertificado;
            db.IdPagado = model.IdPagado;
            db.FechaIngreso = model.FechaIngreso;
            db.NumContrato = model.NumContrato;
            db.NumOrden = model.NumOrden;
            db.FechaInicio = model.FechaInicio;
            db.FechaFinal = model.FechaFinal;
            db.Agencia = model.Agencia;
            db.NombreProyecto = model.NombreProyecto;
            db.Contacto = model.Contacto;
            db.FechaIngreso = model.FechaIngreso;
            db.ValorBruto = model.ValorBruto;
            db.ComiAgen = model.ComiAgen;
            db.ValorAgen = model.ValorAgen;
            db.Valor = model.Valor;
            db.ComiConex = model.ComiConex;
            db.ValorConex = model.ValorConex;
            db.RucVendedor = model.RucVendedor;
            db.ComiVendedor = model.ComiVendedor;
            db.Anunciante = model.Anunciante;
            db.Agencia = model.Agencia;
            db.Contacto = model.Contacto;
            db.Medio = model.Medio;
            db.Facturado = model.Facturado;
            db.FechaCobro = model.FechaCobro;
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
        public async Task<IEnumerable<Generica>> ActualizarDetalleContrato([FromBody] ActuDetalleContratoViewModel model)
        {
            ActuDetalleContrato db = new ActuDetalleContrato();
            db.IdDetalleContrato = model.IdDetalleContrato;          
            db.IdMaterialR = model.IdMaterialR;
            db.IdMaterialE = model.IdMaterialE;        
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.ActualizarDetalleContrato(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarArchivo([FromBody] CargarArchivoViewModel model)
        {
            CargarArchivo db = new CargarArchivo();
            db.IdContrato = model.IdContrato;
            db.IdForeCast = model.IdForeCast;
            db.nombre = model.nombre;
            db.nombreArchivo = model.nombreArchivo;
            db.base64textString = model.base64textString;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.CargaArchivo(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarArchivoRelacion([FromBody] CargarArchivoViewModel model)
        {
            CargarArchivo db = new CargarArchivo();
            db.IdContrato = model.IdContrato;
            db.IdForeCast = model.IdForeCast;
            db.nombre = model.nombre;
            db.nombreArchivo = model.nombreArchivo;
            db.base64textString = model.base64textString;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.CargaArchivoRelacion(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarArchivoForeCast([FromBody] CargarArchivoViewModel model)
        {
            CargarArchivo db = new CargarArchivo();
            db.IdContrato = model.IdContrato;
            db.IdForeCast = model.IdForeCast;
            db.nombre = model.nombre;
            db.nombreArchivo = model.nombreArchivo;
            db.base64textString = model.base64textString;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.CargaArchivoForeCast(db);

            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarArchivoImagen([FromBody] CargarArchivoViewModel model)
        {
            CargarArchivo db = new CargarArchivo();
            db.IdContrato = model.IdContrato;
            db.IdForeCast = model.IdForeCast;
            db.nombre = model.nombre;
            db.nombreArchivo = model.nombreArchivo;
            db.base64textString = model.base64textString;
            db.Estado = model.Estado;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.CargaArchivoImagen(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
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
        public async Task<IEnumerable<ForeAprobadasViewModel>> MostrarForeCastAprobadas(Int64 IdForeCast, Int64 IdContrato,Int64 IdEmpleado, string Perfil2, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarForeCastAprobadas(IdForeCast, IdContrato, IdEmpleado, Perfil2, Tipo);
            return response.Select(s => new ForeAprobadasViewModel
            {
                IdForeCast = s.IdForeCast,
                IdMedio = s.IdMedio,
                Agencia = s.Agencia,
                Cliente = s.Cliente,
                Medios = s.Medios,
                NombreProyecto = s.NombreProyecto.ToUpper(),
                Contacto = s.Contacto,               
                ValorTotalNeto = s.ValorTotalNeto,
                ValorTotalBruto = s.ValorTotalBruto,
                FechaInicioPauta = s.FechaInicioPauta,
                FechaFinalPauta = s.FechaFinalPauta,
                NumContrato = s.NumContrato,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ContratoViewModel>> MostrarContrato(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarContrato(IdForeCast, IdContrato, Tipo);
            return response.Select(s => new ContratoViewModel
            {
                IdContrato=s.IdContrato,
                IdForeCast = s.IdForeCast,
                IdOrdenRecibida = s.IdOrdenRecibida,
                IdOrdenEnviada = s.IdOrdenEnviada,
                IdMaterialR = s.IdMaterialR,
                IdMaterialE = s.IdMaterialE,
                IdFacturado = s.IdFacturado,
                IdCertificado = s.IdCertificado,
                IdPagado = s.IdPagado,
                FechaIngreso = s.FechaIngreso,
                NumContrato = s.NumContrato,
                NumOrden = s.NumOrden,
                FechaInicio = s.FechaInicio,
                FechaFinal = s.FechaFinal,
                ValorBruto = s.ValorBruto,
                ComiAgen = s.ComiAgen,
                ValorAgen = s.ValorAgen,
                Valor = s.Valor,
                ComiConex = s.ComiConex,
                ValorConex = s.ValorConex,
                RucVendedor = s.RucVendedor,
                ComiVendedor = s.ComiVendedor,
                Anunciante = s.Anunciante,
                Agencia = s.Agencia,
                Contacto = s.Contacto,
                Medio = s.Medio,
                Facturado = s.Facturado,
                FechaCobro = s.FechaCobro,
                Estado = s.Estado,
                Tipo = s.Tipo,
                NombreProyecto = s.NombreProyecto,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetalleContratoViewModel>> MostrarDetalleContrato(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarDetalleContrato(IdForeCast, IdContrato, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new DetalleContratoViewModel
            {
                IdDetalleContrato = s.IdDetalleContrato,
                IdMaterialE = s.IdMaterialE,
                IdMaterialR = s.IdMaterialR,
                Canal = s.Canal,
                Programa = s.Programa,
                Duracion = s.Duracion,
                Franja = s.Franja,
                Tarifa = s.Tarifa,
                TotalSegundos = s.TotalSegundos,
                ValorNegocio = s.ValorNegocio,
                Descripcion = s.Descripcion
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ComisionVendedorViewModel>> MostrarComisionVendedor(Int64 IdContrato, decimal ValorBruto, decimal ValorNeto, DateTime AnioProceso, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarComisionVendedor(IdContrato, ValorBruto, ValorNeto, AnioProceso, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new ComisionVendedorViewModel
            {
                Comision = s.Comision,
                Porcentaje = s.Porcentaje
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ContratoReporteViewModel>> MostrarContratoReporte(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarContratoReporte(IdForeCast, IdContrato, Tipo);
            return response.Select(s => new ContratoReporteViewModel
            {
                FechaInicio = s.FechaInicio.ToString("yyyy-MM-dd"),//,
                FechaFinal = s.FechaFinal.ToString("yyyy-MM-dd"),//,
                NumContrato = s.NumContrato,
                Anunciante = s.Anunciante,
                Agencia = s.Agencia,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetalleContratoExcelViewModel>> MostrarDetalleContratoExcel(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarDetalleContratoExcel(IdForeCast, IdContrato, Tipo);

            return response.Select(s => new DetalleContratoExcelViewModel
            {
                Canal = s.Canal,
                Programa = s.Programa,
                Detalle = s.Detalle,
                Duracion = s.Duracion,
                Derecho = s.Derecho,
                Franja = s.Franja,
                Tarifa = s.Tarifa,
                TotalSegundo = s.TotalSegundo,
                ValorNegocio = s.ValorNegocio,
                data_1 = s.data_1,
                data_2 = s.data_2,
                data_3 = s.data_3,
                data_4 = s.data_4,
                data_5 = s.data_5,
                data_6 = s.data_6,
                data_7 = s.data_7,
                data_8 = s.data_8,
                data_9 = s.data_9,
                data_10 = s.data_10,
                data_11 = s.data_11,
                data_12 = s.data_12,
                data_13 = s.data_13,
                data_14 = s.data_14,
                data_15 = s.data_15,
                data_16 = s.data_16,
                data_17 = s.data_17,
                data_18 = s.data_18,
                data_19 = s.data_19,
                data_20 = s.data_20,
                data_21 = s.data_21,
                data_22 = s.data_22,
                data_23 = s.data_23,
                data_24 = s.data_24,
                data_25 = s.data_25,
                data_26 = s.data_26,
                data_27 = s.data_27,
                data_28 = s.data_28,
                data_29 = s.data_29,
                data_30 = s.data_30,
                data_31 = s.data_31,
                Impacto = s.Impacto,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetalleContratoExcelViewModel>> MostrarDetallePautaExcel(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarDetallePautaExcel(IdForeCast, IdContrato, Tipo);

            return response.Select(s => new DetalleContratoExcelViewModel
            {
                Canal = s.Canal,
                Programa = s.Programa,
                Detalle = s.Detalle,
                Duracion = s.Duracion,
                Derecho = s.Derecho,
                Franja = s.Franja,
                Tarifa = s.Tarifa,
                TotalSegundo = s.TotalSegundo,
                ValorNegocio = s.ValorNegocio,
                data_1 = s.data_1,
                data_2 = s.data_2,
                data_3 = s.data_3,
                data_4 = s.data_4,
                data_5 = s.data_5,
                data_6 = s.data_6,
                data_7 = s.data_7,
                data_8 = s.data_8,
                data_9 = s.data_9,
                data_10 = s.data_10,
                data_11 = s.data_11,
                data_12 = s.data_12,
                data_13 = s.data_13,
                data_14 = s.data_14,
                data_15 = s.data_15,
                data_16 = s.data_16,
                data_17 = s.data_17,
                data_18 = s.data_18,
                data_19 = s.data_19,
                data_20 = s.data_20,
                data_21 = s.data_21,
                data_22 = s.data_22,
                data_23 = s.data_23,
                data_24 = s.data_24,
                data_25 = s.data_25,
                data_26 = s.data_26,
                data_27 = s.data_27,
                data_28 = s.data_28,
                data_29 = s.data_29,
                data_30 = s.data_30,
                data_31 = s.data_31,
                Impacto = s.Impacto,
            });

        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<FacturaContratoViewModel>> MostrarContratoPorFacturar(Int64 IdMedio, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarContratoPorFacturar(IdMedio, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new FacturaContratoViewModel
            {
                IdContrato = s.IdContrato,
                NumContrato = s.NumContrato,
                FechaInicio = s.FechaInicio,
                FechaFinal = s.FechaFinal,
                ValorBruto = s.ValorBruto,
                ComiAgen = s.ComiAgen,
                ValorAgen = s.ValorAgen,
                Valor = s.Valor,
                ValorConex = s.ValorConex,
                Anunciante = s.Anunciante,
                Mes = s.Mes,
                Anio = s.Anio,
                NumOrden = s.NumOrden,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> GuardarPagoContrato(string JsonDatos,string JsonDatosFinal, string Descripcion,string FechaEmision)
        {
            var responseResul = await _repository.InsertPagoContrato(JsonDatos, JsonDatosFinal, Descripcion, 1,1, FechaEmision);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> GuardaCobrarContrato(string JsonDatos, string JsonDatosFinal, string Descripcion,string TipoTransaccion,decimal ValorProceso)
        {
            var responseResul = await _repository.InsertCobroContrato(JsonDatos, JsonDatosFinal, Descripcion, TipoTransaccion.Trim(), ValorProceso, 1, 1);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> InsertAsientosContable(string JsonDatos, string JsonDatosFinal,string JsonValorReal, string Descripcion, string TipoTransaccion, decimal ValorProceso,string VariosProceso)
        {
            var responseResul = await _repository.InsertAsientosContable(JsonDatos, JsonDatosFinal,JsonValorReal, Descripcion, TipoTransaccion.Trim(), ValorProceso, 1, VariosProceso, 1);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Generica>> GuardarDetalleContrato(string JsonDatos, Int64 IdForeCast)
        {
            DatosExtra db = new DatosExtra();
            db.IdForeCast = IdForeCast;
            db.Estado = 1;
            db.Tipo = 1;
            var responseResul = await _repository.InsertDetalleContrato(db, JsonDatos);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<FacturaContratoViewModel>> MostrarFacturaPorCobrar(Int64 IdContrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarFacturaPorCobrar(IdContrato, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new FacturaContratoViewModel
            {
                ValorBruto = s.ValorBruto,
                ValorCobrar = s.ValorCobrar,
                ValorNeto = s.ValorNeto,
                ComisionVendedor =s.ComisionVendedor,
                ComisionPorcentaje = s.ComisionPorcentaje,
                NumDocumento = s.NumDocumento,
                FechaRegistro = s.FechaRegistro,
                EstadoPago =s.EstadoPago,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<FacturaContratoViewModel>> MostrarFacturaPorCobrarFecha(Int64 IdMedio,DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarFacturaPorCobrarFecha(IdMedio, FechaInicio, FechaFinal, Tipo);

            #region generar archivo
            #endregion

            return response.Select(s => new FacturaContratoViewModel
            {
                IdPagocontrato = s.IdPagocontrato,
                ValorBruto = s.ValorBruto,
                ValorCobrar = s.ValorCobrar,
                SubTotal = s.ValorCobrar,
                ValorRetencion = s.ValorRetencion,
                Saldo = s.Saldo,
                ValorNeto = s.ValorNeto,
                ComisionVendedor = s.ComisionVendedor,
                ComisionPorcentaje = s.ComisionPorcentaje,
                NumDocumento = s.NumDocumento,
                FechaRegistro = s.FechaRegistro,
                strFechaRegistro = s.FechaRegistro.ToString("dd/MM/yyyy"),
                EstadoPago = s.EstadoPago,
                NombreMedio = s.NombreMedio,
                Vendedor = s.Vendedor,
                ValorRenta = s.ValorRenta,
                ValorIva = s.ValorIva,
                Iva = s.Iva,
                Total = s.ValorCobrar + s.Iva,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CobrosViewModel>> MostrarCobros(Int64 IdPagocontrato, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarCobros(IdPagocontrato, Tipo);

            return response.Select(s => new CobrosViewModel
            {
                IdCobro = s.IdCobro,
                IdPagocontrato = s.IdPagocontrato,
                FormaPago =s.FormaPago,
                Valor = s.Valor,
                Saldo = s.Saldo,
                Total = s.Total,
                Observacion = s.Observacion,
                Descripcion = s.Descripcion,
                FechaPago =s.FechaPago 
            });

        }


    }
}
