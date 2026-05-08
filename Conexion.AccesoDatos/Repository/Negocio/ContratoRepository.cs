using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Globalization;
using Conexion.AccesoDatos.Repository.CArchivo;
using System.IO;

namespace Conexion.AccesoDatos.Repository.Negocio
{
    public class ContratoRepository
    {
        private readonly string _connectionString;

        public ContratoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> CargaArchivo(CargarArchivo cargar)
        {
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "",0);
            string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
            var response2 = new List<Generica>();
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarCargarArchivoSpots", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCargarArchivo", cargar.IdCargarArchivo));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", cargar.IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", cargar.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@RutaArchivo", cargar.RutaArchivo));                  
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

            var  response = new List<Generica>();
            byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
            System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
            CargarXLSX cargarXLSX = new CargarXLSX(_connectionString);
            string resul = "";
            EntRespuesta Respuesta = new EntRespuesta();
            Generica generica = new Generica();
            if (cargar.IdContrato != 0)
            {
                Respuesta = cargarXLSX.SubirArchivoContrato(cargar);
                generica.valor1 = Convert.ToInt32(Respuesta.estado);
                generica.valor2 = Respuesta.mensaje;
                response.Add(generica);
            }
            else if (cargar.IdForeCast != 0)
            {
                resul = cargarXLSX.SubirArchivo(cargar);
                generica.valor1 = 1;
                generica.valor2 = "DOCUMENTO CARGADO..";
                response.Add(generica);
            }
            return  response;
        }

        public async Task<IEnumerable<Generica>> CargaArchivoPro(CargarArchivo cargar)
        {
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "",0);
            string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
            var response2 = new List<Generica>();
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;

            var response = new List<Generica>();
            byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
            System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
            CargarXLSX cargarXLSX = new CargarXLSX(_connectionString);
            string resul = "";
            EntRespuesta Respuesta = new EntRespuesta();
            string dataContrato = "";
            Generica generica = new Generica();
            if (cargar.IdContrato != 0)
            {
                dataContrato = cargarXLSX.SubirArchivoDetalleContrato(cargar.RutaArchivo);
                if(dataContrato != "")
                {
                    var response3 = await InsertDetalleContratoPro(cargar, dataContrato);
                    foreach (Generica generica1 in response3)
                    {
                        generica.valor1 = generica1.valor1;
                        generica.valor2 = generica1.valor2;
                        response.Add(generica);
                    }
                    response.Add(generica);
                }
                //generica.valor1 = Convert.ToInt32(Respuesta.estado);
                //generica.valor2 = Respuesta.mensaje;
                //response.Add(generica);
            }
            return response;
        }

        public async Task<IEnumerable<Generica>> CargaArchivoRelacion(CargarArchivo cargar)
        {
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "",0);
            string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
            string rutaFinal = "";
            var response2 = new List<Generica>();
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;


            var response = new List<Generica>();
            byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
            System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
            rutaFinal = ruta + cargar.nombreArchivo;
            CargarXLSX cargarXLSX = new CargarXLSX(_connectionString);
            string resul = "";
            resul = cargarXLSX.SubirArchivoMedios(rutaFinal);
            Generica generica = new Generica();
            generica.valor1 = 1;
            generica.valor2 = "DOCUMENTO CARGADO..";
            response.Add(generica);

            return response;
        }

        public async Task<IEnumerable<Generica>> CargaArchivoForeCast(CargarArchivo cargar)
        {
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "",0);
            string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
            string rutaFinal = "";
            var response2 = new List<Generica>();
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;


            var response = new List<Generica>();
            byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
            System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
            rutaFinal = ruta + cargar.nombreArchivo;
            CargarXLSX cargarXLSX = new CargarXLSX(_connectionString);
            string resul = "";
            resul = cargarXLSX.SubirArchivoForeCast(rutaFinal);
            DatosExtra datosExtra = new DatosExtra();
            datosExtra.IdForeCast = cargar.IdForeCast;
            datosExtra.Tipo = 1;
            datosExtra.Estado = 1;
            var response3 = await InsertDetalleForecastPro(datosExtra,resul);
            Generica generica = new Generica();
            generica.valor1 = 1;
            generica.valor2 = "DOCUMENTO CARGADO..";
            response.Add(generica);

            return response;
        }

        public async Task<IEnumerable<Generica>> CargaArchivoValidarRelacionMedios(CargarArchivo cargar)
        {
            var response = new List<Generica>();
            Generica generica = new Generica();
            try
            {
                PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
                CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "", 0);
                string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
                string rutaFinal = "";
                var response2 = new List<Generica>();
                cargar.RutaArchivo = ruta + cargar.nombreArchivo;

                byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
                System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
                rutaFinal = ruta + cargar.nombreArchivo;
                CargarXLSX cargarXLSX = new CargarXLSX(_connectionString);
                string resul = "";
                resul = cargarXLSX.SubirArchivoForeCast(rutaFinal);
                DatosExtra datosExtra = new DatosExtra();
                datosExtra.IdForeCast = cargar.IdForeCast;
                datosExtra.Tipo = 1;
                datosExtra.Estado = 1;
                var response3 = await ValidarRelacionDeMedios(datosExtra, resul);

                foreach (Generica generica1 in response3)
                {
                    generica.valor1 = 1;
                    generica.valor2 = generica1.valor2;
                    response.Add(generica);
                }
                return response;
            }
            catch (Exception ex)
            {
                generica.valor1 = 2;
                generica.valor2 = ex.Message.ToString();
                response.Add(generica);
                VerErrores(ex.Message.ToString(), "error", "", 1);
            }
            return response;
        }


        public async Task<IEnumerable<Generica>> CargaArchivoValidarRelacionMapaPauta(CargarArchivo cargar)
        {
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "",0);
            string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
            string rutaFinal = "";
            var response2 = new List<Generica>();
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;


            var response = new List<Generica>();
            byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
            System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
            rutaFinal = ruta + cargar.nombreArchivo;
            CargarXLSX cargarXLSX = new CargarXLSX(_connectionString);
            string resul = "";
            resul = cargarXLSX.SubirArchivoMapaPautaValidar(rutaFinal);
            DatosExtra datosExtra = new DatosExtra();
            datosExtra.IdForeCast = cargar.IdForeCast;
            datosExtra.Tipo = 1;
            datosExtra.Estado = 1;
            var response3 = await ValidarRelacionDeMapaPauta(datosExtra, resul);
            Generica generica = new Generica();
            foreach (Generica generica1 in response3)
            {
                generica.valor1 = 1;
                generica.valor2 = generica1.valor2;
                response.Add(generica);
            }
            return response;
        }

        public async Task<IEnumerable<Generica>> InsertDetalleForecast(DatosExtra foreCast, string json)
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

        public async Task<IEnumerable<Generica>> InsertDetalleForecastPro(DatosExtra foreCast, string json)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarDetalleForecastTablePro", sql))
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

        public async Task<IEnumerable<Generica>> ValidarRelacionDeMedios(DatosExtra foreCast, string json)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ValidarRelacionesMedio", sql))
                {
                    cmd.CommandTimeout = 60 * 5;
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

        public async Task<IEnumerable<Generica>> ValidarRelacionDeMapaPauta(DatosExtra foreCast, string json)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ValidarRelacionDeMapaPauta", sql))
                {
                    cmd.CommandTimeout = 60 * 5;
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

        public async Task<IEnumerable<Generica>> InsertDetalleContratoPro(CargarArchivo cargarArchivo, string Json)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarDetalleContratoPro", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", cargarArchivo.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", cargarArchivo.IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Json", Json));
                    cmd.Parameters.Add(new SqlParameter("@Estado", cargarArchivo.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", cargarArchivo.Tipo));
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

        public async Task<IEnumerable<Generica>> CargaArchivoImagen(CargarArchivo cargar)
        {
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "GUARDAR IMAGEN",0);
            string ruta = archivo.RutaArchivo; //"E:\\SubirArchivo\\";
            string rutaFinal = "";
            var response2 = new List<Generica>();
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;


            var response = new List<Generica>();
            byte[] archivoBytes = Convert.FromBase64String(cargar.base64textString);
            System.IO.File.WriteAllBytes(ruta + cargar.nombreArchivo, archivoBytes);
            rutaFinal = ruta + cargar.nombreArchivo;

            #region Actualizar Empleado
            Empleado db = new Empleado();
            db.IdEmpleado = cargar.IdContrato;
            db.IdPerfil = 0;
            db.NombresApellidos = "";
            db.Rucedula = "";
            db.Sueldo = 0;
            db.Ingreso = Convert .ToDateTime("1900-01-01");
            db.Clase ="";
            db.Direccion = "";
            db.Telefono = "";
            db.Regimen = "";
            db.Correo = "";
            db.Rol = "";
            db.FondoReserva ="";
            db.Estado = 1;
            db.RutaImagen = cargar.nombreArchivo;
            db.Tipo = 4;

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarEmpleado", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", db.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@IdPerfil", db.IdPerfil));
                    cmd.Parameters.Add(new SqlParameter("@NombresApellidos", db.NombresApellidos));
                    cmd.Parameters.Add(new SqlParameter("@Rucedula", db.Rucedula));
                    cmd.Parameters.Add(new SqlParameter("@Sueldo", db.Sueldo));
                    cmd.Parameters.Add(new SqlParameter("@Ingreso", db.Ingreso));
                    cmd.Parameters.Add(new SqlParameter("@Clase", db.Clase));
                    cmd.Parameters.Add(new SqlParameter("@Direccion", db.Direccion));
                    cmd.Parameters.Add(new SqlParameter("@Telefono", db.Telefono));
                    cmd.Parameters.Add(new SqlParameter("@Regimen", db.Regimen));
                    cmd.Parameters.Add(new SqlParameter("@Correo", db.Correo));
                    cmd.Parameters.Add(new SqlParameter("@password_hash", db.password_hash));
                    cmd.Parameters.Add(new SqlParameter("@password_salt", db.password_salt));
                    cmd.Parameters.Add(new SqlParameter("@Rol", db.Rol));
                    cmd.Parameters.Add(new SqlParameter("@FondoReserva", db.FondoReserva));
                    cmd.Parameters.Add(new SqlParameter("@Estado", db.Estado));
                    cmd.Parameters.Add(new SqlParameter("@RutaImagen", db.RutaImagen));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", db.Tipo));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            #endregion 

            Generica generica = new Generica();
            generica.valor1 = 1;
            generica.valor2 = "DOCUMENTO CARGADO..";
            response.Add(generica);

            return response;
        }

        public async Task<IEnumerable<Generica>> CargaArchivoGeneral(CargarArchivoBase64 cargar)
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

        public async Task<IEnumerable<Generica>> Insert(Contrato contrato )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", contrato.IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", contrato.IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdOrdenRecibida", contrato.IdOrdenRecibida));
                    cmd.Parameters.Add(new SqlParameter("@IdOrdenEnviada", contrato.IdOrdenEnviada));
                    cmd.Parameters.Add(new SqlParameter("@IdMaterialR", contrato.IdMaterialR));
                    cmd.Parameters.Add(new SqlParameter("@IdMaterialE", contrato.IdMaterialE));
                    cmd.Parameters.Add(new SqlParameter("@IdFacturado", contrato.IdFacturado));
                    cmd.Parameters.Add(new SqlParameter("@IdCertificado", contrato.IdCertificado));
                    cmd.Parameters.Add(new SqlParameter("@IdPagado", contrato.IdPagado));
                    cmd.Parameters.Add(new SqlParameter("@FechaIngreso", contrato.FechaIngreso));
                    cmd.Parameters.Add(new SqlParameter("@NumContrato", contrato.NumContrato));
                    cmd.Parameters.Add(new SqlParameter("@NumOrden", contrato.NumOrden));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", contrato.FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", contrato.FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@ValorBruto", contrato.ValorBruto));
                    cmd.Parameters.Add(new SqlParameter("@ComiAgen", contrato.ComiAgen));
                    cmd.Parameters.Add(new SqlParameter("@ValorConex", contrato.ValorConex));
                    cmd.Parameters.Add(new SqlParameter("@Valor", contrato.Valor));
                    cmd.Parameters.Add(new SqlParameter("@ComiConex", contrato.ComiConex));
                    cmd.Parameters.Add(new SqlParameter("@ValorAgen", contrato.ValorAgen));
                    cmd.Parameters.Add(new SqlParameter("@RucVendedor", contrato.RucVendedor));
                    cmd.Parameters.Add(new SqlParameter("@ComiVendedor", contrato.ComiVendedor));
                    cmd.Parameters.Add(new SqlParameter("@Anunciante", contrato.Anunciante));
                    cmd.Parameters.Add(new SqlParameter("@Agencia", contrato.Agencia));
                    cmd.Parameters.Add(new SqlParameter("@Contacto", contrato.Contacto));
                    cmd.Parameters.Add(new SqlParameter("@Medio", contrato.Medio));
                    cmd.Parameters.Add(new SqlParameter("@Facturado", contrato.Facturado));
                    cmd.Parameters.Add(new SqlParameter("@FechaCobro", contrato.FechaCobro));
                    cmd.Parameters.Add(new SqlParameter("@Estado", contrato.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", contrato.Tipo));
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

        public async Task<IEnumerable<Generica>> ActualizarDetalleContrato(ActuDetalleContrato contrato)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarDetalleContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdDetalleContrato", contrato.IdDetalleContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdMaterialR", contrato.IdMaterialR));
                    cmd.Parameters.Add(new SqlParameter("@IdMaterialE", contrato.IdMaterialE));
                    cmd.Parameters.Add(new SqlParameter("@Estado", contrato.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", contrato.Tipo));
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

        public async Task<IEnumerable<Contrato>> GetByMostrarContrato(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Contrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToContrato(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ContratoExterior>> GetByMostrarContratoExterior(string Medio, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContratoExterior", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Medio", Medio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ContratoExterior>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToContratoExterior(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ComisionVendedor>> GetByMostrarComisionVendedor(Int64 IdContrato, decimal ValorBruto, decimal ValorNeto,DateTime AnioProceso, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarComisionVendedor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@ValorBruto", ValorBruto));
                    cmd.Parameters.Add(new SqlParameter("@ValorNeto", ValorNeto));
                    cmd.Parameters.Add(new SqlParameter("@AnioProceso", AnioProceso));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ComisionVendedor>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToComisionVendedo(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<FacturaContrato>> GetByMostrarContratoPorFacturar(Int64 IdMedio, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContratoPorFacturar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<FacturaContrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToFacturaContrato(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<CargarFactura>> GetByMostrarFacturaNotaCreditoLista(Int64 IdMedio, string IdContrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaNotaCredito", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<CargarFactura>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToListaFacturas(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<FacturaContrato>> GetByMostrarFacturaNotaCreditoSeleccionada(Int64 IdMedio, string IdContrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaNotaCredito", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<FacturaContrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToFacturaContrato(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<FacturaContrato>> GetByMostrarFacturaPorCobrar(Int64 IdContrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaPorCobrar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<FacturaContrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToFacturaPorCobrar(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<FacturaContrato>> GetByMostrarFacturaPorCobrarFecha(Int64 IdMedio, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaPorCobrarFecha", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<FacturaContrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (Tipo == 2)
                            {
                                response.Add(MapToFacturaPorCobrarFecha2(reader));
                            }
                            else
                            {
                                response.Add(MapToFacturaPorCobrarFecha(reader));
                            }
                        }
                    }

                    return response;
                }
            }
        }

        public string DevolverArchivoBase64Factura(string rutaDocumentoResul)
        {

            string StringBase64 = "";
            try
            {
                if (File.Exists(rutaDocumentoResul))
                {
                    byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    StringBase64 = Convert.ToBase64String(archivoBytes);
                }
            }
            catch (Exception ex)
            {
                VerErrores("ex: " + ex.Message.ToString(), "Log", "Detalle", 1);
            }

            return StringBase64;
        }

        public string DevolverArchivoBase64(string rutaDocumentoResul)
        {

            string StringBase64 = "";
            try
            {
                if (File.Exists(rutaDocumentoResul))
                {
                    byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    StringBase64 = Convert.ToBase64String(archivoBytes);
                }
            }
            catch (Exception ex)
            {
                VerErrores("ex: " + ex.Message.ToString(), "Log", "Detalle", 1);
            }

            return StringBase64;
        }

        public string DevolverArchivoPDFBase64(string rutaDocumentoResul)
        {
            string StringBase64 = "";
            try
            {
                if (File.Exists(rutaDocumentoResul))
                {
                    string str11 = ".pdf";
                    string nameArchivo = Path.GetFileName(rutaDocumentoResul);
                    string rutaDocumento = rutaDocumentoResul.Replace(nameArchivo, "");
                    string QuitarExtArchivo = nameArchivo.Replace(".xml", "");
                    string RutaFinal = rutaDocumento + QuitarExtArchivo + str11;

                    byte[] archivoBytes = System.IO.File.ReadAllBytes(RutaFinal);
                    StringBase64 = Convert.ToBase64String(archivoBytes);
                }
            }
            catch (Exception ex)
            {
                VerErrores("ex: " + ex.Message.ToString(), "Log", "Detalle", 1);
            }

            return StringBase64;
        }

        #region VerErrores
        public void VerErrores(string valor, string Carpeta, string rucEmpresa, int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    string fecha;
                    fecha = DateTime.Now.ToString("dd-MM-yyyy");//DateTime.Now.ToShortDateString().Replace("/", "-");
                    if (!Directory.Exists(@"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha))
                    {
                        Directory.CreateDirectory(@"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha);
                    }

                    string path = @"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha + "\\log.txt";
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ": " + valor);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                // System.Diagnostics.EventLog.WriteEntry("Application", "Exception: " + ex.Message);
            }
        }
        #endregion

        public async Task<IEnumerable<ForeAprobadas>> GetByMostrarForeCastAprobadas(Int64 IdForeCast, Int64 IdContrato, Int64 IdEmpleado, string Perfil2, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@Perfil2", Perfil2));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ForeAprobadas>();
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

        public async Task<IEnumerable<Generica>> InsertDetalleContrato(DatosExtra foreCast, string json)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarDetalleContratoTable", sql))
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
        public async Task<IEnumerable<DetalleContrato>> GetByMostrarDetalleContrato(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<DetalleContrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDetalleContrato(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<DetalleContratoExcel>> GetByMostrarDetalleContratoExcel(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo, Int32 TipoProceso)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<DetalleContratoExcel>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDetalleContratoExcel(reader));
                        }
                    }
                    var response2 = await GetByMostrarContratoReporte(IdForeCast, IdContrato, 5);
                    GenerarReporte(response, response2, IdContrato,"CONTRATO", TipoProceso);

                    return response;
                }
            }
        }

        public async Task<IEnumerable<DetalleContratoExcel>> GetByMostrarDetallePautaExcel(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo, Int32 TipoProceso)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<DetalleContratoExcel>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDetalleContratoExcel(reader));
                        }
                    }
                    var response2 = await GetByMostrarContratoReporte(IdForeCast, IdContrato, 8);
                    GenerarReportePauta(response, response2, IdForeCast, "PAUTA", TipoProceso);

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ContratoReporte>> GetByMostrarContratoReporte(Int64 IdForeCast, Int64 IdContrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdForeCast", IdForeCast));
                    cmd.Parameters.Add(new SqlParameter("@IdContrato", IdContrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ContratoReporte>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToContratoReporte(reader));
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
        public async Task<IEnumerable<Generica>> InsertPagoContrato(string json,string jsonFinal, string Descripcion, int Estado, int Tipo, string FechaEmision, string FechaMesContrato,int IdIva)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarPagoContrato", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@FechaEmision", FechaEmision));
                    cmd.Parameters.Add(new SqlParameter("@FechaMesContrato", FechaMesContrato));
                    cmd.Parameters.Add(new SqlParameter("@IdIva", IdIva));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public async Task<IEnumerable<Generica>> InsertNotaDeCredito(string json, string jsonFinal, string Descripcion, int Estado, int Tipo,int IdActivar, string FechaEmision, int IdIva)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarNotaDeCredito", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@FechaEmision", FechaEmision));
                    cmd.Parameters.Add(new SqlParameter("@IdActivar", IdActivar));
                    cmd.Parameters.Add(new SqlParameter("@IdIva", IdIva));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public async Task<IEnumerable<Generica>> InsertFacturaServicio(string json, string jsonFinal, string Descripcion, int Estado, int Tipo, string FechaEmision)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarFacturaServicio", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@FechaEmision", FechaEmision));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public async Task<IEnumerable<Generica>> InsertLiquidacion(string json, string jsonFinal,string jsonReembolso,string jsonAsiento, string Descripcion, int Estado, int Tipo, string FechaEmision)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarLiquidacion", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@jsonReembolso", jsonReembolso));
                    cmd.Parameters.Add(new SqlParameter("@jsonAsiento", jsonAsiento));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@FechaEmision", FechaEmision));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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


        public async Task<IEnumerable<Generica>> InsertCobroContrato(string json, string jsonFinal, string Descripcion,string TipoTransaccion,decimal ValorProceso, int Estado, int Tipo,string NumDocumento)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarCobrosAsientosContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@TipoTransaccion", TipoTransaccion));
                    cmd.Parameters.Add(new SqlParameter("@ValorProceso", ValorProceso));
                    cmd.Parameters.Add(new SqlParameter("@NumDocumento", NumDocumento));
                    cmd.Parameters.Add(new SqlParameter("@Estado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public async Task<IEnumerable<Generica>> InsertAsientosContable(string json, string jsonFinal, string jsonValorReal, string Descripcion, string TipoTransaccion, decimal ValorProceso, int Estado, string VariosProceso , int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarAsientosContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@json", json));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@jsonValorReal", jsonValorReal));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@TipoTransaccion", TipoTransaccion.Trim()));
                    cmd.Parameters.Add(new SqlParameter("@ValorProceso", ValorProceso));
                    cmd.Parameters.Add(new SqlParameter("@Estado", Estado));
                    cmd.Parameters.Add(new SqlParameter("@VariosProceso", VariosProceso));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        private DetalleContrato MapToDetalleContrato(SqlDataReader reader)
        {
            return new DetalleContrato()
            {
                IdDetalleContrato=(Int64)reader["IdDetalleContrato"],
                IdMaterialE = (Int64)reader["IdMaterialE"],
                IdMaterialR = (Int64)reader["IdMaterialR"],
                Canal = reader["Canal"].ToString(),
                Programa = reader["Programa"].ToString(),
                Duracion = reader["Duracion"].ToString(),
                Franja = reader["Franja"].ToString(),
                Tarifa = (decimal)reader["Tarifa"],
                TotalSegundos = (decimal)reader["TotalSegundos"],
                ValorNegocio = (decimal)reader["ValorNegocio"],
                Descripcion = reader["Descripcion"].ToString(),
                Detalle = reader["Detalle"].ToString(),
            };








        }
        private ContratoReporte MapToContratoReporte(SqlDataReader reader)
        {
            return new ContratoReporte()
            {
                FechaInicio = (DateTime)reader["FechaInicio"],
                FechaFinal = (DateTime)reader["FechaFinal"],
                NumContrato = reader["NumContrato"].ToString(),
                Anunciante = reader["Anunciante"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                ComiAgen = (decimal)reader["ComiAgen"],
            };
        }
        private DetalleContratoExcel MapToDetalleContratoExcel(SqlDataReader reader)
        {
            DetalleContratoExcel detalleContratoExcel = new DetalleContratoExcel();

            if (reader.FieldCount == 46)
            {

                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Programa = reader["Programa"].ToString();
                detalleContratoExcel.Detalle = reader["Detalle"].ToString();
                detalleContratoExcel.Versiones = reader["Versiones"].ToString();
                detalleContratoExcel.Duracion = reader["Duracion"].ToString();
                detalleContratoExcel.Derecho = reader["Derecho"].ToString();
                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Franja = reader["Franja"].ToString();
                detalleContratoExcel.Tarifa = reader["Tarifa"].ToString();
                detalleContratoExcel.TotalSegundo = reader["TotalSegundo"].ToString();
                detalleContratoExcel.ValorNegocio = reader["ValorNegocio"].ToString();

                detalleContratoExcel.Valor1 = reader["Valor1"].ToString();
                detalleContratoExcel.Valor2 = reader["Valor2"].ToString();
                detalleContratoExcel.Valor3 = reader["Valor3"].ToString();
                detalleContratoExcel.Valor4 = reader["Valor4"].ToString();

                detalleContratoExcel.data_1 = reader["data_1"].ToString();
                detalleContratoExcel.data_2 = reader["data_2"].ToString();
                detalleContratoExcel.data_3 = reader["data_3"].ToString();
                detalleContratoExcel.data_4 = reader["data_4"].ToString();
                detalleContratoExcel.data_5 = reader["data_5"].ToString();
                detalleContratoExcel.data_6 = reader["data_6"].ToString();
                detalleContratoExcel.data_7 = reader["data_7"].ToString();
                detalleContratoExcel.data_8 = reader["data_8"].ToString();
                detalleContratoExcel.data_9 = reader["data_9"].ToString();
                detalleContratoExcel.data_10 = reader["data_10"].ToString();
                detalleContratoExcel.data_11 = reader["data_11"].ToString();
                detalleContratoExcel.data_12 = reader["data_12"].ToString();
                detalleContratoExcel.data_13 = reader["data_13"].ToString();
                detalleContratoExcel.data_14 = reader["data_14"].ToString();
                detalleContratoExcel.data_15 = reader["data_15"].ToString();
                detalleContratoExcel.data_16 = reader["data_16"].ToString();
                detalleContratoExcel.data_17 = reader["data_17"].ToString();
                detalleContratoExcel.data_18 = reader["data_18"].ToString();
                detalleContratoExcel.data_19 = reader["data_19"].ToString();
                detalleContratoExcel.data_20 = reader["data_20"].ToString();
                detalleContratoExcel.data_21 = reader["data_21"].ToString();
                detalleContratoExcel.data_22 = reader["data_22"].ToString();
                detalleContratoExcel.data_23 = reader["data_23"].ToString();
                detalleContratoExcel.data_24 = reader["data_24"].ToString();
                detalleContratoExcel.data_25 = reader["data_25"].ToString();
                detalleContratoExcel.data_26 = reader["data_26"].ToString();
                detalleContratoExcel.data_27 = reader["data_27"].ToString();
                detalleContratoExcel.data_28 = reader["data_28"].ToString();
                detalleContratoExcel.data_29 = reader["data_29"].ToString();
                detalleContratoExcel.data_30 = reader["data_30"].ToString();
                detalleContratoExcel.data_31 = reader["data_31"].ToString();
                detalleContratoExcel.Impacto = reader["Impacto"].ToString();
            }
            else if (reader.FieldCount == 45)
            {

                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Programa = reader["Programa"].ToString();
                detalleContratoExcel.Detalle = reader["Detalle"].ToString();
                detalleContratoExcel.Versiones = reader["Versiones"].ToString();
                detalleContratoExcel.Duracion = reader["Duracion"].ToString();
                detalleContratoExcel.Derecho = reader["Derecho"].ToString();
                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Franja = reader["Franja"].ToString();
                detalleContratoExcel.Tarifa = reader["Tarifa"].ToString();
                detalleContratoExcel.TotalSegundo = reader["TotalSegundo"].ToString();
                detalleContratoExcel.ValorNegocio = reader["ValorNegocio"].ToString();

                detalleContratoExcel.Valor1 = reader["Valor1"].ToString();
                detalleContratoExcel.Valor2 = reader["Valor2"].ToString();
                detalleContratoExcel.Valor3 = reader["Valor3"].ToString();
                detalleContratoExcel.Valor4 = reader["Valor4"].ToString();

                detalleContratoExcel.data_1 = reader["data_1"].ToString();
                detalleContratoExcel.data_2 = reader["data_2"].ToString();
                detalleContratoExcel.data_3 = reader["data_3"].ToString();
                detalleContratoExcel.data_4 = reader["data_4"].ToString();
                detalleContratoExcel.data_5 = reader["data_5"].ToString();
                detalleContratoExcel.data_6 = reader["data_6"].ToString();
                detalleContratoExcel.data_7 = reader["data_7"].ToString();
                detalleContratoExcel.data_8 = reader["data_8"].ToString();
                detalleContratoExcel.data_9 = reader["data_9"].ToString();
                detalleContratoExcel.data_10 = reader["data_10"].ToString();
                detalleContratoExcel.data_11 = reader["data_11"].ToString();
                detalleContratoExcel.data_12 = reader["data_12"].ToString();
                detalleContratoExcel.data_13 = reader["data_13"].ToString();
                detalleContratoExcel.data_14 = reader["data_14"].ToString();
                detalleContratoExcel.data_15 = reader["data_15"].ToString();
                detalleContratoExcel.data_16 = reader["data_16"].ToString();
                detalleContratoExcel.data_17 = reader["data_17"].ToString();
                detalleContratoExcel.data_18 = reader["data_18"].ToString();
                detalleContratoExcel.data_19 = reader["data_19"].ToString();
                detalleContratoExcel.data_20 = reader["data_20"].ToString();
                detalleContratoExcel.data_21 = reader["data_21"].ToString();
                detalleContratoExcel.data_22 = reader["data_22"].ToString();
                detalleContratoExcel.data_23 = reader["data_23"].ToString();
                detalleContratoExcel.data_24 = reader["data_24"].ToString();
                detalleContratoExcel.data_25 = reader["data_25"].ToString();
                detalleContratoExcel.data_26 = reader["data_26"].ToString();
                detalleContratoExcel.data_27 = reader["data_27"].ToString();
                detalleContratoExcel.data_28 = reader["data_28"].ToString();
                detalleContratoExcel.data_29 = reader["data_29"].ToString();
                detalleContratoExcel.data_30 = reader["data_30"].ToString();
                detalleContratoExcel.Impacto = reader["Impacto"].ToString();

            }
            else if (reader.FieldCount == 44)
            {

                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Programa = reader["Programa"].ToString();
                detalleContratoExcel.Detalle = reader["Detalle"].ToString();
                detalleContratoExcel.Versiones = reader["Versiones"].ToString();
                detalleContratoExcel.Duracion = reader["Duracion"].ToString();
                detalleContratoExcel.Derecho = reader["Derecho"].ToString();
                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Franja = reader["Franja"].ToString();
                detalleContratoExcel.Tarifa = reader["Tarifa"].ToString();
                detalleContratoExcel.TotalSegundo = reader["TotalSegundo"].ToString();
                detalleContratoExcel.ValorNegocio = reader["ValorNegocio"].ToString();

                detalleContratoExcel.Valor1 = reader["Valor1"].ToString();
                detalleContratoExcel.Valor2 = reader["Valor2"].ToString();
                detalleContratoExcel.Valor3 = reader["Valor3"].ToString();
                detalleContratoExcel.Valor4 = reader["Valor4"].ToString();

                detalleContratoExcel.data_1 = reader["data_1"].ToString();
                detalleContratoExcel.data_2 = reader["data_2"].ToString();
                detalleContratoExcel.data_3 = reader["data_3"].ToString();
                detalleContratoExcel.data_4 = reader["data_4"].ToString();
                detalleContratoExcel.data_5 = reader["data_5"].ToString();
                detalleContratoExcel.data_6 = reader["data_6"].ToString();
                detalleContratoExcel.data_7 = reader["data_7"].ToString();
                detalleContratoExcel.data_8 = reader["data_8"].ToString();
                detalleContratoExcel.data_9 = reader["data_9"].ToString();
                detalleContratoExcel.data_10 = reader["data_10"].ToString();
                detalleContratoExcel.data_11 = reader["data_11"].ToString();
                detalleContratoExcel.data_12 = reader["data_12"].ToString();
                detalleContratoExcel.data_13 = reader["data_13"].ToString();
                detalleContratoExcel.data_14 = reader["data_14"].ToString();
                detalleContratoExcel.data_15 = reader["data_15"].ToString();
                detalleContratoExcel.data_16 = reader["data_16"].ToString();
                detalleContratoExcel.data_17 = reader["data_17"].ToString();
                detalleContratoExcel.data_18 = reader["data_18"].ToString();
                detalleContratoExcel.data_19 = reader["data_19"].ToString();
                detalleContratoExcel.data_20 = reader["data_20"].ToString();
                detalleContratoExcel.data_21 = reader["data_21"].ToString();
                detalleContratoExcel.data_22 = reader["data_22"].ToString();
                detalleContratoExcel.data_23 = reader["data_23"].ToString();
                detalleContratoExcel.data_24 = reader["data_24"].ToString();
                detalleContratoExcel.data_25 = reader["data_25"].ToString();
                detalleContratoExcel.data_26 = reader["data_26"].ToString();
                detalleContratoExcel.data_27 = reader["data_27"].ToString();
                detalleContratoExcel.data_28 = reader["data_28"].ToString();
                detalleContratoExcel.data_29 = reader["data_29"].ToString();
                detalleContratoExcel.Impacto = reader["Impacto"].ToString();

            }
            else if (reader.FieldCount == 43)
            {

                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Programa = reader["Programa"].ToString();
                detalleContratoExcel.Detalle = reader["Detalle"].ToString();
                detalleContratoExcel.Versiones = reader["Versiones"].ToString();
                detalleContratoExcel.Duracion = reader["Duracion"].ToString();
                detalleContratoExcel.Derecho = reader["Derecho"].ToString();
                detalleContratoExcel.Canal = reader["Canal"].ToString();
                detalleContratoExcel.Franja = reader["Franja"].ToString();
                detalleContratoExcel.Tarifa = reader["Tarifa"].ToString();
                detalleContratoExcel.TotalSegundo = reader["TotalSegundo"].ToString();
                detalleContratoExcel.ValorNegocio = reader["ValorNegocio"].ToString();

                detalleContratoExcel.Valor1 = reader["Valor1"].ToString();
                detalleContratoExcel.Valor2 = reader["Valor2"].ToString();
                detalleContratoExcel.Valor3 = reader["Valor3"].ToString();
                detalleContratoExcel.Valor4 = reader["Valor4"].ToString();

                detalleContratoExcel.data_1 = reader["data_1"].ToString();
                detalleContratoExcel.data_2 = reader["data_2"].ToString();
                detalleContratoExcel.data_3 = reader["data_3"].ToString();
                detalleContratoExcel.data_4 = reader["data_4"].ToString();
                detalleContratoExcel.data_5 = reader["data_5"].ToString();
                detalleContratoExcel.data_6 = reader["data_6"].ToString();
                detalleContratoExcel.data_7 = reader["data_7"].ToString();
                detalleContratoExcel.data_8 = reader["data_8"].ToString();
                detalleContratoExcel.data_9 = reader["data_9"].ToString();
                detalleContratoExcel.data_10 = reader["data_10"].ToString();
                detalleContratoExcel.data_11 = reader["data_11"].ToString();
                detalleContratoExcel.data_12 = reader["data_12"].ToString();
                detalleContratoExcel.data_13 = reader["data_13"].ToString();
                detalleContratoExcel.data_14 = reader["data_14"].ToString();
                detalleContratoExcel.data_15 = reader["data_15"].ToString();
                detalleContratoExcel.data_16 = reader["data_16"].ToString();
                detalleContratoExcel.data_17 = reader["data_17"].ToString();
                detalleContratoExcel.data_18 = reader["data_18"].ToString();
                detalleContratoExcel.data_19 = reader["data_19"].ToString();
                detalleContratoExcel.data_20 = reader["data_20"].ToString();
                detalleContratoExcel.data_21 = reader["data_21"].ToString();
                detalleContratoExcel.data_22 = reader["data_22"].ToString();
                detalleContratoExcel.data_23 = reader["data_23"].ToString();
                detalleContratoExcel.data_24 = reader["data_24"].ToString();
                detalleContratoExcel.data_25 = reader["data_25"].ToString();
                detalleContratoExcel.data_26 = reader["data_26"].ToString();
                detalleContratoExcel.data_27 = reader["data_27"].ToString();
                detalleContratoExcel.data_28 = reader["data_28"].ToString();
                detalleContratoExcel.Impacto = reader["Impacto"].ToString();

            }

            return detalleContratoExcel;
        }

        public async Task<IEnumerable<Cobros>> GetByMostrarCobros(Int64 IdPagocontrato, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarCobros", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPagocontrato", IdPagocontrato));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Cobros>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToCobros(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Contrato MapToContrato(SqlDataReader reader)
        {
            return new Contrato()
            {
                IdContrato = (Int64)reader["IdContrato"],
                IdForeCast = (Int64)reader["IdForeCast"],
                IdOrdenRecibida = (Int64)reader["IdOrdenRecibida"],
                IdOrdenEnviada = (Int64)reader["IdOrdenEnviada"],
                IdMaterialR = (Int64)reader["IdMaterialR"],
                IdMaterialE = (Int64)reader["IdMaterialE"],
                IdFacturado = (Int64)reader["IdFacturado"],
                IdCertificado = (Int64)reader["IdCertificado"],
                IdPagado = (Int64)reader["IdPagado"],
                FechaIngreso = (DateTime)reader["FechaIngreso"],
                FechaInicio = (DateTime)reader["FechaInicio"],
                FechaFinal = (DateTime)reader["FechaFinal"],
                ValorBruto = (decimal)reader["ValorBruto"],
                ComiAgen = (decimal)reader["ComiAgen"],
                ValorAgen = (decimal)reader["ValorAgen"],
                Valor = (decimal)reader["Valor"],
                ComiConex = (decimal)reader["ComiConex"],
                ValorConex = (decimal)reader["ValorConex"],
                RucVendedor = reader["RucVendedor"].ToString(),
                ComiVendedor = (decimal)reader["ComiVendedor"],
                Anunciante = reader["Anunciante"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                Medio = reader["Medio"].ToString(),
                Facturado = (decimal)reader["Facturado"],
                FechaCobro = (DateTime)reader["FechaCobro"],
                NumContrato = reader["NumContrato"].ToString(),
                NumOrden = reader["NumOrden"].ToString(),
                //NombreProyecto = reader["NombreProyecto"].ToString(),
                Estado = (Int32)reader["Estado"],
            };
        }

        private ContratoExterior MapToContratoExterior(SqlDataReader reader)
        {
            return new ContratoExterior()
            {
                NumContrato = reader["NumContrato"].ToString(),             
                NombreProyecto = reader["NombreProyecto"].ToString(),
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                Anunciante = reader["Anunciante"].ToString(),
                Email = reader["Email"].ToString(),
            };
        }


        private ComisionVendedor MapToComisionVendedo(SqlDataReader reader)
        {
            return new ComisionVendedor()
            {              
                Comision = (decimal)reader["Comision"],
                Porcentaje = (decimal)reader["Porcentaje"],
            };
        }
        private FacturaContrato MapToFacturaContrato(SqlDataReader reader)
        {
            return new FacturaContrato()
            {
                IdContrato = (Int64)reader["IdContrato"],
                NumContrato = reader["NumContrato"].ToString(),
                FechaInicio = (DateTime)reader["FechaInicio"],
                FechaFinal = (DateTime)reader["FechaFinal"],
                ValorBruto = (decimal)reader["ValorBruto"],
                ComiAgen = (decimal)reader["ComiAgen"],
                ValorAgen = (decimal)reader["ValorAgen"],
                Valor = (decimal)reader["Valor"],
                ValorConex = (decimal)reader["ValorConex"],
                Anunciante = reader["Anunciante"].ToString(),
                Mes = reader["Mes"].ToString(),
                Anio = (Int32)reader["Anio"],
                NumOrden = reader["NumOrden"].ToString(),
            };
        }

        private CargarFactura MapToListaFacturas(SqlDataReader reader)
        {
            return new CargarFactura()
            {
                ValorBruto = (decimal)reader["ValorBruto"],
                ValorNeto = (decimal)reader["ValorNeto"],
                ValorCobrar = (decimal)reader["ValorCobrar"],
                Iva = (decimal)reader["Iva"],
                Total = (decimal)reader["Total"],
                NumDocumento = reader["NumDocumento"].ToString(),
                IdContrato = reader["IdContrato"].ToString(),
            };
        }
        private FacturaContrato MapToFacturaPorCobrar(SqlDataReader reader)
        {
            return new FacturaContrato()
            {

                ValorBruto = (decimal)reader["ValorBruto"],
                ValorCobrar  = (decimal)reader["ValorCobrar"],
                ValorNeto = (decimal)reader["ValorNeto"],
                ComisionVendedor = (decimal)reader["ComisionVendedor"],
                ComisionPorcentaje = (decimal)reader["ComisionPorcentaje"],
                NumDocumento = reader["NumDocumento"].ToString(),
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                EstadoPago = reader["EstadoPago"].ToString()
            };
        }

        private FacturaContrato MapToFacturaPorCobrarFecha(SqlDataReader reader)
        {
            return new FacturaContrato()
            {

                IdPagocontrato = (Int64)reader["IdPagocontrato"],
                ValorBruto = (decimal)reader["ValorBruto"],
                ValorCobrar = (decimal)reader["ValorCobrar"],
                ValorRetencion = (decimal)reader["ValorRetencion"],
                Saldo = (decimal)reader["Saldo"],
                ValorNeto = (decimal)reader["ValorNeto"],
                ComisionVendedor = (decimal)reader["ComisionVendedor"],
                ComisionPorcentaje = (decimal)reader["ComisionPorcentaje"],
                NumDocumento = reader["NumDocumento"].ToString(),
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                EstadoPago = reader["EstadoPago"].ToString(),
                NombreMedio = reader["NombreMedio"].ToString(),
                Vendedor = reader["Vendedor"].ToString()
            };
        }

        private FacturaContrato MapToFacturaPorCobrarFecha2(SqlDataReader reader)
        {
            return new FacturaContrato()
            {

                IdPagocontrato = (Int64)reader["IdPagocontrato"],
                ValorBruto = (decimal)reader["ValorBruto"],
                ValorCobrar = (decimal)reader["ValorCobrar"],
                ValorRetencion = (decimal)reader["ValorRetencion"],
                Saldo = (decimal)reader["Saldo"],
                ValorNeto = (decimal)reader["ValorNeto"],
                ComisionVendedor = (decimal)reader["ComisionVendedor"],
                ComisionPorcentaje = (decimal)reader["ComisionPorcentaje"],
                NumDocumento = reader["NumDocumento"].ToString(),
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                EstadoPago = reader["EstadoPago"].ToString(),
                NombreMedio = reader["NombreMedio"].ToString(),
                Vendedor = reader["Vendedor"].ToString(),
                Anunciante = reader["Anunciante"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                NumContrato = reader["NumContrato"].ToString(),
                ValorRenta = (decimal)reader["ValorRenta"],
                ValorIva = (decimal)reader["ValorIva"],
                Iva = (decimal)reader["Iva"],
                RutaRetencion = DevolverArchivoBase64(reader["RutaRetencion"].ToString()),
                RutaRetencionPDF = DevolverArchivoPDFBase64(reader["RutaRetencion"].ToString()),
                RutaFacturaXML = DevolverArchivoBase64Factura(reader["RutaFacturaXML"].ToString()),
                RutaFacturaPDF = DevolverArchivoBase64Factura(reader["RutaFacturaPDF"].ToString()),
                NumRetencion = reader["NumRetencion"].ToString(),
            };
        }

        private ForeAprobadas MapToForeCast(SqlDataReader reader)
        {
            return new ForeAprobadas()
            {
                IdForeCast = (Int64)reader["IdForeCast"],
                IdMedio = (Int64)reader["IdMedio"],
                Cliente = reader["Cliente"].ToString(),
                Medios = reader["Medios"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                NombreProyecto = reader["NombreProyecto"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                ValorTotalNeto = (decimal)reader["ValorTotalNeto"],
                ValorTotalBruto = (decimal)reader["ValorTotalBruto"],
                FechaInicioPauta = (DateTime)reader["FechaInicioPauta"],
                FechaFinalPauta = (DateTime)reader["FechaFinalPauta"],
                NumContrato = reader["NumContrato"].ToString(),
                Estado = (Int32)reader["Estado"],
                TotalNegocio = (decimal)reader["TotalNegocio"],
                TotalSegundos = (decimal)reader["TotalSegundos"],
                RucVendedor = reader["RucVendedor"].ToString(),
            };
        }
        private Cobros MapToCobros(SqlDataReader reader)
        {
            return new Cobros()
            {
                IdCobro = (Int64)reader["IdCobro"],
                IdPagocontrato = (Int64)reader["IdPagocontrato"],
                FormaPago = reader["FormaPago"].ToString(),
                Valor = (decimal)reader["Valor"],
                Saldo = (decimal)reader["Saldo"],
                Total = (decimal)reader["Total"],
                Observacion = reader["Observacion"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                FechaPago = (DateTime)reader["FechaPago"],
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
        private string GenerarReporte(List<DetalleContratoExcel> detalles, IEnumerable<ContratoReporte> reportes, Int64 IdContrato, string TipoDocumento, Int32 TipoProceso)
        {
            string Ruta = "";
            string fecha;
            string NumContrato = "";
            int numeroRegistro = detalles.Count;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(IdContrato, 0, TipoDocumento, TipoProceso);
            string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;// "F:\\PropuestaConexion\\MESES.xlsx";
            string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;// "F:\\PropuestaConexion\\MESES_" + fecha + ".xlsx";
            using var wbook = new XLWorkbook(rutaDocumento);
            int cont = 16;
            int cont2 = 14;
            int ValorPorcentaje = numeroRegistro + 21;

            DateTime dateTime = DateTime.Now;
            var ws = wbook.Worksheet(1);
            ws.Row(16).InsertRowsBelow(numeroRegistro);
            foreach (ContratoReporte s in reportes)
            {
                ws.Cell("C6").Value = s.NumContrato;
                ws.Cell("C6").Style.Font.Bold = true;

                ws.Cell("C7").Value = s.Anunciante;
                ws.Cell("C7").Style.Font.Bold = true;

                ws.Cell("C8").Value = s.Agencia;
                ws.Cell("C8").Style.Font.Bold = true;

                ws.Cell("C9").Value = s.FechaInicio.ToString("yyyy-MM-dd") + " - " + s.FechaFinal.ToString("yyyy-MM-dd");
                ws.Cell("C9").Style.Font.Bold = true;

                ws.Cell("C10").Value = dateTime.ToString("yyyy-MM-dd");
                ws.Cell("C10").Style.Font.Bold = true;

                DateTime fechaActual = DateTime.Now;
                string mes1 = s.FechaInicio.ToString("MMMM");
                ws.Cell("Q13").Value = mes1.ToUpper();
                ws.Cell("Q13").Style.Font.Bold = true;

                int valorPor = Convert.ToInt32(s.ComiAgen);

                //ws.Cell("M286").Value = valorPor;
                //ws.Cell("M286").Style.Font.Bold = true;
                ws.Cell("N" + ValorPorcentaje.ToString()).Value = valorPor; //ANTES M
                ws.Cell("N" + ValorPorcentaje.ToString()).Style.Font.Bold = true; //ANTES M
                int ivaValor = 0;
                int ivaValor2 = 0;
                ivaValor = ValorPorcentaje + 2;
                ivaValor2 = ValorPorcentaje + 1;

                DateTime fechaIva = Convert.ToDateTime("2024-03-31");
                if (DateTime.Compare(fechaIva, s.FechaInicio) < 0)
                {
                    ws.Cell("N" + ivaValor.ToString()).Value = "(+)  15% IVA"; //ANTES M
                    ws.Cell("N" + ivaValor.ToString()).Style.Font.Bold = true; //ANTES M

                    ws.Cell("P" + ivaValor.ToString()).FormulaA1 = "=P" + ivaValor2.ToString() + "*" + "15%"; //ANTES O
                    ws.Cell("P" + ivaValor.ToString()).Style.Font.Bold = true; //ANTES O
                }

                NumContrato = s.NumContrato;

            }

            foreach (DetalleContratoExcel s in detalles)
            {
                if(s.Programa != "" && s.Programa != "PROGRAMA")
                {
                    ws.Cell("B" + cont.ToString()).Value = s.Canal;
                    ws.Cell("C" + cont.ToString()).Value = s.Programa;
                    ws.Cell("D" + cont.ToString()).Value = s.Detalle;
                    ws.Cell("E" + cont.ToString()).Value = s.Versiones;
                    ws.Cell("F" + cont.ToString()).Value = s.Derecho;
                    ws.Cell("G" + cont.ToString()).Value = s.Duracion;//--F ANTES
                    ws.Cell("H" + cont.ToString()).Value = s.Franja;

                    ws.Cell("I" + cont.ToString()).Value = s.Tarifa;//--H ANTES
                    ws.Cell("J" + cont.ToString()).Value = s.Valor1;
                    ws.Cell("K" + cont.ToString()).Value = s.Valor2;
                    ws.Cell("L" + cont.ToString()).Value = s.Valor3;
                    ws.Cell("M" + cont.ToString()).Value = s.Valor4;
                    ws.Cell("O" + cont.ToString()).FormulaA1 = "=G" + cont.ToString() + "*" + "AV" + cont.ToString(); //--N ANTES
                    ws.Cell("P" + cont.ToString()).FormulaA1 = "=O" + cont.ToString() + "*" + "I" + cont.ToString(); //--O ANTES
                    cont++;
                }
            }
            int countFinal = cont + 2;
            int countFinall = cont + 3;
            ws.Cell("O" + countFinall.ToString()).FormulaA1 = "=SUM(O16:O" + countFinal.ToString() + ")"; //--N ANTES
            ws.Cell("P" + countFinall.ToString()).FormulaA1 = "=SUM(P16:P" + countFinal.ToString() + ")"; //--O ANTES

            foreach (DetalleContratoExcel s in detalles)
            {
                ws.Cell("Q" + cont2.ToString()).Value = s.data_1; //ANTES P
                ws.Cell("R" + cont2.ToString()).Value = s.data_2;
                ws.Cell("S" + cont2.ToString()).Value = s.data_3;
                ws.Cell("T" + cont2.ToString()).Value = s.data_4;
                ws.Cell("U" + cont2.ToString()).Value = s.data_5;
                ws.Cell("V" + cont2.ToString()).Value = s.data_6;
                ws.Cell("W" + cont2.ToString()).Value = s.data_7;
                ws.Cell("X" + cont2.ToString()).Value = s.data_8;
                ws.Cell("Y" + cont2.ToString()).Value = s.data_9;
                ws.Cell("Z" + cont2.ToString()).Value = s.data_10;
                ws.Cell("AA" + cont2.ToString()).Value = s.data_11;
                ws.Cell("AB" + cont2.ToString()).Value = s.data_12;
                ws.Cell("AC" + cont2.ToString()).Value = s.data_13;
                ws.Cell("AD" + cont2.ToString()).Value = s.data_14;
                ws.Cell("AE" + cont2.ToString()).Value = s.data_15;
                ws.Cell("AF" + cont2.ToString()).Value = s.data_16;
                ws.Cell("AG" + cont2.ToString()).Value = s.data_17;
                ws.Cell("AH" + cont2.ToString()).Value = s.data_18;
                ws.Cell("AI" + cont2.ToString()).Value = s.data_19;
                ws.Cell("AJ" + cont2.ToString()).Value = s.data_20;
                ws.Cell("AK" + cont2.ToString()).Value = s.data_21;
                ws.Cell("AL" + cont2.ToString()).Value = s.data_22;
                ws.Cell("AM" + cont2.ToString()).Value = s.data_23;
                ws.Cell("AN" + cont2.ToString()).Value = s.data_24;
                ws.Cell("AO" + cont2.ToString()).Value = s.data_25;
                ws.Cell("AP" + cont2.ToString()).Value = s.data_26;
                ws.Cell("AQ" + cont2.ToString()).Value = s.data_27;
                ws.Cell("AR" + cont2.ToString()).Value = s.data_28;
                ws.Cell("AS" + cont2.ToString()).Value = s.data_29;
                ws.Cell("AT" + cont2.ToString()).Value = s.data_30;
                ws.Cell("AU" + cont2.ToString()).Value = s.data_31;  //--AT ANTES
                ws.Cell("AV" + cont2.ToString()).FormulaA1 = "=SUM(Q" + cont2.ToString() + ":AU" + cont2.ToString() + ")";
                cont2++;
            }


            int countFinal2 = cont2 + 2;
            int countFinall2 = cont2 + 3;
            ws.Cell("Q" + countFinall2.ToString()).FormulaA1 = "SUM(Q16:Q" + countFinal2.ToString() + ")";
            ws.Cell("R" + countFinall2.ToString()).FormulaA1 = "SUM(R16:R" + countFinal2.ToString() + ")";
            ws.Cell("S" + countFinall2.ToString()).FormulaA1 = "SUM(S16:S" + countFinal2.ToString() + ")";
            ws.Cell("T" + countFinall2.ToString()).FormulaA1 = "SUM(T16:T" + countFinal2.ToString() + ")";
            ws.Cell("U" + countFinall2.ToString()).FormulaA1 = "SUM(U16:U" + countFinal2.ToString() + ")";
            ws.Cell("V" + countFinall2.ToString()).FormulaA1 = "SUM(V16:V" + countFinal2.ToString() + ")";
            ws.Cell("W" + countFinall2.ToString()).FormulaA1 = "SUM(W16:W" + countFinal2.ToString() + ")";
            ws.Cell("X" + countFinall2.ToString()).FormulaA1 = "SUM(X16:X" + countFinal2.ToString() + ")";
            ws.Cell("Y" + countFinall2.ToString()).FormulaA1 = "SUM(Y16:Y" + countFinal2.ToString() + ")";
            ws.Cell("Z" + countFinall2.ToString()).FormulaA1 = "SUM(Z16:Z" + countFinal2.ToString() + ")";
            ws.Cell("AA" + countFinall2.ToString()).FormulaA1 = "SUM(AA16:AA" + countFinal2.ToString() + ")";
            ws.Cell("AB" + countFinall2.ToString()).FormulaA1 = "SUM(AB16:AB" + countFinal2.ToString() + ")";
            ws.Cell("AC" + countFinall2.ToString()).FormulaA1 = "SUM(AC16:AC" + countFinal2.ToString() + ")";
            ws.Cell("AD" + countFinall2.ToString()).FormulaA1 = "SUM(AD16:AD" + countFinal2.ToString() + ")";
            ws.Cell("AE" + countFinall2.ToString()).FormulaA1 = "SUM(AE16:AE" + countFinal2.ToString() + ")";
            ws.Cell("AF" + countFinall2.ToString()).FormulaA1 = "SUM(AF16:AF" + countFinal2.ToString() + ")";
            ws.Cell("AG" + countFinall2.ToString()).FormulaA1 = "SUM(AG16:AG" + countFinal2.ToString() + ")";
            ws.Cell("AH" + countFinall2.ToString()).FormulaA1 = "SUM(AH16:AH" + countFinal2.ToString() + ")";
            ws.Cell("AI" + countFinall2.ToString()).FormulaA1 = "SUM(AI16:AI" + countFinal2.ToString() + ")";
            ws.Cell("AJ" + countFinall2.ToString()).FormulaA1 = "SUM(AJ16:AJ" + countFinal2.ToString() + ")";
            ws.Cell("AK" + countFinall2.ToString()).FormulaA1 = "SUM(AK16:AK" + countFinal2.ToString() + ")";
            ws.Cell("AL" + countFinall2.ToString()).FormulaA1 = "SUM(AL16:AL" + countFinal2.ToString() + ")";
            ws.Cell("AM" + countFinall2.ToString()).FormulaA1 = "SUM(AM16:AM" + countFinal2.ToString() + ")";
            ws.Cell("AN" + countFinall2.ToString()).FormulaA1 = "SUM(AN16:AN" + countFinal2.ToString() + ")";
            ws.Cell("AO" + countFinall2.ToString()).FormulaA1 = "SUM(AO16:AO" + countFinal2.ToString() + ")";
            ws.Cell("AP" + countFinall2.ToString()).FormulaA1 = "SUM(AP16:AP" + countFinal2.ToString() + ")";
            ws.Cell("AQ" + countFinall2.ToString()).FormulaA1 = "SUM(AQ16:AQ" + countFinal2.ToString() + ")";
            ws.Cell("AR" + countFinall2.ToString()).FormulaA1 = "SUM(AR16:AR" + countFinal2.ToString() + ")";
            ws.Cell("AS" + countFinall2.ToString()).FormulaA1 = "SUM(AS16:AS" + countFinal2.ToString() + ")";
            ws.Cell("AT" + countFinall2.ToString()).FormulaA1 = "SUM(AT16:AT" + countFinal2.ToString() + ")";
            ws.Cell("AU" + countFinall2.ToString()).FormulaA1 = "SUM(AU16:AU" + countFinal2.ToString() + ")";
            ws.Cell("AV" + countFinall2.ToString()).FormulaA1 = "SUM(AV16:AV" + countFinal2.ToString() + ")";

            //wbook.SaveAs(rutaDocumentoResul);
            //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
            byte[] archivoBytes = null;
            using (var msA = new MemoryStream())
            {
                wbook.SaveAs(msA);
                archivoBytes = msA.ToArray();
            }
            string archivoBase64 = Convert.ToBase64String(archivoBytes);

            CargarArchivoBase64 cargar = new CargarArchivoBase64();
            cargar.IdRutaDoc = 0;
            cargar.IdForeCast = 0;
            cargar.IdContrato = IdContrato;
            cargar.Tipo = 1;
            cargar.TipoDocumento = TipoDocumento;
            cargar.Estado = 1;
            cargar.ArchivoBase64 = archivoBase64;
            cargar.NombreArchivo = NumContrato + "_" + fecha + archivo.Extencion;// "MESES_" + fecha + ".xlsx";
            CargaBase64(cargar);

            return Ruta;
        }

        private string GenerarReportePauta(List<DetalleContratoExcel> detalles, IEnumerable<ContratoReporte> reportes, Int64 IdContrato, string TipoDocumento, Int32 TipoProceso)
        {
            string Ruta = "";
            string fecha;
            string NumContrato = "";
            int numeroRegistro = detalles.Count;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, IdContrato, TipoDocumento, TipoProceso);
            string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;// "F:\\PropuestaConexion\\MESES.xlsx";
            string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;// "F:\\PropuestaConexion\\MESES_" + fecha + ".xlsx";
            using var wbook = new XLWorkbook(rutaDocumento);
            int cont = 16;
            int cont2 = 14;
            int ValorPorcentaje = numeroRegistro + 21;

            DateTime dateTime = DateTime.Now;
            var ws = wbook.Worksheet(1);
            ws.Row(16).InsertRowsBelow(numeroRegistro);
            #region NO BORRAR
            //foreach (ContratoReporte s in reportes)
            //{
            //    ws.Cell("C6").Value = s.NumContrato;
            //    ws.Cell("C6").Style.Font.Bold = true;

            //    ws.Cell("C7").Value = s.Anunciante;
            //    ws.Cell("C7").Style.Font.Bold = true;

            //    ws.Cell("C8").Value = s.Agencia;
            //    ws.Cell("C8").Style.Font.Bold = true;

            //    ws.Cell("C9").Value = s.FechaInicio.ToString("yyyy-MM-dd") + " - " + s.FechaFinal.ToString("yyyy-MM-dd");
            //    ws.Cell("C9").Style.Font.Bold = true;

            //    ws.Cell("C10").Value = dateTime.ToString("yyyy-MM-dd");
            //    ws.Cell("C10").Style.Font.Bold = true;

            //    DateTime fechaActual = DateTime.Now;
            //    string mes1 = s.FechaInicio.ToString("MMMM");
            //    ws.Cell("P13").Value = mes1.ToUpper();
            //    ws.Cell("P13").Style.Font.Bold = true;

            //    int valorPor = Convert.ToInt32(s.ComiAgen);

            //    ws.Cell("M"+ ValorPorcentaje.ToString()).Value = valorPor;
            //    ws.Cell("M" + ValorPorcentaje.ToString()).Style.Font.Bold = true;

            //    NumContrato = s.NumContrato;

            //}

            //foreach (DetalleContratoExcel s in detalles)
            //{
            //    if (s.Programa != "" && s.Programa != "PROGRAMA")
            //    {
            //        ws.Cell("B" + cont.ToString()).Value = s.Canal;
            //        ws.Cell("C" + cont.ToString()).Value = s.Programa;
            //        ws.Cell("D" + cont.ToString()).Value = s.Detalle;
            //        ws.Cell("E" + cont.ToString()).Value = s.Derecho;
            //        ws.Cell("F" + cont.ToString()).Value = s.Duracion;
            //        ws.Cell("G" + cont.ToString()).Value = s.Franja;

            //        ws.Cell("H" + cont.ToString()).Value = s.Tarifa;
            //        ws.Cell("I" + cont.ToString()).Value = s.Valor1;
            //        ws.Cell("J" + cont.ToString()).Value = s.Valor2;
            //        ws.Cell("K" + cont.ToString()).Value = s.Valor3;
            //        ws.Cell("L" + cont.ToString()).Value = s.Valor4;
            //        ws.Cell("N" + cont.ToString()).FormulaA1 = "=F"+ cont.ToString() + "*"+ "AU"+ cont.ToString();
            //        ws.Cell("O" + cont.ToString()).FormulaA1 = "=N" + cont.ToString() + "*" + "H" + cont.ToString();
            //        cont++;
            //    }
            //}
            //int countFinal = cont + 2;
            //int countFinall = cont + 3;
            //ws.Cell("N" + countFinall.ToString()).FormulaA1 = "=SUM(N16:N"+ countFinal.ToString() + ")";
            //ws.Cell("O" + countFinall.ToString()).FormulaA1 = "=SUM(O16:O"+ countFinal.ToString() + ")";

            //foreach (DetalleContratoExcel s in detalles)
            //{
            //    ws.Cell("P" + cont2.ToString()).Value = s.data_1;
            //    ws.Cell("Q" + cont2.ToString()).Value = s.data_2;
            //    ws.Cell("R" + cont2.ToString()).Value = s.data_3;
            //    ws.Cell("S" + cont2.ToString()).Value = s.data_4;
            //    ws.Cell("T" + cont2.ToString()).Value = s.data_5;
            //    ws.Cell("U" + cont2.ToString()).Value = s.data_6;
            //    ws.Cell("V" + cont2.ToString()).Value = s.data_7;
            //    ws.Cell("W" + cont2.ToString()).Value = s.data_8;
            //    ws.Cell("X" + cont2.ToString()).Value = s.data_9;
            //    ws.Cell("Y" + cont2.ToString()).Value = s.data_10;
            //    ws.Cell("Z" + cont2.ToString()).Value = s.data_11;
            //    ws.Cell("AA" + cont2.ToString()).Value = s.data_12;
            //    ws.Cell("AB" + cont2.ToString()).Value = s.data_13;
            //    ws.Cell("AC" + cont2.ToString()).Value = s.data_14;
            //    ws.Cell("AD" + cont2.ToString()).Value = s.data_15;
            //    ws.Cell("AE" + cont2.ToString()).Value = s.data_16;
            //    ws.Cell("AF" + cont2.ToString()).Value = s.data_17;
            //    ws.Cell("AG" + cont2.ToString()).Value = s.data_18;
            //    ws.Cell("AH" + cont2.ToString()).Value = s.data_19;
            //    ws.Cell("AI" + cont2.ToString()).Value = s.data_20;
            //    ws.Cell("AJ" + cont2.ToString()).Value = s.data_21;
            //    ws.Cell("AK" + cont2.ToString()).Value = s.data_22;
            //    ws.Cell("AL" + cont2.ToString()).Value = s.data_23;
            //    ws.Cell("AM" + cont2.ToString()).Value = s.data_24;
            //    ws.Cell("AN" + cont2.ToString()).Value = s.data_25;
            //    ws.Cell("AO" + cont2.ToString()).Value = s.data_26;
            //    ws.Cell("AP" + cont2.ToString()).Value = s.data_27;
            //    ws.Cell("AQ" + cont2.ToString()).Value = s.data_28;
            //    ws.Cell("AR" + cont2.ToString()).Value = s.data_29;
            //    ws.Cell("AS" + cont2.ToString()).Value = s.data_30;
            //    ws.Cell("AT" + cont2.ToString()).Value = s.data_31;
            //    ws.Cell("AU" + cont2.ToString()).FormulaA1 = "=SUM(P" + cont2.ToString() + ":AT" + cont2.ToString() + ")"; 
            //    cont2++;
            //}

            //int countFinal2 = cont2 + 4;
            //ws.Cell("N" + cont2.ToString()).FormulaA1 = "SUMA(AU16" + ":AU" + countFinal2.ToString() + ")";
            #endregion

            foreach (ContratoReporte s in reportes)
            {
                ws.Cell("C6").Value = s.NumContrato;
                ws.Cell("C6").Style.Font.Bold = true;

                ws.Cell("C7").Value = s.Anunciante;
                ws.Cell("C7").Style.Font.Bold = true;

                ws.Cell("C8").Value = s.Agencia;
                ws.Cell("C8").Style.Font.Bold = true;

                ws.Cell("C9").Value = s.FechaInicio.ToString("yyyy-MM-dd") + " - " + s.FechaFinal.ToString("yyyy-MM-dd");
                ws.Cell("C9").Style.Font.Bold = true;

                ws.Cell("C10").Value = dateTime.ToString("yyyy-MM-dd");
                ws.Cell("C10").Style.Font.Bold = true;

                DateTime fechaActual = DateTime.Now;
                string mes1 = s.FechaInicio.ToString("MMMM");
                ws.Cell("Q13").Value = mes1.ToUpper();
                ws.Cell("Q13").Style.Font.Bold = true;

                int valorPor = Convert.ToInt32(s.ComiAgen);

                //ws.Cell("M286").Value = valorPor;
                //ws.Cell("M286").Style.Font.Bold = true;
                ws.Cell("N" + ValorPorcentaje.ToString()).Value = valorPor; //ANTES M
                ws.Cell("N" + ValorPorcentaje.ToString()).Style.Font.Bold = true; //ANTES M
                int ivaValor = 0;
                int ivaValor2 = 0;
                ivaValor = ValorPorcentaje + 2;
                ivaValor2 = ValorPorcentaje + 1;

                DateTime fechaIva = Convert.ToDateTime("2024-03-31");
                if (DateTime.Compare(fechaIva, s.FechaInicio) < 0)
                {
                    ws.Cell("N" + ivaValor.ToString()).Value = "(+)  15% IVA"; //ANTES M
                    ws.Cell("N" + ivaValor.ToString()).Style.Font.Bold = true; //ANTES M

                    ws.Cell("P" + ivaValor.ToString()).FormulaA1 = "=P" + ivaValor2.ToString() + "*" + "15%"; //ANTES O
                    ws.Cell("P" + ivaValor.ToString()).Style.Font.Bold = true; //ANTES O
                }

                NumContrato = s.NumContrato;

            }

            foreach (DetalleContratoExcel s in detalles)
            {
                if (s.Programa != "" && s.Programa != "PROGRAMA")
                {
                    ws.Cell("B" + cont.ToString()).Value = s.Canal;
                    ws.Cell("C" + cont.ToString()).Value = s.Programa;
                    ws.Cell("D" + cont.ToString()).Value = s.Detalle;
                    ws.Cell("E" + cont.ToString()).Value = s.Versiones;
                    ws.Cell("F" + cont.ToString()).Value = s.Derecho;
                    ws.Cell("G" + cont.ToString()).Value = s.Duracion;//--F ANTES
                    ws.Cell("H" + cont.ToString()).Value = s.Franja;

                    ws.Cell("I" + cont.ToString()).Value = s.Tarifa;//--H ANTES
                    ws.Cell("J" + cont.ToString()).Value = s.Valor1;
                    ws.Cell("K" + cont.ToString()).Value = s.Valor2;
                    ws.Cell("L" + cont.ToString()).Value = s.Valor3;
                    ws.Cell("M" + cont.ToString()).Value = s.Valor4;
                    ws.Cell("O" + cont.ToString()).FormulaA1 = "=G" + cont.ToString() + "*" + "AV" + cont.ToString(); //--N ANTES
                    ws.Cell("P" + cont.ToString()).FormulaA1 = "=O" + cont.ToString() + "*" + "I" + cont.ToString(); //--O ANTES
                    cont++;
                }
            }
            int countFinal = cont + 2;
            int countFinall = cont + 3;
            ws.Cell("O" + countFinall.ToString()).FormulaA1 = "=SUM(O16:O" + countFinal.ToString() + ")"; //--N ANTES
            ws.Cell("P" + countFinall.ToString()).FormulaA1 = "=SUM(P16:P" + countFinal.ToString() + ")"; //--O ANTES

            foreach (DetalleContratoExcel s in detalles)
            {
                ws.Cell("Q" + cont2.ToString()).Value = s.data_1; //ANTES P
                ws.Cell("R" + cont2.ToString()).Value = s.data_2;
                ws.Cell("S" + cont2.ToString()).Value = s.data_3;
                ws.Cell("T" + cont2.ToString()).Value = s.data_4;
                ws.Cell("U" + cont2.ToString()).Value = s.data_5;
                ws.Cell("V" + cont2.ToString()).Value = s.data_6;
                ws.Cell("W" + cont2.ToString()).Value = s.data_7;
                ws.Cell("X" + cont2.ToString()).Value = s.data_8;
                ws.Cell("Y" + cont2.ToString()).Value = s.data_9;
                ws.Cell("Z" + cont2.ToString()).Value = s.data_10;
                ws.Cell("AA" + cont2.ToString()).Value = s.data_11;
                ws.Cell("AB" + cont2.ToString()).Value = s.data_12;
                ws.Cell("AC" + cont2.ToString()).Value = s.data_13;
                ws.Cell("AD" + cont2.ToString()).Value = s.data_14;
                ws.Cell("AE" + cont2.ToString()).Value = s.data_15;
                ws.Cell("AF" + cont2.ToString()).Value = s.data_16;
                ws.Cell("AG" + cont2.ToString()).Value = s.data_17;
                ws.Cell("AH" + cont2.ToString()).Value = s.data_18;
                ws.Cell("AI" + cont2.ToString()).Value = s.data_19;
                ws.Cell("AJ" + cont2.ToString()).Value = s.data_20;
                ws.Cell("AK" + cont2.ToString()).Value = s.data_21;
                ws.Cell("AL" + cont2.ToString()).Value = s.data_22;
                ws.Cell("AM" + cont2.ToString()).Value = s.data_23;
                ws.Cell("AN" + cont2.ToString()).Value = s.data_24;
                ws.Cell("AO" + cont2.ToString()).Value = s.data_25;
                ws.Cell("AP" + cont2.ToString()).Value = s.data_26;
                ws.Cell("AQ" + cont2.ToString()).Value = s.data_27;
                ws.Cell("AR" + cont2.ToString()).Value = s.data_28;
                ws.Cell("AS" + cont2.ToString()).Value = s.data_29;
                ws.Cell("AT" + cont2.ToString()).Value = s.data_30;
                ws.Cell("AU" + cont2.ToString()).Value = s.data_31;  //--AT ANTES
                ws.Cell("AV" + cont2.ToString()).FormulaA1 = "=SUM(Q" + cont2.ToString() + ":AU" + cont2.ToString() + ")";
                cont2++;
            }


            int countFinal2 = cont2 + 2;
            int countFinall2 = cont2 + 3;
            ws.Cell("Q" + countFinall2.ToString()).FormulaA1 = "SUM(Q16:Q" + countFinal2.ToString() + ")";
            ws.Cell("R" + countFinall2.ToString()).FormulaA1 = "SUM(R16:R" + countFinal2.ToString() + ")";
            ws.Cell("S" + countFinall2.ToString()).FormulaA1 = "SUM(S16:S" + countFinal2.ToString() + ")";
            ws.Cell("T" + countFinall2.ToString()).FormulaA1 = "SUM(T16:T" + countFinal2.ToString() + ")";
            ws.Cell("U" + countFinall2.ToString()).FormulaA1 = "SUM(U16:U" + countFinal2.ToString() + ")";
            ws.Cell("V" + countFinall2.ToString()).FormulaA1 = "SUM(V16:V" + countFinal2.ToString() + ")";
            ws.Cell("W" + countFinall2.ToString()).FormulaA1 = "SUM(W16:W" + countFinal2.ToString() + ")";
            ws.Cell("X" + countFinall2.ToString()).FormulaA1 = "SUM(X16:X" + countFinal2.ToString() + ")";
            ws.Cell("Y" + countFinall2.ToString()).FormulaA1 = "SUM(Y16:Y" + countFinal2.ToString() + ")";
            ws.Cell("Z" + countFinall2.ToString()).FormulaA1 = "SUM(Z16:Z" + countFinal2.ToString() + ")";
            ws.Cell("AA" + countFinall2.ToString()).FormulaA1 = "SUM(AA16:AA" + countFinal2.ToString() + ")";
            ws.Cell("AB" + countFinall2.ToString()).FormulaA1 = "SUM(AB16:AB" + countFinal2.ToString() + ")";
            ws.Cell("AC" + countFinall2.ToString()).FormulaA1 = "SUM(AC16:AC" + countFinal2.ToString() + ")";
            ws.Cell("AD" + countFinall2.ToString()).FormulaA1 = "SUM(AD16:AD" + countFinal2.ToString() + ")";
            ws.Cell("AE" + countFinall2.ToString()).FormulaA1 = "SUM(AE16:AE" + countFinal2.ToString() + ")";
            ws.Cell("AF" + countFinall2.ToString()).FormulaA1 = "SUM(AF16:AF" + countFinal2.ToString() + ")";
            ws.Cell("AG" + countFinall2.ToString()).FormulaA1 = "SUM(AG16:AG" + countFinal2.ToString() + ")";
            ws.Cell("AH" + countFinall2.ToString()).FormulaA1 = "SUM(AH16:AH" + countFinal2.ToString() + ")";
            ws.Cell("AI" + countFinall2.ToString()).FormulaA1 = "SUM(AI16:AI" + countFinal2.ToString() + ")";
            ws.Cell("AJ" + countFinall2.ToString()).FormulaA1 = "SUM(AJ16:AJ" + countFinal2.ToString() + ")";
            ws.Cell("AK" + countFinall2.ToString()).FormulaA1 = "SUM(AK16:AK" + countFinal2.ToString() + ")";
            ws.Cell("AL" + countFinall2.ToString()).FormulaA1 = "SUM(AL16:AL" + countFinal2.ToString() + ")";
            ws.Cell("AM" + countFinall2.ToString()).FormulaA1 = "SUM(AM16:AM" + countFinal2.ToString() + ")";
            ws.Cell("AN" + countFinall2.ToString()).FormulaA1 = "SUM(AN16:AN" + countFinal2.ToString() + ")";
            ws.Cell("AO" + countFinall2.ToString()).FormulaA1 = "SUM(AO16:AO" + countFinal2.ToString() + ")";
            ws.Cell("AP" + countFinall2.ToString()).FormulaA1 = "SUM(AP16:AP" + countFinal2.ToString() + ")";
            ws.Cell("AQ" + countFinall2.ToString()).FormulaA1 = "SUM(AQ16:AQ" + countFinal2.ToString() + ")";
            ws.Cell("AR" + countFinall2.ToString()).FormulaA1 = "SUM(AR16:AR" + countFinal2.ToString() + ")";
            ws.Cell("AS" + countFinall2.ToString()).FormulaA1 = "SUM(AS16:AS" + countFinal2.ToString() + ")";
            ws.Cell("AT" + countFinall2.ToString()).FormulaA1 = "SUM(AT16:AT" + countFinal2.ToString() + ")";
            ws.Cell("AU" + countFinall2.ToString()).FormulaA1 = "SUM(AU16:AU" + countFinal2.ToString() + ")";
            ws.Cell("AV" + countFinall2.ToString()).FormulaA1 = "SUM(AV16:AV" + countFinal2.ToString() + ")";


            //wbook.SaveAs(rutaDocumentoResul);
            //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
            byte[] archivoBytes = null;
            using (var msA = new MemoryStream())
            {
                wbook.SaveAs(msA);
                archivoBytes = msA.ToArray();
            }
            string archivoBase64 = Convert.ToBase64String(archivoBytes);

            CargarArchivoBase64 cargar = new CargarArchivoBase64();
            cargar.IdRutaDoc = 0;
            cargar.IdForeCast = 0;
            cargar.IdContrato = IdContrato;
            cargar.Tipo = 1;
            cargar.TipoDocumento = TipoDocumento;
            cargar.Estado = 1;
            cargar.ArchivoBase64 = archivoBase64;
            cargar.NombreArchivo = NumContrato + "_" + fecha + archivo.Extencion;// "MESES_" + fecha + ".xlsx";
            CargaBase64(cargar);

            return Ruta;
        }

    }
}
