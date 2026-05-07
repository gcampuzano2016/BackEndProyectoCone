using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.AccesoDatos.Repository.Negocio
{
    public class DocumentoProcesadosRepository
    {
        private readonly string _connectionString;

        public DocumentoProcesadosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<DocumentoProcesados>> GetByMostrarDocumentosProcesados(DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarDocumentosProcesados", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<DocumentoProcesados>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDocumentoProcesados(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public string DevolverArchivoBase64(string rutaDocumentoResul)
        {
            string StringBase64 = "";
            if (File.Exists(rutaDocumentoResul))
            {
                byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                StringBase64 = Convert.ToBase64String(archivoBytes);
            }
            return StringBase64;
        }

        private DocumentoProcesados MapToDocumentoProcesados(SqlDataReader reader)
        {


            return new DocumentoProcesados()
            {
                id = (Int32)reader["id"],
                ruc = reader["ruc"].ToString(),
                razonsocial = reader["razonsocial"].ToString(),
                estado = reader["estado"].ToString(),
                claveacceso = reader["claveacceso"].ToString(),
                fechaautorizacion = (DateTime)reader["fechaautorizacion"],
                autorizacionsri = reader["autorizacionsri"].ToString(),
                subtotalsinimpuesto = (decimal)reader["subtotalsinimpuesto"],
                iva = (decimal)reader["iva"],
                totalfactura = (decimal)reader["totalfactura"],
                error = reader["error"].ToString(),
                ruta = reader["ruta"].ToString(),
                tipocomprobante = reader["tipocomprobante"].ToString(),
                stringArchivo64 = DevolverArchivoBase64(reader["ruta"].ToString()),
            };
        }
    }
}
