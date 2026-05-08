using Conexion.AccesoDatos.Repository.Negocio;
using Conexion.Entidad.Administracion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppConexion.Models;
using System.Data;
using Conexion.Entidad.Negocio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using System.IO;

namespace WebAppConexion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForeCastController : Controller
    {
        private readonly ForeCastRepository _repository;
        private readonly IConfiguration _config;
        public ForeCastController(ForeCastRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> Guardar([FromBody] ForeCastViewModel model)
        {
            ForeCast db = new ForeCast();
            db.IdForeCast = model.IdForeCast;
            db.IdCliente = model.IdCliente;
            db.IdMarca = model.IdMarca;
            db.IdMedio = model.IdMedio;
            db.IdAgencia = model.IdAgencia;
            db.IdCanal = model.IdCanal;
            db.IdPrograma = model.IdPrograma;
            db.IdDerecho = model.IdDerecho;
            db.IdUnidad = model.IdUnidad;
            db.IdNegocio = model.IdNegocio;
            db.IdPropuesta = model.IdPropuesta;
            db.IdEmpleado = model.IdEmpleado;
            db.IdTipoRechazo  = model.IdTipoRechazo;
            db.IdContacto = model.IdContacto;
            db.Agencia = model.Agencia;
            db.NombreProyecto = model.NombreProyecto;
            db.Contacto = model.Contacto;
            db.FechaIngreso = model.FechaIngreso;
            db.Cantidad = model.Cantidad;
            db.Monto = model.Monto;
            db.ValorTotalBruto = model.ValorTotalBruto;
            db.PorcentajeAgencia = model.PorcentajeAgencia;
            db.ValorAgencia = model.ValorAgencia;
            db.ValorTotalNeto = model.ValorTotalNeto;
            db.FechaInicioPauta = model.FechaInicioPauta;
            db.FechaFinalPauta = model.FechaFinalPauta;
            db.FechaTope = model.FechaTope;
            db.FechaCierre = model.FechaCierre;
            db.Seguimientollamada = model.Seguimientollamada;
            db.FechaSeguimiento = model.FechaSeguimiento;
            db.SeguimientoVisita = model.SeguimientoVisita;
            db.FechaVisita = model.FechaVisita;
            db.Usuario = model.Usuario;
            db.MotivoRechazo = model.MotivoRechazo;
            db.UltimaModificacion = model.UltimaModificacion;
            db.TotalNegocio = model.TotalNegocio;
            db.TotalSegundos = model.TotalSegundos;
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
        public async Task<IEnumerable<Generica>> InsertarModificarEliminarConex([FromBody] NuevoConexViewModel model)
        {
            NuevoConex db = new NuevoConex();
            db.IdForeCast = model.IdForeCast;
            db.NumContrato = model.NumContrato;
            db.Usuario = model.Usuario;
            db.NumConex = model.NumConex;
            db.Tipo = model.Tipo;

            var responseResul = await _repository.InsertarModificarEliminarConex(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> InsertarModificarEliminarTotalNegSegun([FromBody] TotalNegSegunViewModel model)
        {
            TotalNegSegun db = new TotalNegSegun();
            db.IdForeCast = model.IdForeCast;
            db.TotalNegocio = model.TotalNegocio;
            db.TotalSegundos = model.TotalSegundos;

            var responseResul = await _repository.InsertarModificarEliminarTotalNegSegun(db);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }


        [HttpPost("[action]")]
        public async Task<IEnumerable<Generica>> GuardarDetalleForeCast([FromBody] GuardarDetalleForeCastDto dto)
        {
            DatosExtra db = new DatosExtra();
            db.IdForeCast = dto.IdForeCast;           
            db.Estado = 1;
            db.Tipo = 1;
            var responseResul = await _repository.InsertDetalleForecast(db, dto.JsonDatos);
            return responseResul.Select(s => new Generica
            {
                valor1 = s.valor1,
                valor2 = s.valor2
            });
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ForeCastViewModel>> MostrarForeCast(Int64 IdForeCast,Int64 IdEmpleado, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarForeCast(IdForeCast, IdEmpleado, Tipo);
            return response.Select(s => new ForeCastViewModel
            {
                IdForeCast = s.IdForeCast,
                IdCliente = s.IdCliente,
                IdMarca = s.IdMarca,
                IdMedio = s.IdMedio,
                IdCanal = s.IdCanal,
                IdPrograma =s.IdPrograma,
                IdDerecho=s.IdDerecho ,
                IdUnidad=s.IdUnidad,
                IdNegocio =s.IdNegocio,
                IdPropuesta=s.IdPropuesta,
                IdEmpleado =s.IdEmpleado,
                IdTipoRechazo = s.IdTipoRechazo,
                Agencia =s.Agencia, 
                NombreProyecto = s.NombreProyecto.ToUpper(),
                Contacto = s.Contacto,
                FechaIngreso = s.FechaIngreso,
                Cantidad = s.Cantidad,
                Monto = s.Monto,
                ValorTotalBruto = s.ValorTotalBruto,
                PorcentajeAgencia = s.PorcentajeAgencia,
                ValorAgencia = s.ValorAgencia,
                ValorTotalNeto = s.ValorTotalNeto,
                FechaInicioPauta=s.FechaInicioPauta,
                FechaFinalPauta=s.FechaFinalPauta ,
                FechaTope =s.FechaTope ,
                FechaCierre =s.FechaCierre ,
                Seguimientollamada =s.Seguimientollamada,
                FechaSeguimiento =s.FechaSeguimiento ,
                SeguimientoVisita =s.SeguimientoVisita,
                FechaVisita =s.FechaVisita ,
                Usuario =s.Usuario,
                MotivoRechazo=s.MotivoRechazo,
                UltimaModificacion =s.UltimaModificacion,
                Estado = s.Estado,
                NombresApellidos=s.NombresApellidos, 
                Medios=s.Medios,
                Numcontrato = s.Numcontrato,
                NumPauta = s.NumPauta,
                NumForeCast = s.NumForeCast

            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ForeCastReporteViewModel>> MostrarForeCastReporte(Int64 IdForeCast, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarForeCastReporte(IdForeCast, Tipo);

            //HtmlToPdf pdf = new HtmlToPdf();
            //string html="";
            //foreach (ForeCast item in response)
            //{
            //    html = item.NombreProyecto;
            //}
            //PdfDocument pdfDocument = pdf.ConvertHtmlString(html);
            ////string _FileName = "F:\\archivo.pdf";
            //byte[] pdfs1 = pdfDocument.Save();
            
            // Abrir archivo para leer
            //FileStream _FileStream = new FileStream(_FileName, FileMode.Create, FileAccess.Write);
            // Escribe un bloque de bytes en esta secuencia utilizando datos de una matriz de bytes.
            //_FileStream.Write(pdfs, 0, pdfs.Length);
            // Cerrar secuencia de archivos
            //_FileStream.Close();
            //_FileStream.Flush();
            //File(pdfs, "F:\\", "archivo.pdf");

            return response.Select(s => new ForeCastReporteViewModel
            {
                Cliente = s.Cliente,
                NombreProyecto = s.NombreProyecto,
                Medios = s.Medios,
                Agencia = s.Agencia,
                Contacto = s.Contacto,
                Negocio = s.Negocio,
                FechaInicioPauta = s.FechaInicioPauta.ToString("yyyy-MM-dd"),// Convert.ToDateTime(s.FechaInicioPauta,""),
                FechaFinalPauta = s.FechaFinalPauta.ToString("yyyy-MM-dd"),
                Cantidad = s.Cantidad,
                Monto = s.Monto,
                Vendedor = s.Vendedor,
                //Pdfs = pdfs1
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ForeCastIntermedioViewModel>> MostrarForeCastIntermedio(Int64 IdForeCast, Int32 Tipo,string TipoDocumento, Int32 TipoProceso)
        {
            var response = await _repository.GetByMostrarForeCastIntermedio(IdForeCast, Tipo, TipoDocumento, TipoProceso);

            return response.Select(s => new ForeCastIntermedioViewModel
            {
                Canal = s.Canal,
                Programa = s.Programa,
                Franja = s.Franja,
                Derecho = s.Derecho,
                Formato = s.Formato,
                Unidad = s.Unidad,
                Cantidad = s.Cantidad,
                Precio = s.Precio,
                TarifaSegundos = s.TarifaSegundos,
                TotalSegundos = s.TotalSegundos,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetalleForeCastViewModel>> MostrarDetalleForeCast(Int64 IdForeCast, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarDetalleForeCast(IdForeCast, Tipo);
            return response.Select(s => new DetalleForeCastViewModel
            {              
                Canal = s.Canal,
                Programa = s.Programa,
                Franja=s.Franja,
                Derecho=s.Derecho,
                Formato=s.Formato,
                Unidad=s.Unidad,
                Cantidad=s.Cantidad,
                Precio=s.Precio

            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarArchivoBase64(Int64 IdContrato, Int64 IdForeCast, string TipoDocumento, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarArchivoBase64(IdContrato,IdForeCast, TipoDocumento, Tipo);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CargarArchivoBase64ViewModel>> MostrarPlantilla(Int64 IdContrato, Int64 IdForeCast, string TipoDocumento)
        {
            var response = await _repository.GetByTraerPlantilla(IdContrato, IdForeCast, TipoDocumento);
            return response.Select(s => new CargarArchivoBase64ViewModel
            {
                NombreArchivo = s.NombreArchivo,
                ArchivoBase64 = s.ArchivoBase64,
            });

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ForeCastJsonViewModel>> MostrarForeCastJson(Int64 IdForeCast, Int32 Tipo)
        {
            var response = await _repository.GetByMostrarForeCastJson(IdForeCast, Tipo);
            return response.Select(s => new ForeCastJsonViewModel
            {
                IdForeCast = s.IdForeCast,
                IdCliente = s.IdCliente,
                IdMarca = s.IdMarca,
                IdMedio = s.IdMedio,
                IdAgencia=s.IdAgencia,
                IdCanal = s.IdCanal,
                IdPrograma = s.IdPrograma,
                IdDerecho = s.IdDerecho,
                IdUnidad = s.IdUnidad,
                IdNegocio = s.IdNegocio,
                IdPropuesta = s.IdPropuesta,
                IdEmpleado = s.IdEmpleado,
                IdTipoRechazo = s.IdTipoRechazo,
                IdContacto =s.IdContacto,
                Agencia = s.Agencia,
                NombreProyecto = s.NombreProyecto.ToUpper(),
                Contacto = s.Contacto,
                FechaIngreso = s.FechaIngreso,
                Cantidad = s.Cantidad,
                Monto = s.Monto,
                ValorTotalBruto = s.ValorTotalBruto,
                PorcentajeAgencia = s.PorcentajeAgencia,
                ValorAgencia = s.ValorAgencia,
                ValorTotalNeto = s.ValorTotalNeto,
                FechaInicioPauta = s.FechaInicioPauta,
                FechaFinalPauta = s.FechaFinalPauta,
                FechaTope = s.FechaTope,
                FechaCierre = s.FechaCierre,
                Seguimientollamada = s.Seguimientollamada,
                FechaSeguimiento = s.FechaSeguimiento,
                SeguimientoVisita = s.SeguimientoVisita,
                FechaVisita = s.FechaVisita,
                Usuario = s.Usuario,
                MotivoRechazo = s.MotivoRechazo,
                UltimaModificacion = s.UltimaModificacion,
                Estado = s.Estado,

                JsonMedio =s.JsonMedio,
                MedioSinJson=s.MedioSinJson,

                JsonCanal = s.JsonCanal,
                CanalSinJson = s.CanalSinJson,

                JsonPrograma = s.JsonPrograma,
                ProgramaSinJson = s.ProgramaSinJson,

                JsonDerecho = s.JsonDerecho,
                DerechoSinJson = s.DerechoSinJson,

                JsonUnidad = s.JsonUnidad,
                UnidadSinJson = s.UnidadSinJson,
                NombresApellidos =s.NombresApellidos,
                Numcontrato = s.Numcontrato,
                NumPauta = s.NumPauta,
                NumForeCast = s.NumForeCast,
                TotalNegocio = s.TotalNegocio,
                TotalSegundos = s.TotalSegundos,
            });

        }

        // how to convert json to datatable in asp.net c#
        protected DataTable ConvertJsonToDatatable(string jsonString)
        {
            DataTable dt = new DataTable();
            //strip out bad characters
            string[] jsonParts = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            //hold column names
            List<string> dtColumns = new List<string>();
            //get columns
            foreach (string jp in jsonParts)
            {
                //only loop thru once to get column names
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1);
                        string v = rowData.Substring(idx + 1);
                        if (!dtColumns.Contains(n))
                        {
                            dtColumns.Add(n.Replace("\"", ""));
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", rowData));
                    }
                }
                break; // TODO: might not be correct. Was : Exit For
            }
            //build dt
            foreach (string c in dtColumns)
            {
                dt.Columns.Add(c);
            }
            //get table data
            foreach (string jp in jsonParts)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[n] = v;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

    }
}
