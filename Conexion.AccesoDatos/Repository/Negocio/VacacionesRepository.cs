using Conexion.AccesoDatos.Repository.Administracion;
using Conexion.AccesoDatos.Repository.CArchivo;
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
    public class VacacionesRepository
    {
        private readonly string _connectionString;

        public VacacionesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<GenericaVP>> InsertVacaciones(Vacaciones vacaciones)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarVacaciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdVacaciones", vacaciones.IdVacaciones));
                    cmd.Parameters.Add(new SqlParameter("@IdTipoSolicitud", vacaciones.IdTipoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", vacaciones.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaRegistro", vacaciones.FechaRegistro));
                    cmd.Parameters.Add(new SqlParameter("@Cedula", vacaciones.Cedula));
                    cmd.Parameters.Add(new SqlParameter("@Colaborador", vacaciones.Colaborador));
                    cmd.Parameters.Add(new SqlParameter("@Departamento", vacaciones.Departamento));
                    cmd.Parameters.Add(new SqlParameter("@JefeInmediato", vacaciones.JefeInmediato));
                    cmd.Parameters.Add(new SqlParameter("@Remplazo", vacaciones.Remplazo));
                    cmd.Parameters.Add(new SqlParameter("@FechaDesde", vacaciones.FechaDesde));
                    cmd.Parameters.Add(new SqlParameter("@FechaHasta", vacaciones.FechaHasta));
                    cmd.Parameters.Add(new SqlParameter("@TotalDias", vacaciones.TotalDias));
                    cmd.Parameters.Add(new SqlParameter("@Feriado", vacaciones.Feriado));
                    cmd.Parameters.Add(new SqlParameter("@SaldoDias", vacaciones.SaldoDias));
                    cmd.Parameters.Add(new SqlParameter("@CargoVacaciones", vacaciones.CargoVacaciones));
                    cmd.Parameters.Add(new SqlParameter("@Horas", vacaciones.Horas));
                    cmd.Parameters.Add(new SqlParameter("@Actividad", vacaciones.Actividad));
                    cmd.Parameters.Add(new SqlParameter("@Observacion", vacaciones.Observacion));
                    cmd.Parameters.Add(new SqlParameter("@EstadoSolicitud", vacaciones.EstadoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@FechaAprobacion", vacaciones.FechaAprobacion));
                    cmd.Parameters.Add(new SqlParameter("@FechaRechazo", vacaciones.FechaRechazo));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioAprobo", vacaciones.UsuarioAprobo));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioRechazo", vacaciones.UsuarioRechazo));
                    cmd.Parameters.Add(new SqlParameter("@Estado", vacaciones.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Ruta_Archivo", vacaciones.Ruta_Archivo));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion_Archivo", vacaciones.Descripcion_Archivo));
                    cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", vacaciones.MotivoAnulacion));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", vacaciones.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    var response = new List<GenericaVP>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGenericaVP(reader));
                        }
                    }

                    Int64 IdVacaciones = 0;
                    foreach (GenericaVP generica1 in response)
                    {
                        IdVacaciones = generica1.IdVacaciones;
                    }

                    FiltroVacaciones filtro = new FiltroVacaciones();
                    filtro.IdVacaciones = IdVacaciones;
                    filtro.IdEmpleado = vacaciones.IdEmpleado;
                    filtro.FechaDesde = DateTime.Now;
                    filtro.FechaHasta = DateTime.Now;
                    filtro.IdTipoSolicitud = 0;
                    filtro.EstadoSolicitud = "";
                    filtro.Filtro = 0;
                    filtro.Tipo = 3;
                    var response3 = await GetByMostrarVacacionesUnificado(filtro);
                    string TipoVacaciones = "";
                    Notificacion notificacion = new Notificacion();
                    foreach (Vacaciones generica1 in response3)
                    {
                        TipoVacaciones = generica1.Descripcion;
                    }
                    if (TipoVacaciones == "VACACIONES")
                    {
                        notificacion.GenerarNotificacionVacaciones(response3, "SOLICITUD VACACIONES", _connectionString);
                    }
                    else if (TipoVacaciones == "PERMISO")
                    {
                        string StrObservacion = "";
                        if(vacaciones.Observacion != "")
                        {
                            StrObservacion = vacaciones.Observacion;
                        }
                        else if(vacaciones.Actividad != "")
                        {
                            StrObservacion = vacaciones.Actividad;
                        }
                        notificacion.GenerarNotificacionPermiso(response3, "SOLICITUD PERMISO", _connectionString, StrObservacion);
                    }

                    //Enviar Notificacion

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Generica>> InsertSaldoVacaciones(SaldoVacaciones saldoVacaciones)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarSaldoVacaciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdSaldoVacaciones", saldoVacaciones.IdSaldoVacaciones));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", saldoVacaciones.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@PeriodoInicio", saldoVacaciones.PeriodoInicio));
                    cmd.Parameters.Add(new SqlParameter("@PeriodoFinal", saldoVacaciones.PeriodoFinal));
                    cmd.Parameters.Add(new SqlParameter("@DiasGenerados", saldoVacaciones.DiasGenerados));
                    cmd.Parameters.Add(new SqlParameter("@DiasTomados", saldoVacaciones.DiasTomados));
                    cmd.Parameters.Add(new SqlParameter("@Saldo", saldoVacaciones.Saldo));
                    cmd.Parameters.Add(new SqlParameter("@Estado", saldoVacaciones.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", saldoVacaciones.Tipo));
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

        public async Task<IEnumerable<Generica>> InsertAprobacionSolicitud(AprobarSolicitud aprobar)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarAprobacionSolicitud", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdVacaciones", aprobar.IdVacaciones));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", aprobar.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@IdTipoSolicitud", aprobar.IdTipoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@EstadoSolicitud", aprobar.EstadoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@Usuario", aprobar.Usuario));
                    cmd.Parameters.Add(new SqlParameter("@MotivoAnulacion", aprobar.MotivoAnulacion));
                    cmd.Parameters.Add(new SqlParameter("@Estado", aprobar.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", aprobar.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    var response = new List<Generica>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGenerica(reader));
                        }

                        //Enviar Notificacion
                        #region Enviar Notificacion
                        int IdProceso = 0;
                        foreach (Generica generica1 in response)
                        {
                            IdProceso = generica1.valor1;
                        }
                        if (IdProceso == 1)
                        {
                            FiltroVacaciones filtro = new FiltroVacaciones();
                            filtro.IdVacaciones = aprobar.IdVacaciones;
                            filtro.IdEmpleado = aprobar.IdEmpleado;
                            filtro.FechaDesde = DateTime.Now;
                            filtro.FechaHasta = DateTime.Now;
                            filtro.IdTipoSolicitud = 0;
                            filtro.EstadoSolicitud = "";
                            filtro.Filtro = 0;
                            filtro.Tipo = 3;
                            var response3 = await GetByMostrarVacacionesUnificado(filtro);
                            Vacaciones vacaciones = new Vacaciones();
                            string TipoVacaciones = "";
                            Notificacion notificacion = new Notificacion();
                            foreach (Vacaciones generica1 in response3)
                            {
                                TipoVacaciones = generica1.Descripcion;
                            }
                            if (TipoVacaciones == "VACACIONES")
                            {
                                notificacion.GenerarNotificacionVacaciones(response3, "SOLICITUD VACACIONES", _connectionString);
                            }
                            else if (TipoVacaciones == "PERMISO")
                            {
                                notificacion.GenerarNotificacionPermiso(response3, "SOLICITUD PERMISO", _connectionString, aprobar.MotivoAnulacion);
                            }
                        }
                        #endregion
                        //Enviar Notificacion

                    }
                    return response;
                }
            }
        }

        public async Task<IEnumerable<PrmTipoSolicitud>> GetByMostrarPrmTipoSolicitud()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPrmTipoSolicitud", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<PrmTipoSolicitud>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPrmTipoSolicitud(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<PrmEstadoSolicitud>> GetByMostrarPrmEstadoSolicitud()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPrmEstadoSolicitud", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<PrmEstadoSolicitud>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPrmEstadoSolicitud(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Vacaciones>> GetByMostrarVacaciones(FiltroVacaciones filtro)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarVacaciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", filtro.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaDesde", filtro.FechaDesde));
                    cmd.Parameters.Add(new SqlParameter("@FechaHasta", filtro.FechaHasta));
                    cmd.Parameters.Add(new SqlParameter("@IdTipoSolicitud", filtro.IdTipoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@EstadoSolicitud", filtro.EstadoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@Filtro", filtro.Filtro));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", filtro.Tipo));
                    var response = new List<Vacaciones>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToVacaciones(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Vacaciones>> GetByMostrarVacacionesUnificado(FiltroVacaciones filtro)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarVacaciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdVacaciones", filtro.IdVacaciones));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", filtro.IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaDesde", filtro.FechaDesde));
                    cmd.Parameters.Add(new SqlParameter("@FechaHasta", filtro.FechaHasta));
                    cmd.Parameters.Add(new SqlParameter("@IdTipoSolicitud", filtro.IdTipoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@EstadoSolicitud", filtro.EstadoSolicitud));
                    cmd.Parameters.Add(new SqlParameter("@Filtro", filtro.Filtro));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", filtro.Tipo));
                    var response = new List<Vacaciones>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToVacaciones(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<SaldoVacaciones>> GetByMostrarDiasVacacionesGenerados(Int64 IdEmpleado,int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarDiasVacacionesGenerados", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<SaldoVacaciones>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToSaldoVacaciones(reader));
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

        private GenericaVP MapToGenericaVP(SqlDataReader reader)
        {
            return new GenericaVP()
            {
                valor1 = (Int16)reader["valor1"],
                valor2 = reader["valor2"].ToString(),
                IdVacaciones = (Int64)reader["IdVacaciones"],
            };
        }


        private PrmTipoSolicitud MapToPrmTipoSolicitud(SqlDataReader reader)
        {
            return new PrmTipoSolicitud()
            {
                IdTipoSolicitud = (Int64)reader["IdTipoSolicitud"],
                Descripcion = reader["Descripcion"].ToString(),
                Estado = (Int32)reader["Estado"],
            };
        }

        private PrmEstadoSolicitud MapToPrmEstadoSolicitud(SqlDataReader reader)
        {
            return new PrmEstadoSolicitud()
            {
                IdEstadoSolicitud = (Int64)reader["IdEstadoSolicitud"],
                Descripcion = reader["Descripcion"].ToString(),
                Estado = (Int32)reader["Estado"],
            };
        }

        private Vacaciones MapToVacaciones(SqlDataReader reader)
        {
            return new Vacaciones()
            {
                IdVacaciones = (Int64)reader["IdVacaciones"],
                IdTipoSolicitud = (Int64)reader["IdTipoSolicitud"],
                Descripcion = reader["Descripcion"].ToString(),
                IdEmpleado = (Int64)reader["IdEmpleado"],
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                Cedula = reader["Cedula"].ToString(),
                Colaborador = reader["Colaborador"].ToString(),
                Departamento = reader["Departamento"].ToString(),
                JefeInmediato = reader["JefeInmediato"].ToString(),
                Remplazo = reader["Remplazo"].ToString(),
                FechaDesde = (DateTime)reader["FechaDesde"],
                FechaHasta = (DateTime)reader["FechaHasta"],
                TotalDias = (decimal)reader["TotalDias"],
                Feriado = (decimal)reader["Feriado"],
                SaldoDias = (decimal)reader["SaldoDias"],
                CargoVacaciones = (Int32)reader["CargoVacaciones"],
                Horas = reader["Horas"].ToString(),
                Actividad = reader["Actividad"].ToString(),
                Observacion = reader["Observacion"].ToString(),
                EstadoSolicitud = reader["EstadoSolicitud"].ToString(),
                FechaAprobacion = (DateTime)reader["FechaAprobacion"],
                FechaRechazo = (DateTime)reader["FechaRechazo"],
                UsuarioAprobo = reader["UsuarioAprobo"].ToString(),
                UsuarioRechazo = reader["UsuarioRechazo"].ToString(),
                Estado = (Int32)reader["Estado"],
                Ruta_Archivo = reader["Ruta_Archivo"].ToString(),
                Descripcion_Archivo = reader["Descripcion_Archivo"].ToString(),
                MotivoAnulacion = reader["MotivoAnulacion"].ToString(),
                CorreoJefe = reader["CorreoJefe"].ToString(),
            };
        }

        private SaldoVacaciones MapToSaldoVacaciones(SqlDataReader reader)
        {
            return new SaldoVacaciones()
            {
                IdSaldoVacaciones = (Int64)reader["IdSaldoVacaciones"],
                IdEmpleado = (Int64)reader["IdEmpleado"],
                PeriodoInicio = (DateTime)reader["PeriodoInicio"],
                PeriodoFinal = (DateTime)reader["PeriodoFinal"],
                DiasGenerados = (decimal)reader["DiasGenerados"],
                DiasTomados = (decimal)reader["DiasTomados"],
                Saldo = (decimal)reader["Saldo"],
                Estado = (Int32)reader["Estado"],
            };
        }

    }
}
