using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Conexion.AccesoDatos.Repository.CArchivo;

namespace Conexion.AccesoDatos.Repository.Negocio
{
    public class ForeCastRepository
    {
        private readonly string _connectionString;

        public ForeCastRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }
        public async Task<IEnumerable<Generica>> Insert(ForeCast foreCast)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", foreCast.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", foreCast.IdCliente));
                    cmd.Parameters.Add(new SqlParameter("@IdMarca", foreCast.IdMarca));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", foreCast.IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdAgencia", foreCast.IdAgencia));
                    cmd.Parameters.Add(new SqlParameter("@IdCanal", foreCast.IdCanal));
                    cmd.Parameters.Add(new SqlParameter("@IdPrograma", foreCast.IdPrograma));
                    cmd.Parameters.Add(new SqlParameter("@IdDerecho", foreCast.IdDerecho));
                    cmd.Parameters.Add(new SqlParameter("@IdUnidad", foreCast.IdUnidad));
                    cmd.Parameters.Add(new SqlParameter("@IdNegocio", foreCast.IdNegocio));
                    cmd.Parameters.Add(new SqlParameter("@IdPropuesta", foreCast.IdPropuesta));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", foreCast.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@IdTipoRechazo", foreCast.IdTipoRechazo));
                    cmd.Parameters.Add(new SqlParameter("@IdContacto", foreCast.IdContacto));
                    cmd.Parameters.Add(new SqlParameter("@Agencia", foreCast.Agencia));
                    cmd.Parameters.Add(new SqlParameter("@NombreProyecto", foreCast.NombreProyecto));
                    cmd.Parameters.Add(new SqlParameter("@Contacto", foreCast.Contacto));
                    cmd.Parameters.Add(new SqlParameter("@FechaIngreso", foreCast.FechaIngreso));
                    cmd.Parameters.Add(new SqlParameter("@Cantidad", foreCast.Cantidad));
                    cmd.Parameters.Add(new SqlParameter("@Monto", foreCast.Monto));
                    cmd.Parameters.Add(new SqlParameter("@ValorTotalBruto", foreCast.ValorTotalBruto));
                    cmd.Parameters.Add(new SqlParameter("@PorcentajeAgencia", foreCast.PorcentajeAgencia));
                    cmd.Parameters.Add(new SqlParameter("@ValorAgencia", foreCast.ValorAgencia));
                    cmd.Parameters.Add(new SqlParameter("@ValorTotalNeto", foreCast.ValorTotalNeto));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicioPauta", foreCast.FechaInicioPauta));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinalPauta", foreCast.FechaFinalPauta));
                    cmd.Parameters.Add(new SqlParameter("@FechaTope", foreCast.FechaTope));
                    cmd.Parameters.Add(new SqlParameter("@FechaCierre", foreCast.FechaCierre));
                    cmd.Parameters.Add(new SqlParameter("@Seguimientollamada", foreCast.Seguimientollamada));
                    cmd.Parameters.Add(new SqlParameter("@FechaSeguimiento", foreCast.FechaSeguimiento));
                    cmd.Parameters.Add(new SqlParameter("@SeguimientoVisita", foreCast.SeguimientoVisita));
                    cmd.Parameters.Add(new SqlParameter("@FechaVisita", foreCast.FechaVisita));
                    cmd.Parameters.Add(new SqlParameter("@Usuario", foreCast.Usuario));
                    cmd.Parameters.Add(new SqlParameter("@MotivoRechazo", foreCast.MotivoRechazo));
                    cmd.Parameters.Add(new SqlParameter("@UltimaModificacion", foreCast.UltimaModificacion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", foreCast.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", foreCast.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    var response = new List<Generica>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGenerica(reader));
                        }
                    }


                    if (foreCast.IdForeCast != 0)
                    {
                        string resul = EnviarNotificacionOportunidad(foreCast.IdForeCast);
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Generica>> InsertDetalleForecast(DatosExtra foreCast,string json)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarDetalleForecastTable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", foreCast.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@Estado", foreCast.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", foreCast.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    var response = new List<Generica>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGenerica(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Generica>> InsertarModificarEliminarConex(NuevoConex conex)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarConex", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", conex.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@NumContrato", conex.NumContrato));
                    cmd.Parameters.Add(new SqlParameter("@Usuario", conex.Usuario));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", conex.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    var response = new List<Generica>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGenerica(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ForeCast>> GetByMostrarForeCast(Int64 IdForeCast,Int64 IdEmpleado, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ForeCast>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToForeCast(reader));
                        }
                    }

                    return response;
                }
            }
        }
        public async Task<IEnumerable<Generica>> CargaBase64(CargarArchivoBase64 cargar)
        {
            var response2 = new List<Generica>();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarRegitroArchivo", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRutaDoc", cargar.IdRutaDoc));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", cargar.IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdForecast", cargar.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@ArchivoBase64", cargar.ArchivoBase64));
                    cmd.Parameters.Add(new SqlParameter("@NombreArchivo", cargar.NombreArchivo));
                    cmd.Parameters.Add(new SqlParameter("@TipoDocumento", cargar.TipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("@Estado", cargar.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", cargar.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response2.Add(MapToGenerica(reader));
                        }
                    }
                }
            }

            return response2;
        }
        public async Task<IEnumerable<ForeCastReporte>> GetByMostrarForeCastReporte(Int64 IdForeCast, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ForeCastReporte>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToForeCastReporte(reader));
                        }
                    }

                    return response;
                }
            }
        }
        public async Task<IEnumerable<ForeCastIntermedio>> GetByMostrarForeCastIntermedio(Int64 IdForeCast, Int32 Tipo,string TipoDocumento)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ForeCastIntermedio>();

                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToForeCastIntermedio(reader));
                        }
                    }
                    var response2 = await GetByMostrarForeCastReporte(IdForeCast, 5);

                    GenerarReporte(response, response2, IdForeCast, TipoDocumento);
                    return response;
                }
            }
        }
        public async Task<IEnumerable<DetalleForeCast>> GetByMostrarDetalleForeCast(Int64 IdForeCast, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarDetalleForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<DetalleForeCast>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDetalleForeCast(reader));
                        }
                    }

                    return response;
                }
            }
        }
        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarArchivoBase64(Int64 IdContrato, Int64 IdForeCast,string TipoDocumento, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarArchivoBase64", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@TipoDocumento", TipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<CargarArchivoBase64>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToCombo(reader));
                        }
                    }

                    return response;
                }
            }
        }
        public async Task<IEnumerable<CargarArchivoBase64>> GetByTraerPlantilla(Int64 IdContrato, Int64 IdForeCast, string TipoDocumento)
        {

            var response = new List<CargarArchivoBase64>();
            CargarArchivoBase64 cargarArchivoBase = new CargarArchivoBase64();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, IdForeCast, TipoDocumento);
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            string rutaDocumento = "";
            rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
            byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumento);
            string archivoBase64 = Convert.ToBase64String(archivoBytes);
            cargarArchivoBase.ArchivoBase64 = archivoBase64;
            cargarArchivoBase.NombreArchivo = rutaDocumento;
            response.Add(cargarArchivoBase);

            return response;

        }
        public async Task<IEnumerable<ForeCastJson>> GetByMostrarForeCastJson(Int64 IdForeCast, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ForeCastJson>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToForeCastJson(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private DetalleForeCast MapToDetalleForeCast(SqlDataReader reader)
        {
            return new DetalleForeCast()
            {
                Canal = reader["Canal"].ToString(),
                Programa = reader["Programa"].ToString(),
                Franja = reader["Franja"].ToString(),
                Derecho = reader["Derecho"].ToString(),
                Formato = reader["Formato"].ToString(),
                Unidad = reader["Unidad"].ToString(),
                Cantidad = (decimal)reader["Cantidad"],
                Precio = (decimal)reader["Precio"],
                
            };
        }
        private ForeCast MapToForeCast(SqlDataReader reader)
        {
            return new ForeCast()
            {
                IdForeCast = (Int64)reader["IdForeCast"],
                IdCliente = (Int64)reader["IdCliente"],
                IdMarca = (Int64)reader["IdMarca"],
                IdMedio = (Int64)reader["IdMedio"],
                IdCanal = reader["IdCanal"].ToString(),
                IdPrograma = reader["IdPrograma"].ToString(),
                IdDerecho = reader["IdDerecho"].ToString(),
                IdUnidad = reader["IdCliente"].ToString(),
                IdNegocio = (Int64)reader["IdNegocio"],
                IdPropuesta = (Int64)reader["IdPropuesta"],
                IdEmpleado = (Int64)reader["IdEmpleado"],
                IdTipoRechazo = (Int64)reader["IdTipoRechazo"],
                Agencia = reader["Agencia"].ToString(),
                NombreProyecto = reader["NombreProyecto"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                FechaIngreso = (DateTime)reader["FechaIngreso"],
                Cantidad = (Int32)reader["Cantidad"],
                Monto = (decimal)reader["Monto"],
                ValorTotalBruto = (decimal)reader["ValorTotalBruto"],
                PorcentajeAgencia = (decimal)reader["PorcentajeAgencia"],
                ValorAgencia = (decimal)reader["ValorAgencia"],
                ValorTotalNeto = (decimal)reader["ValorTotalNeto"],
                FechaInicioPauta = (DateTime)reader["FechaInicioPauta"],
                FechaFinalPauta = (DateTime)reader["FechaFinalPauta"],
                FechaTope = (DateTime)reader["FechaTope"],
                FechaCierre = (DateTime)reader["FechaCierre"],
                Seguimientollamada = reader["Seguimientollamada"].ToString(),
                FechaSeguimiento = (DateTime)reader["FechaSeguimiento"],
                SeguimientoVisita = reader["SeguimientoVisita"].ToString(),
                FechaVisita = (DateTime)reader["FechaVisita"],
                Usuario = reader["Usuario"].ToString(),
                MotivoRechazo = reader["MotivoRechazo"].ToString(),
                UltimaModificacion = (DateTime)reader["UltimaModificacion"],
                Estado = (Int32)reader["Estado"],
                NombresApellidos = reader["NombresApellidos"].ToString(),
                Medios = reader["Medios"].ToString(),
                Numcontrato = (Int32)reader["Numcontrato"],
                NumPauta = (Int32)reader["NumPauta"],
                NumForeCast = (Int32)reader["NumForeCast"],
            };
        }
        private ForeCastReporte MapToForeCastReporte(SqlDataReader reader)
        {
            return new ForeCastReporte()
            {
                Cliente = reader["Cliente"].ToString(),
                NombreProyecto = reader["NombreProyecto"].ToString(),
                Medios = reader["Medios"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                Negocio = reader["Negocio"].ToString(),
                FechaInicioPauta = (DateTime)reader["FechaInicioPauta"],
                FechaFinalPauta = (DateTime)reader["FechaFinalPauta"],
                Cantidad = (Int32)reader["Cantidad"],
                Monto = (decimal)reader["Monto"],
                PorcentajeAgencia = (decimal)reader["PorcentajeAgencia"],
                Vendedor = reader["Vendedor"].ToString(),
            };
        }
        private ForeCastIntermedio MapToForeCastIntermedio(SqlDataReader reader)
        {
            return new ForeCastIntermedio()
            {
                Canal = reader["Canal"].ToString(),
                Programa = reader["Programa"].ToString(),
                Franja = reader["Franja"].ToString(),
                Derecho = reader["Derecho"].ToString(),
                Formato = reader["Formato"].ToString(),
                Unidad = reader["Unidad"].ToString(),
                Cantidad = (decimal)reader["Cantidad"],
                Precio = (decimal)reader["Precio"],
                TarifaSegundos = (decimal)reader["TarifaSegundos"],
                TotalSegundos = (decimal)reader["TotalSegundos"],
            };
        }
        private ForeCastJson MapToForeCastJson(SqlDataReader reader)
        {
            return new ForeCastJson()
            {
                IdForeCast = (Int64)reader["IdForeCast"],
                IdCliente = (Int64)reader["IdCliente"],
                IdMarca = (Int64)reader["IdMarca"],
                IdMedio = (Int64)reader["IdMedio"],
                IdAgencia = (Int64)reader["IdAgencia"],
                IdCanal = reader["IdCanal"].ToString(),
                IdPrograma = reader["IdPrograma"].ToString(),
                IdDerecho = reader["IdDerecho"].ToString(),
                IdUnidad = reader["IdCliente"].ToString(),
                IdNegocio = (Int64)reader["IdNegocio"],
                IdPropuesta = (Int64)reader["IdPropuesta"],
                IdEmpleado = (Int64)reader["IdEmpleado"],
                IdTipoRechazo = (Int64)reader["IdTipoRechazo"],
                IdContacto = (Int64)reader["IdContacto"],
                Agencia = reader["Agencia"].ToString(),
                NombreProyecto = reader["NombreProyecto"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                FechaIngreso = (DateTime)reader["FechaIngreso"],
                Cantidad = (Int32)reader["Cantidad"],
                Monto = (decimal)reader["Monto"],
                ValorTotalBruto = (decimal)reader["ValorTotalBruto"],
                PorcentajeAgencia = (decimal)reader["PorcentajeAgencia"],
                ValorAgencia = (decimal)reader["ValorAgencia"],
                ValorTotalNeto = (decimal)reader["ValorTotalNeto"],
                FechaInicioPauta = (DateTime)reader["FechaInicioPauta"],
                FechaFinalPauta = (DateTime)reader["FechaFinalPauta"],
                FechaTope = (DateTime)reader["FechaTope"],
                FechaCierre = (DateTime)reader["FechaCierre"],
                Seguimientollamada = reader["Seguimientollamada"].ToString(),
                FechaSeguimiento = (DateTime)reader["FechaSeguimiento"],
                SeguimientoVisita = reader["SeguimientoVisita"].ToString(),
                FechaVisita = (DateTime)reader["FechaVisita"],
                MotivoRechazo = reader["MotivoRechazo"].ToString(),
                Usuario = reader["Usuario"].ToString(),
                UltimaModificacion = (DateTime)reader["UltimaModificacion"],
                Estado = (Int32)reader["Estado"],

                JsonMedio = reader["JsonMedio"].ToString(),
                MedioSinJson = reader["MedioSinJson"].ToString(),

                JsonCanal = reader["JsonCanal"].ToString(),
                CanalSinJson = reader["CanalSinJson"].ToString(),

                JsonPrograma = reader["JsonPrograma"].ToString(),
                ProgramaSinJson = reader["ProgramaSinJson"].ToString(),

                JsonDerecho = reader["JsonDerecho"].ToString(),
                DerechoSinJson = reader["DerechoSinJson"].ToString(),

                JsonUnidad = reader["JsonUnidad"].ToString(),
                UnidadSinJson = reader["UnidadSinJson"].ToString(),
                NombresApellidos = reader["NombresApellidos"].ToString(),

                Numcontrato = (Int32)reader["Numcontrato"],
                NumPauta = (Int32)reader["NumPauta"],
                NumForeCast = (Int32)reader["NumForeCast"],
            };
        }
        private Generica MapToGenerica(SqlDataReader reader)
        {
            return new Generica()
            {
                valor1 = (Int16)reader["valor1"],
                valor2 = reader["valor2"].ToString()
            };
        }
        private CargarArchivoBase64 MapToCombo(SqlDataReader reader)
        {
            return new CargarArchivoBase64()
            {
                NombreArchivo = reader["NombreArchivo"].ToString(),
                ArchivoBase64 = reader["ArchivoBase64"].ToString()
            };
        }
        private string GenerarReporte(List<ForeCastIntermedio> detalles, IEnumerable<ForeCastReporte> reportes,Int64 IdForeCast,string TipoDocumento)
        {
            string Ruta = "";
            string Cliente = "";
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ","_").Replace(":","_");
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, IdForeCast, TipoDocumento);
            string rutaDocumento = "";
            string rutaDocumentoResul = "";
            if (TipoDocumento == "SPONSOR")
            {
                rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;// "F:\\PropuestaConexion\\SPOTS_AGREGADOS.xlsx";
                rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion; //"F:\\PropuestaConexion\\SPOTS_AGREGADOS_" + fecha + ".xlsx";
            }
            else if (TipoDocumento == "INTERMEDIO")
            {
                rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;// "F:\\PropuestaConexion\\INTERMEDIO.xlsx";
                rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion; //"F:\\PropuestaConexion\\INTERMEDIO_" + fecha + ".xlsx";
            }

            DateTime dateTime = DateTime.Now;
            using var wbook = new XLWorkbook(rutaDocumento);
            int cont = 16;
            var ws = wbook.Worksheet(1);
            foreach (ForeCastReporte s in reportes)
            {
                ws.Cell("C7").Value =s.Cliente;
                ws.Cell("C7").Style.Font.Bold = true;

                ws.Cell("C8").Value = s.Agencia;
                ws.Cell("C8").Style.Font.Bold = true;

                ws.Cell("C9").Value = s.FechaInicioPauta.ToString("yyyy-MM-dd") + " - " + s.FechaFinalPauta.ToString("yyyy-MM-dd");
                ws.Cell("C9").Style.Font.Bold = true;

                ws.Cell("C10").Value = dateTime.ToString("yyyy-MM-dd");
                ws.Cell("C10").Style.Font.Bold = true;

                int valorPor = Convert.ToInt32(s.PorcentajeAgencia);

                ws.Cell("G29").Value = valorPor;
                ws.Cell("G29").Style.Font.Bold = true;

                Cliente = s.Cliente;
            }

            foreach (ForeCastIntermedio s in detalles)
            {
                ws.Cell("B" + cont.ToString()).Value = s.Canal;
                ws.Cell("C" + cont.ToString()).Value = s.Programa;
                ws.Cell("D" + cont.ToString()).Value = s.Franja;
                ws.Cell("E" + cont.ToString()).Value = s.Derecho;
                if (TipoDocumento == "SPONSOR")
                {
                    ws.Cell("F" + cont.ToString()).Value = s.Formato;
                }
                else if (TipoDocumento == "INTERMEDIO")
                {
                    ws.Cell("F" + cont.ToString()).Value ="";
                }
                ws.Cell("G" + cont.ToString()).Value = s.Unidad;
                if (TipoDocumento == "SPONSOR")
                {
                    ws.Cell("H" + cont.ToString()).Value = s.Cantidad;
                }
                else if (TipoDocumento == "INTERMEDIO")
                {
                    ws.Cell("H" + cont.ToString()).Value = "";
                }
                ws.Cell("I" + cont.ToString()).Value = s.Precio;
                ws.Cell("J" + cont.ToString()).Value = s.TarifaSegundos;
                ws.Cell("K" + cont.ToString()).Value = s.TotalSegundos;
                cont++;
            }
            wbook.SaveAs(rutaDocumentoResul);

            byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
            string archivoBase64 = Convert.ToBase64String(archivoBytes);

            CargarArchivoBase64 cargar = new CargarArchivoBase64();
            cargar.IdRutaDoc = 0;
            cargar.IdForeCast = IdForeCast;
            if (TipoDocumento == "SPONSOR")
            {
                cargar.NombreArchivo = archivo.NombreArchivoSalida + "_" + Cliente + "_" + fecha + archivo.Extencion;// "SPOTS_AGREGADOS_" + fecha + ".xlsx";
            }
            else if (TipoDocumento == "INTERMEDIO")
            {
                cargar.NombreArchivo = archivo.NombreArchivoSalida + "_" + Cliente + "_" + fecha + archivo.Extencion;// "INTERMEDIO_" + fecha + ".xlsx";
            }

            cargar.IdContrato = 0;
            cargar.Tipo = 1;
            cargar.TipoDocumento = TipoDocumento;
            cargar.Estado = 1;
            cargar.ArchivoBase64 = archivoBase64;
            CargaBase64(cargar);

            return Ruta;
        }
        private string EnviarNotificacionOportunidad(Int64 IdForeCast)
        {
            string resultado = "";
            EntParametrosCorreo correo = new EntParametrosCorreo();
            DataTable table = new DataTable();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            string mensaje = "";
            table = cargar1.RetornarConfigSMTP(ref mensaje);
            if (table.Rows.Count > 0)
            {
                correo.smtpAddress = table.Rows[0]["HostSmtp"].ToString();
                correo.portNumber = Convert.ToInt32(table.Rows[0]["PuertoSmtp"].ToString());
                correo.emailFrom = table.Rows[0]["UsuarioSmtp"].ToString();
                correo.password = table.Rows[0]["ClaveSmtp"].ToString();
                correo.enableSSL = Convert.ToBoolean(table.Rows[0]["enableSSL"].ToString());

                DataTable noticorre = new DataTable();
                noticorre = cargar1.NotificacionOportunidad(IdForeCast, 0, ref mensaje);
                if (noticorre.Rows.Count > 0)
                {
                    foreach (DataRow data in noticorre.Rows)
                    {
                        bool temp = cargar1.EnviarCorreo(data["Correo"].ToString(), data["Titulo"].ToString(), data["Contenido"].ToString(), correo);
                    }
                }
            }
            
            return resultado;
        }

    }
}
