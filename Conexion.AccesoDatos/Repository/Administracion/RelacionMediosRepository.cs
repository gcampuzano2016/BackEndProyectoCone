using Conexion.Entidad.Administracion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.AccesoDatos.Repository.Administracion
{
    public class RelacionMediosRepository
    {
        private readonly string _connectionString;

        public RelacionMediosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> Insert(CargaMasiva cargaMasiva)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarRelacionMediosMasivo", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProceso", cargaMasiva.IdProceso));
                    cmd.Parameters.Add(new SqlParameter("@TipoRegistro", cargaMasiva.TipoRegistro));
                    cmd.Parameters.Add(new SqlParameter("@TipoAccion", cargaMasiva.TipoAccion ));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", cargaMasiva.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", cargaMasiva.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", cargaMasiva.Tipo));
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

        public async Task<IEnumerable<Generica>> InsertRelacion(RelacionMedios relacionMedios)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarRelacionMedios", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRelacion", relacionMedios.IdRelacion));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", relacionMedios.IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdCanal", relacionMedios.IdCanal));
                    cmd.Parameters.Add(new SqlParameter("@IdPrograma", relacionMedios.IdPrograma));
                    cmd.Parameters.Add(new SqlParameter("@IdDerecho", relacionMedios.IdDerecho));
                    cmd.Parameters.Add(new SqlParameter("@IdFormato", relacionMedios.IdFormato));
                    cmd.Parameters.Add(new SqlParameter("@IdUnidad", relacionMedios.IdUnidad));
                    cmd.Parameters.Add(new SqlParameter("@Generico", relacionMedios.Generico));
                    cmd.Parameters.Add(new SqlParameter("@Estado", relacionMedios.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", relacionMedios.Tipo));
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

        private Generica MapToGenerica(SqlDataReader reader)
        {
            return new Generica()
            {
                valor1 = (Int16)reader["valor1"],
                valor2 = reader["valor2"].ToString()
            };
        }
    }
}
