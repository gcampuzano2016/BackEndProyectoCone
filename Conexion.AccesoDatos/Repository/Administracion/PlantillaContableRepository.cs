using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.AccesoDatos.Repository.Administracion
{
    public class PlantillaContableRepository
    {
        private readonly string _connectionString;

        public PlantillaContableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> Insert(PlantillaContable plantilla)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarPlantilla", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlantilla", plantilla.IdPlantilla));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", plantilla.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@json", plantilla.json));
                    cmd.Parameters.Add(new SqlParameter("@Estado", plantilla.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", plantilla.Tipo));
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

        public async Task<IEnumerable<DetallePlantilla>> GetByMostrarPlantilla(Int64 IdPlantilla, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPlantilla", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlantilla", IdPlantilla));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<DetallePlantilla>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPLantilla(reader));
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

        private DetallePlantilla MapToPLantilla(SqlDataReader reader)
        {
            return new DetallePlantilla()
            {
                IdPlantilla = (Int64)reader["IdPlantilla"],
                IdDetalle = (Int64)reader["IdDetalle"],
                Descripcion = reader["Descripcion"].ToString(),
                Tipo = reader["Tipo"].ToString(),
                Estado = (Int32)reader["Estado"],
            };
        }

    }
}
