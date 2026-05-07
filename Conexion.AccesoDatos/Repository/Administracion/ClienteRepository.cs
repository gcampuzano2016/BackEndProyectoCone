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
    public   class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> Insert(ComboLlenar combo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarCliente", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", combo.IdProceso));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", combo.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Contacto", combo.Contacto));
                    cmd.Parameters.Add(new SqlParameter("@Telefono", combo.Telefono));
                    cmd.Parameters.Add(new SqlParameter("@Email", combo.Email));
                    cmd.Parameters.Add(new SqlParameter("@Estado", combo.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", combo.Tipo));
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

        public async Task<IEnumerable<ClienteAgencia>> GetByMostrarCliente(Int64 IdCliente, Int64 IdAgencia, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarClienteAgencia", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", IdCliente));
                    cmd.Parameters.Add(new SqlParameter("@IdAgencia", IdAgencia));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ClienteAgencia>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToClienteAgencia(reader));
                        }
                    }

                    return response;
                }
            }
        }


        private ClienteAgencia MapToClienteAgencia(SqlDataReader reader)
        {
            return new ClienteAgencia()
            {
                IdCliente = (Int64)reader["IdCliente"],
                Descripcion = reader["Descripcion"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                Telefono = reader["Telefono"].ToString(),
                Email = reader["Email"].ToString()
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
    }
}
