using Conexion.AccesoDatos.Repository.Negocio;
using Conexion.Entidad.Negocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppConexion.Models;

namespace WebAppConexion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteGeneralController : Controller
    {
        private readonly ReporteGeneralRepository _repository;
        private readonly IConfiguration _config;
        public ReporteGeneralController(ReporteGeneralRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<ReporteGeneralViewModel>> MostrarReporteGeneral(Int64 IdMedio, int Anio, int Tipo)
        {
            var response = await _repository.GetByMostrarReporteGeneral(IdMedio, Anio, Tipo);
            return response.Select(s => new ReporteGeneralViewModel
            {
                Id = s.Id,
                Descripcion = s.Descripcion,
                NumeroConex = s.NumeroConex,
                Enero =s.Enero ,
                Febrero = s.Febrero,
                Abril = s.Abril,
                Marzo = s.Marzo,
                Mayo = s.Mayo,
                Junio = s.Junio,
                Julio = s.Julio,
                Agosto = s.Agosto,
                Septiembre = s.Septiembre,
                Octubre = s.Octubre,
                Noviembre = s.Noviembre,
                Diciembre = s.Diciembre,
                Total = s.Total,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ResumenCtasPagarCobrarViewModel>> ReporteDetallePagarCobrar(string NumDocumento, int Tipo)
        {
            var response = await _repository.GetByReporteDetallePagarCobrar(NumDocumento, Tipo);
            return response.Select(s => new ResumenCtasPagarCobrarViewModel
            {
                IdProcesoContable = s.IdProcesoContable,
                StrFechaRegistro = s.FechaRegistro.ToString("dd/MM/yyyy"),
                Descripcion = s.Descripcion,
                Valor = s.Valor,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ReporteGeneralViewModel>> MostrarReportePresupuesto(Int64 IdMedio, Int64 IdEmpleado, int Anio, int Tipo)
        {
            var response = await _repository.GetByMostrarReportePresupuesto(IdMedio,IdEmpleado, Anio, Tipo);
            return response.Select(s => new ReporteGeneralViewModel
            {
                Id = s.Id,
                Descripcion = s.Descripcion,
                Enero = s.Enero,
                Febrero = s.Febrero,
                Abril = s.Abril,
                Marzo = s.Marzo,
                Mayo = s.Mayo,
                Junio = s.Junio,
                Julio = s.Julio,
                Agosto = s.Agosto,
                Septiembre = s.Septiembre,
                Octubre = s.Octubre,
                Noviembre = s.Noviembre,
                Diciembre = s.Diciembre,
                Total = s.Total,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReportePresupuestoArchivoBase64(Int64 IdMedio, Int64 IdEmpleado, int Anio, int Tipo,string TipoDocumento)
        {
            var response = await _repository.GetByMostrarReportePresupuestoArchivoBase64(IdMedio, IdEmpleado, Anio, Tipo, TipoDocumento);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReporteEstadoCuentaArchivoBase64(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo,string TipoDocumento)
        {
            var response = await _repository.GetByMostrarReporteEstadoCuentaArchivoBase64(IdPlanCuenta, FechaInicio, FechaFinal, Tipo, TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> GenerarATS(DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response = await _repository.GetByGenerarATS(FechaInicio, FechaFinal, Tipo, TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarFacturaPorCobrarFechaArchivoBase64(Int64 IdMedio, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response = await _repository.GetByMostrarFacturaPorCobrarFechaArchivoBase64(IdMedio, FechaInicio, FechaFinal, Tipo, TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarFacturaPorPagarFechaArchivoBase64(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response = await _repository.GetByMostrarFacturaPorPagarFechaArchivoBase64(IdEmpleado, FechaInicio, FechaFinal, Tipo,TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReporteBalanceComprobacionArchivoBase64(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response = await _repository.GetByMostrarReporteBalanceComprobacionArchivoBase64(IdPlanCuenta, FechaInicio, FechaFinal, Tipo, TipoDocumento);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });
        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<ReporteDiarioViewModel>> MostrarReporteDiario(Int64 IdMedio, int Anio, int Tipo)
        {
            var response = await _repository.GetByMostrarReporteDiario(IdMedio, Anio, Tipo);
            return response.Select(s => new ReporteDiarioViewModel
            {
                FECHA = s.FECHA ,
                NUMCONTRAT = s.NUMCONTRAT,
                NUMERORDEN = s.NUMERORDEN,
                DESDE = s.DESDE,
                HASTA = s.HASTA,
                ANUNCIANTE = s.ANUNCIANTE,
                MEDIOPUBLI = s.MEDIOPUBLI,
                AGENCIA = s.AGENCIA,
                RUCVENDOR = s.RUCVENDOR,
                CANTSPOTS = s.CANTSPOTS,
                CANAL = s.CANAL,
                PROGRAMA = s.PROGRAMA,
                DERECHO = s.DERECHO,
                FRANJA = s.FRANJA,
                PARCIAL = s.PARCIAL,
                VALORBRUTO = s.VALORBRUTO,
                COMISAGEN = s.COMISAGEN,
                VALORAGEN = s.VALORAGEN,
                VALOR = s.VALOR,
                COMISCONEX = s.COMISCONEX,
                VALORCONEX = s.VALORCONEX,
                FACTURADOPORMEDIO = s.FACTURADOPORMEDIO,
                CERTIFICADOPORMEDIO = s.CERTIFICADOPORMEDIO,
                PAGADOALMEDIO = s.PAGADOALMEDIO,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ReporteRelacionMediosViewModel>> ReporteRelacionMedios(Int64 IdMedio, int Tipo)
        {
            var response = await _repository.GetByReporteRelacionMedios(IdMedio, Tipo);
            return response.Select(s => new ReporteRelacionMediosViewModel
            {
                IdRelacion = s.IdRelacion,
                Medios = s.Medios,
                Canal = s.Canal,
                Programa = s.Programa,
                Derecho = s.Derecho,
                Formato = s.Formato,
                Unidad = s.Unidad,
                Generico = s.Generico,

            });
        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<ReporteContactoViewModel>> MostrarReporteAnuncianteAgencia( int Tipo)
        {
            var response = await _repository.GetByMostrarReporteAnuncianteAgencia(Tipo);
            return response.Select(s => new ReporteContactoViewModel
            {
                ANUNCIANTE = s.ANUNCIANTE,
                CONTACTO_ANUNCIANTE = s.CONTACTO_ANUNCIANTE,
                TELEFONO_CONVENCIONAL = s.TELEFONO_CONVENCIONAL,
                E_MAIL_ANUNCIANTE = s.E_MAIL_ANUNCIANTE,
                AGENCIA = s.AGENCIA,
                CONTACTO_AGENCIA = s.CONTACTO_AGENCIA,
                TELEFONO_CONVENCIONAL_C = s.TELEFONO_CONVENCIONAL_C,
                E_MAIL_AGENCIA = s.E_MAIL_AGENCIA,

            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ReporteContactoViewModel>> MostrarReporteAnuncianteAgencia2(int Tipo)
        {
            var response = await _repository.GetByMostrarReporteAnuncianteAgencia(Tipo);
            return response.Select(s => new ReporteContactoViewModel
            {
                ANUNCIANTE = s.ANUNCIANTE,
                CONTACTO_ANUNCIANTE = s.CONTACTO_ANUNCIANTE,
                TELEFONO_CONVENCIONAL = s.TELEFONO_CONVENCIONAL,
                E_MAIL_ANUNCIANTE = s.E_MAIL_ANUNCIANTE,
                AGENCIA = s.AGENCIA,
                CONTACTO_AGENCIA = s.CONTACTO_AGENCIA,
                TELEFONO_CONVENCIONAL_C = s.TELEFONO_CONVENCIONAL_C,
                E_MAIL_AGENCIA = s.E_MAIL_AGENCIA,

            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReporteAnuncianteAgenciaArchivoBase64(int Tipo, string TipoDocumento)
        {
            //VerErrores("tipo: " + Tipo.ToString() +"TipoDocumento: " + TipoDocumento, "Log", "Detalle", 1);
            var response = await _repository.GetByMostrarReporteAnuncianteAgenciaArchivoBase64(Tipo, TipoDocumento);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> ReporteRelacionMediosArchivoBase64(Int64 IdMedio,int Tipo, string TipoDocumento)
        {

            var response = await _repository.GetReporteRelacionMediosArchivoBase64(IdMedio, Tipo, TipoDocumento);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReporteDiarioArchivoBase64(Int64 IdMedio, int Anio, int Tipo, string TipoDocumento)
        {
            var response = await _repository.GetByMostrarReporteDiarioArchivoBase64(IdMedio, Anio, Tipo, TipoDocumento);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });
        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarRolPagoArchivoBase64(Int64 IdContrato, Int64 IdForeCast, string TipoDocumento, string JsonRol, string NombreEmpleado)
        {
            var response = await _repository.GetByMostrarRolPagoArchivoBase64(IdContrato, IdForeCast, TipoDocumento, JsonRol, NombreEmpleado);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarMovimientoContableArchivoBase64(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response = await _repository.GetByMostrarMovimientoContableArchivoBase64(IdProcesoContable, FechaInicio, FechaFinal, Tipo, TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<RolPagosViewModel>> MostrarArchivoRolPago(string RuCedula, DateTime FechaPago)
        {
            var response = await _repository.GetByMostrarArchivoRolPago(RuCedula, FechaPago);
            return response.Select(s => new RolPagosViewModel
            {
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<SeguimientoForeCastViewModel>> ReporteSeguimiento(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByReporteSeguimiento(IdEmpleado, FechaInicio, FechaFinal, Tipo);
            return response.Select(s => new SeguimientoForeCastViewModel
            {
                IdForeCast = s.IdForeCast,
                NombresApellidos = s.NombresApellidos,
                FechaSeguimiento = s.FechaSeguimiento,
                Seguimientollamada = s.Seguimientollamada,
                SeguimientoVisita = s.SeguimientoVisita,
                Cliente = s.Cliente,
                Agencia = s.Agencia,
                Negocio = s.Negocio,
                ValorTotalBruto = s.ValorTotalBruto,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ComisionFInalViewModel>> MostrarReporteComision(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarReporteComision(IdEmpleado, FechaInicio, FechaFinal, Tipo);
            return response.Select(s => new ComisionFInalViewModel
            {
                NumContrato = s.NumContrato,
                Medios = s.Medios,
                Anunciante = s.Anunciante,
                FechaInicio = s.FechaInicio,
                FechaFinal = s.FechaFinal,
                NumDocumento = s.NumDocumento,
                EstadoPago = s.EstadoPago,
                NombresApellidos = s.NombresApellidos,
                ValorBruto = s.ValorBruto,
                ValorNeto =s.ValorNeto,
                Porcentaje = s.Porcentaje,
                Comision = s.Comision,
                StrFechaRegistro = s.FechaInicio.ToString("dd/MM/yyyy"),
            });

        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReporteComisionArchivoBase64(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string Perfil, string TipoDocumento)
        {
            var response = await _repository.GetByMostrarReporteComisionArchivoBase64(IdEmpleado, FechaInicio, FechaFinal, Tipo, Perfil, TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CobrosFacturasViewModel>> MostrarReporteFacturasCobradas(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarReporteFacturasCobradas(IdEmpleado, FechaInicio, FechaFinal, Tipo);
            return response.Select(s => new CobrosFacturasViewModel
            {
                Medios = s.Medios,
                Valor = s.Valor,
                NumDocumento = s.NumDocumento,
                EstadoPago = s.EstadoPago,
                Conex = s.Conex,
                ValorBruto = s.ValorBruto,
                ValorNeto = s.ValorNeto,
                Porcentaje = s.Porcentaje,
                Comision = s.Comision,
                Vendedor = s.Vendedor,
                EstadoComision = s.EstadoComision
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarReporteFacturasCobradasArchivoBase64(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo,  string TipoDocumento)
        {
            var response = await _repository.GetByMostrarReporteFacturasCobradasArchivoBase64(IdEmpleado, FechaInicio, FechaFinal, Tipo, TipoDocumento);

            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }




        #region VerErrores
        //public void VerErrores(string valor, string Carpeta, string rucEmpresa, int tipo)
        //{
        //    try
        //    {
        //        if (tipo == 1)
        //        {
        //            string fecha;
        //            fecha = DateTime.Now.ToString("dd-MM-yyyy");//DateTime.Now.ToShortDateString().Replace("/", "-");
        //            if (!Directory.Exists(@"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha))
        //            {
        //                Directory.CreateDirectory(@"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha);
        //            }

        //            string path = @"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha + "\\log.txt";
        //            TextWriter tw = new StreamWriter(path, true);
        //            tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ": " + valor);
        //            tw.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.EventLog.WriteEntry("Application", "Exception: " + ex.Message);
        //    }
        //}
        #endregion

    }
}
