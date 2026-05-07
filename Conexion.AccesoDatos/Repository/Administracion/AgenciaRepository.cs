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
    public class AgenciaRepository
    {
        private readonly string _connectionString;

        public AgenciaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> Insert(ComboLlenar combo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarAgencia", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdAgencia", combo.IdProceso));
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

        public async Task<IEnumerable<Generica>> Modificar(ClienteAgencia clienteAgencia)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarClienteAgencia", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdClienteAgencia", clienteAgencia.IdClienteAgencia));
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", clienteAgencia.IdCliente));
                    cmd.Parameters.Add(new SqlParameter("@IdAgencia", clienteAgencia.IdAgencia));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", clienteAgencia.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@Contacto", clienteAgencia.Contacto));
                    cmd.Parameters.Add(new SqlParameter("@Telefono", clienteAgencia.Telefono));
                    cmd.Parameters.Add(new SqlParameter("@Email", clienteAgencia.Email));
                    cmd.Parameters.Add(new SqlParameter("@Estado", clienteAgencia.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", clienteAgencia.Tipo));
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

        public async Task<IEnumerable<ClienteAgencia>> GetByMostrarClienteAgencia(Int64 IdCliente,Int64 IdAgencia, Int32 Tipo)
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

        private Generica MapToGenerica(SqlDataReader reader)
        {
            return new Generica()
            {
                valor1 = (Int16)reader["valor1"],
                valor2 = reader["valor2"].ToString()
            };
        }

        private ClienteAgencia  MapToClienteAgencia(SqlDataReader reader)
        {
            return new ClienteAgencia()
            {
                IdClienteAgencia = (Int64)reader["IdClienteAgencia"],
                IdCliente = (Int64)reader["IdCliente"],
                IdAgencia = (Int64)reader["IdAgencia"],
                Descripcion = reader["Descripcion"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                Telefono = reader["Telefono"].ToString(),
                Email = reader["Email"].ToString()
            };
        }

    }
}
