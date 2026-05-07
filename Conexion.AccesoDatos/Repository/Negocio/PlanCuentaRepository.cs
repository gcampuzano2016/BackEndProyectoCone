using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.AccesoDatos.Repository.Negocio
{
    public class PlanCuentaRepository
    {
        private readonly string _connectionString;

        public PlanCuentaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Generica>> Insert(PlanCuenta planCuenta)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarPlanCuentas", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", planCuenta.IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@IdPadre", planCuenta.IdPadre));
                    cmd.Parameters.Add(new SqlParameter("@Codigo", planCuenta.Codigo));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", planCuenta.Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@SaldoInicial", planCuenta.SaldoInicial));
                    cmd.Parameters.Add(new SqlParameter("@Debe", planCuenta.Debe));
                    cmd.Parameters.Add(new SqlParameter("@Haber", planCuenta.Haber));
                    cmd.Parameters.Add(new SqlParameter("@SaldoFinal", planCuenta.SaldoFinal));                   
                    cmd.Parameters.Add(new SqlParameter("@Estado", planCuenta.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", planCuenta.Tipo));
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

        public async Task<IEnumerable<PlanCuenta>> GetByMostrarPlanCuentas(Int64 IdPlanCuenta, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPlanCuentas", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PlanCuenta>();
                    await sql.OpenAsync();

                    if(Tipo==2)
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToCodigoNuevo(reader));
                            }
                        }
                    }
                    else
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToPlanCuentas(reader));
                            }
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<UnionIngresoEgreso>> GetByMostrarUnionIngresoEgreso(Int64 IdProcesoContable, DateTime FechaInicio,DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarRegistroContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProcesoContable", IdProcesoContable));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<UnionIngresoEgreso>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToUnionIngresoEgreso(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<EstadoCuenta>> GetByMostrarReporteEstadoCuenta(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteEstadoCuenta", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<EstadoCuenta>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToEstadoCuenta(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<RegistroContable>> GetByMostrarRegistroContable(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarRegistroContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProcesoContable", IdProcesoContable));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<RegistroContable>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToRegistroContable(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<RegistroContable>> GetByMostrarMovimientoContable(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarRegistroContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProcesoContable", IdProcesoContable));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<RegistroContable>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToMovimientoContable(reader));
                        }
                    }

                    return response;
                }
            }
        }


        public async Task<IEnumerable<BalanceComprobacion>> GetByMostrarReporteBalanceComprobacion(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteBalanceComprobacion", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<BalanceComprobacion>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToBalanceComprobacion(reader));
                        }
                    }

                    return response;
                }
            }
        }


        private PlanCuenta MapToPlanCuentas(SqlDataReader reader)
        {
            return new PlanCuenta()
            {
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                Codigo = reader["Codigo"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                SaldoInicial = (decimal)reader["SaldoInicial"],
                Debe = (decimal)reader["Debe"],
                Haber = (decimal)reader["Haber"],
                SaldoFinal = (decimal)reader["SaldoFinal"],
                Estado = (Int32)reader["Estado"],
            };
        }

        private UnionIngresoEgreso MapToUnionIngresoEgreso(SqlDataReader reader)
        {
            return new UnionIngresoEgreso()
            {
                IdProcesoContable = (Int64)reader["IdProcesoContable"],
                Venta = (Int64)reader["Venta"],
                Compra = (Int64)reader["Compra"],
                Pagos = (Int64)reader["pagos"],
                Cobros = (Int64)reader["Cobros"],
                TipoTransaccion = reader["TipoTransaccion"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                Valor = (decimal)reader["Valor"],
                FechaRegistro = (DateTime)reader["FechaRegistro"]
            };
        }

        private EstadoCuenta MapToEstadoCuenta(SqlDataReader reader)
        {
            return new EstadoCuenta()
            {
                id = (Int64)reader["id"],
                IdRegistro = (Int64)reader["IdRegistro"],
                Fecha = reader["Fecha"].ToString(),
                Debito = (decimal)reader["Debito"],
                Credito = (decimal)reader["Credito"],
                Concepto = reader["Concepto"].ToString(),
                Saldo = (decimal)reader["Saldo"],
            };
        }

        private BalanceComprobacion MapToBalanceComprobacion(SqlDataReader reader)
        {
            return new BalanceComprobacion()
            {
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                CODIGO = reader["CODIGO"].ToString(),
                CUENTA = reader["CUENTA"].ToString(),
                INICIAL = (decimal)reader["INICIAL"],
                DEBITOS = (decimal)reader["DEBITOS"],
                CREDITOS = (decimal)reader["CREDITOS"],
                SALDOFINAL = (decimal)reader["SALDOFINAL"],
            };
        }

        private RegistroContable MapToRegistroContable(SqlDataReader reader)
        {
            return new RegistroContable()
            {
                IdRegistro = (Int64)reader["IdRegistro"],
                IdProcesoContable = (Int64)reader["IdProcesoContable"],
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                Codigo = reader["Codigo"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                Debe = (decimal)reader["Debe"],
                Haber = (decimal)reader["Haber"],
                FechaRegistro = (DateTime)reader["FechaRegistro"]
            };
        }

        private RegistroContable MapToMovimientoContable(SqlDataReader reader)
        {
            return new RegistroContable()
            {
                IdRegistro = (Int64)reader["IdRegistro"],
                IdProcesoContable = (Int64)reader["IdProcesoContable"],
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                TipoTransaccion = reader["TipoTRansaccion"].ToString(),
                Fecha = reader["Fecha"].ToString(),
                Concepto = reader["Concepto"].ToString(),
                Codigo = reader["Codigo"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                Debe = (decimal)reader["Debe"],
                Haber = (decimal)reader["Haber"],
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                Proceso = reader["Proceso"].ToString(),
            };
        }

        private PlanCuenta MapToCodigoNuevo(SqlDataReader reader)
        {
            return new PlanCuenta()
            {
                Codigo = reader["Codigo"].ToString(),
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
