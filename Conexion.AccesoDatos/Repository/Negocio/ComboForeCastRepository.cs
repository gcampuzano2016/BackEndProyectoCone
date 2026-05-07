using Conexion.Entidad.Administracion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.AccesoDatos.Repository.Negocio
{
   public  class ComboForeCastRepository
    {
        private readonly string _connectionString;

        public ComboForeCastRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Combo>> GetByMostrarComboForeCast(Int32 Tipo, Int64 IdCliente )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarComboForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", IdCliente));
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

        public async Task<IEnumerable<Combo>> GetByMostrarDescripcionCombo(Int32 Tipo, Int64 IdProceso,string Descripcion)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarDescripcionCombo", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    cmd.Parameters.Add(new SqlParameter("@IdProceso", IdProceso));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
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

        public async Task<IEnumerable<ReferenciaCliente>> GetByMostrarComboForeCastCliente(Int32 Tipo, Int64 IdCliente, Int64 IdMedio, Int64 IdAgencia)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarComboForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", IdCliente));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdAgencia", IdAgencia));
                    var response = new List<ReferenciaCliente>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReferencia(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Combo>> GetByMostrarComboForeCastClienteAgencia(Int32 Tipo, Int64 IdCliente, Int64 IdMedio, Int64 IdAgencia)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarComboForeCast", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", IdCliente));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdAgencia", IdAgencia));
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

        private ReferenciaCliente MapToReferencia(SqlDataReader reader)
        {
            return new ReferenciaCliente()
            {
                IdProceso = (Int64)reader["IdProceso"],
                Descripcion = reader["Descripcion"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                Comision = (Int64)reader["Comision"],
            };
        }
    }
}
