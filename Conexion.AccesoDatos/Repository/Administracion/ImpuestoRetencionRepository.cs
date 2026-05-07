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
    public class ImpuestoRetencionRepository
    {
        private readonly string _connectionString;

        public ImpuestoRetencionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Impuesto>> GetByMostrarImpuestos(Int32 IDImpuesto, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarImpuestos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IDImpuesto", IDImpuesto));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Impuesto>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToImpuesto(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Impuesto MapToImpuesto(SqlDataReader reader)
        {
            return new Impuesto()
            {
                ID = (Int32)reader["ID"],
                Codigo = reader["Codigo"].ToString(),
                Nombre = reader["Nombre"].ToString(),
                CodigoSRI = reader["CodigoSRI"].ToString(),
                Porcentaje = (decimal)reader["Porcentaje"],
            };
        }

    }
}
