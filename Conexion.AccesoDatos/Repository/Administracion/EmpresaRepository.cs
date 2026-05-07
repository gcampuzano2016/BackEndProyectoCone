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
    public class EmpresaRepository
    {
        private readonly string _connectionString;

        public EmpresaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Empresa>> GetByMostrarEmpresa(Int64 IdEmpresa, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarEmpresa", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpresa", IdEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Empresa>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToEmpresa(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Empresa MapToEmpresa(SqlDataReader reader)
        {
            return new Empresa()
            {
                ID = (Int32)reader["ID"],
                RUC = reader["RUC"].ToString(),
                RazonSocial = reader["RazonSocial"].ToString(),
                NombreComercial = reader["NombreComercial"].ToString(),
                NumResolucion = reader["NumResolucion"].ToString(),
                Telefono = reader["Telefono"].ToString(),
                Direccion = reader["Direccion"].ToString(),
            };
        }


    }
}
