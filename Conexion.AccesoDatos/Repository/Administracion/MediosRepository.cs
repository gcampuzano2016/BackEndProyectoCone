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
    public class MediosRepository
    {
        private readonly string _connectionString;

        public MediosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> Insert(Medios medios)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarMedios", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", medios.IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Medios", medios.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@RuCedula", medios.RuCedula));
                    cmd.Parameters.Add(new SqlParameter("@CodPais", medios.CodPais));
                    cmd.Parameters.Add(new SqlParameter("@ComisionAgencia", medios.ComisionAgencia));
                    cmd.Parameters.Add(new SqlParameter("@ComisionCone", medios.ComisionCone));
                    cmd.Parameters.Add(new SqlParameter("@Iva", medios.Iva));
                    cmd.Parameters.Add(new SqlParameter("@FormaPago", medios.FormaPago));
                    cmd.Parameters.Add(new SqlParameter("@Direccion", medios.Direccion));
                    cmd.Parameters.Add(new SqlParameter("@Telefono", medios.Telefono));
                    cmd.Parameters.Add(new SqlParameter("@Contacto", medios.Contacto));
                    cmd.Parameters.Add(new SqlParameter("@Correo", medios.Correo));
                    cmd.Parameters.Add(new SqlParameter("@Estado", medios.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", medios.Tipo));
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

        public async Task<IEnumerable<Medios>> GetByMostrarMedio(Int64 IdMedio, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarMedio", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Medios>();
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

        private Medios MapToMedios(SqlDataReader reader)
        {
            return new Medios()
            {
                IdMedio = (Int64)reader["IdMedio"],
                Descripcion = reader["Medios"].ToString(),
                RuCedula = reader["RuCedula"].ToString(),
                CodPais = reader["CodPais"].ToString(),
                ComisionAgencia = (decimal)reader["ComisionAgencia"],
                ComisionCone = (decimal)reader["ComisionCone"],
                Iva = (Int32)reader["Iva"],
                FormaPago = reader["FormaPago"].ToString(),
                Direccion = reader["Direccion"].ToString(),
                Telefono = reader["Telefono"].ToString(),
                Contacto = reader["Contacto"].ToString(),
                Correo = reader["Correo"].ToString(),
                Estado = (Int32)reader["Estado"],
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
