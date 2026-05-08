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

   public class ComisionPresupuestoRepository
    {
        private readonly string _connectionString;

        public ComisionPresupuestoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }
        public async Task<IEnumerable<Generica>> InsertComision(Comision comision)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarComision", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdComision", comision.IdComision));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", comision.IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", comision.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@CumpInicio", comision.CumpInicio));
                    cmd.Parameters.Add(new SqlParameter("@CumpFinal", comision.CumpFinal));
                    cmd.Parameters.Add(new SqlParameter("@Comision", comision.Comisions));
                    cmd.Parameters.Add(new SqlParameter("@Participacion", comision.Participacion));
                    cmd.Parameters.Add(new SqlParameter("@AnioComision", comision.AnioComision));
                    cmd.Parameters.Add(new SqlParameter("@Generico", comision.Generico));
                    cmd.Parameters.Add(new SqlParameter("@Estado", comision.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", comision.Tipo));
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
        public async Task<IEnumerable<Generica>> InsertPresupuesto(PresupuestoMedios presupuesto)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarPresupuestoMedios", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPresupuesto", presupuesto.IdPresupuesto));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", presupuesto.IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", presupuesto.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@ValorPresupuesto", presupuesto.ValorPresupuesto));
                    cmd.Parameters.Add(new SqlParameter("@AnioPresupuesto", presupuesto.AnioPresupuesto));
                    cmd.Parameters.Add(new SqlParameter("@Generico", presupuesto.Generico));
                    cmd.Parameters.Add(new SqlParameter("@json", presupuesto.json));
                    cmd.Parameters.Add(new SqlParameter("@Estado", presupuesto.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", presupuesto.Tipo));
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

        public async Task<IEnumerable<PresupuestoMedios>> GetByMostrarPresupuestoMedios(Int64 IdPresupuesto, Int64 IdMedio, Int64 IdEmpleado, Int32 mes, Int32 anio, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPresupuestoMedios", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPresupuesto", IdPresupuesto));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@mes", mes));
                    cmd.Parameters.Add(new SqlParameter("@anio", anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PresupuestoMedios>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPresupuesto(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Comision>> GetByMostrarComision(Int64 IdComision, Int64 IdMedio, Int64 IdEmpleado, Int32 mes, Int32 anio, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarComision", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdComision", IdComision));
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@mes", mes));
                    cmd.Parameters.Add(new SqlParameter("@anio", anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Comision>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToComision(reader));
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

        private PresupuestoMedios MapToPresupuesto(SqlDataReader reader)
        {
            return new PresupuestoMedios()
            {
                IdPresupuesto = (Int64)reader["IdPresupuesto"],
                IdMedio = (Int64)reader["IdMedio"],
                IdEmpleado = (Int64)reader["IdEmpleado"],
                ValorPresupuesto = (decimal)reader["ValorPresupuesto"],
                AnioPresupuesto = (DateTime)reader["AnioPresupuesto"],
                Estado = (Int32)reader["Estado"],
                Medios = reader["Medios"].ToString(),
                Vendedor = reader["Vendedor"].ToString(),
                Generico = reader["Generico"].ToString(),
                JsonMedio = reader["JsonMedio"].ToString(),
                JsonEmpleado = reader["JsonEmpleado"].ToString(),
                JsonGenerico = reader["JsonGenerico"].ToString(),
            };
        }

        private Comision MapToComision(SqlDataReader reader)
        {
            return new Comision()
            {
                IdComision = (Int64)reader["IdComision"],
                IdMedio = (Int64)reader["IdMedio"],
                IdEmpleado = (Int64)reader["IdEmpleado"],
                CumpInicio = (decimal)reader["CumpInicio"],
                CumpFinal = (decimal)reader["CumpFinal"],
                Comisions = (decimal)reader["Comision"],
                Participacion = (decimal)reader["Participacion"],
                AnioComision = (DateTime)reader["AnioComision"],
                Generico = reader["Generico"].ToString(),
                Estado = (Int32)reader["Estado"],
                Medios = reader["Medios"].ToString(),
                JsonMedio = reader["JsonMedio"].ToString(),
                JsonEmpleado = reader["JsonEmpleado"].ToString(),
                JsonGenerico = reader["JsonGenerico"].ToString(),
                Vendedor = reader["Vendedor"].ToString(),
            };
        }
    }
}
