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
    public class ComboMediosRepository
    {
        private readonly string _connectionString;

        public ComboMediosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Combo>> GetByMostrarMedioRelacionado(RelacionMedios relacionMedios)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarMedioRelacionado", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", relacionMedios.IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdCanal", relacionMedios.IdCanal));
                    cmd.Parameters.Add(new SqlParameter("@IdPrograma", relacionMedios.IdPrograma));
                    cmd.Parameters.Add(new SqlParameter("@IdDerecho", relacionMedios.IdDerecho));
                    cmd.Parameters.Add(new SqlParameter("@IdFormato", relacionMedios.IdFormato));
                    cmd.Parameters.Add(new SqlParameter("@IdUnidad", relacionMedios.IdUnidad));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", relacionMedios.Tipo));
                    var response = new List<Combo>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToMedios(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Combo MapToMedios(SqlDataReader reader)
        {
            return new Combo()
            {
                IdProceso = (Int64)reader["IdProceso"],
                Descripcion = reader["Descripcion"].ToString()
            };
        }

    }
}
