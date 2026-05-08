using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion.AccesoDatos.Repository.CArchivo
{
   public class Notificacion
    {
        public string GenerarNotificacionVacaciones(IEnumerable<Vacaciones> vacaciones, string TipoDocumento,string _connectionString)
        {
            string Resultado = "";
            string EstadoSolicitud = "";
            string CorreoJefe = "";
            string HTML = "";
            string strFechaRegistro = "";
            string Cedula = "";
            string Colaborador = "";
            string Desde = "";
            string Hasta = "";
            string DiasSolicitados = "";
            string Cargo = "";
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
            foreach (Vacaciones generica1 in vacaciones)
            {
                EstadoSolicitud = generica1.EstadoSolicitud;
                CorreoJefe = generica1.CorreoJefe;
                strFechaRegistro = generica1.FechaRegistro.ToString("yyyy-MM-dd");
                Cedula = generica1.Cedula;
                Colaborador = generica1.Colaborador;
                Desde = generica1.FechaDesde.ToString("yyyy-MM-dd");
                Hasta = generica1.FechaHasta.ToString("yyyy-MM-dd");
                DiasSolicitados = generica1.TotalDias.ToString();
            }
            HTML = archivo.NombreArchivo;

            if (EstadoSolicitud == "APROBADO")
            {
                HTML = HTML.Replace("[NombresApelidos]", TipoDocumento + " APROBADA Y REGISTRADA");
                HTML = HTML.Replace("[Fecha]", strFechaRegistro);
                HTML = HTML.Replace("[Cedula]", Cedula);
                HTML = HTML.Replace("[Colaborador]", Colaborador);
                HTML = HTML.Replace("[Desde]", Desde);
                HTML = HTML.Replace("[Hasta]", Hasta);
                HTML = HTML.Replace("[Dias]", DiasSolicitados);
                HTML = HTML.Replace("[Notificacion]", "");
                EnviarNotificacionOportunidad(CorreoJefe, TipoDocumento + " APROBADA Y REGISTRADA", HTML, _connectionString);
            }
            else if (EstadoSolicitud == "POR APROBAR")
            {
                HTML = HTML.Replace("[NombresApelidos]", TipoDocumento);
                HTML = HTML.Replace("[Fecha]", strFechaRegistro);
                HTML = HTML.Replace("[Cedula]", Cedula);
                HTML = HTML.Replace("[Colaborador]", Colaborador);
                HTML = HTML.Replace("[Desde]", Desde);
                HTML = HTML.Replace("[Hasta]", Hasta);
                HTML = HTML.Replace("[Dias]", DiasSolicitados);
                HTML = HTML.Replace("[Notificacion]", "");
                EnviarNotificacionOportunidad(CorreoJefe, TipoDocumento, HTML, _connectionString);
            }
            else
            {
                HTML = HTML.Replace("[NombresApelidos]", TipoDocumento + " APROBADO Y REGISTRADO");
                HTML = HTML.Replace("[Fecha]", strFechaRegistro);
                HTML = HTML.Replace("[Cedula]", Cedula);
                HTML = HTML.Replace("[Colaborador]", Colaborador);
                HTML = HTML.Replace("[Desde]", Desde);
                HTML = HTML.Replace("[Hasta]", Hasta);
                HTML = HTML.Replace("[Dias]", DiasSolicitados);
                HTML = HTML.Replace("[Notificacion]", "");
                EnviarNotificacionOportunidad(CorreoJefe, TipoDocumento + " " + EstadoSolicitud, HTML, _connectionString);
            }

            return Resultado;
        }

        public string GenerarNotificacionPermiso(IEnumerable<Vacaciones> vacaciones, string TipoDocumento, string _connectionString,string Observacion)
        {
            string Resultado = "";
            string EstadoSolicitud = "";
            string CorreoJefe = "";
            string HTML = "";
            string strFechaRegistro = "";
            string Cedula = "";
            string Colaborador = "";
            string Desde = "";
            string Hasta = "";
            string TotalHoras = "";
            string Cargo = "";
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
            foreach (Vacaciones generica1 in vacaciones)
            {
                EstadoSolicitud = generica1.EstadoSolicitud;
                CorreoJefe = generica1.CorreoJefe;
                strFechaRegistro = generica1.FechaRegistro.ToString("yyyy-MM-dd");
                Cedula = generica1.Cedula;
                Colaborador = generica1.Colaborador;
                Desde = generica1.FechaDesde.ToString("HH:ss");
                Hasta = generica1.FechaHasta.ToString("HH:ss");
                TotalHoras = generica1.Horas;
                if (generica1.CargoVacaciones == 1)
                {
                    Cargo = "SI";
                }
                else
                {
                    Cargo = "NO";
                }
                Observacion = generica1.Observacion;
            }
            HTML = archivo.NombreArchivo;

            if (EstadoSolicitud == "APROBADO")
            {
                HTML = HTML.Replace("[NombresApelidos]", TipoDocumento +" APROBADA Y REGISTRADA");
                HTML = HTML.Replace("[Fecha]", strFechaRegistro);
                HTML = HTML.Replace("[Cedula]", Cedula);
                HTML = HTML.Replace("[Colaborador]", Colaborador);
                HTML = HTML.Replace("[Desde]", Desde);
                HTML = HTML.Replace("[Hasta]", Hasta);
                HTML = HTML.Replace("[TotalHoras]", TotalHoras);
                HTML = HTML.Replace("[Cargo]", Cargo);
                HTML = HTML.Replace("[Observacion]", Observacion);
                HTML = HTML.Replace("[Notificacion]", "");
                EnviarNotificacionOportunidad(CorreoJefe, TipoDocumento + " APROBADA Y REGISTRADA", HTML, _connectionString);
            }
            else if (EstadoSolicitud == "POR APROBAR")
            {
                HTML = HTML.Replace("[NombresApelidos]", TipoDocumento);
                HTML = HTML.Replace("[Fecha]", strFechaRegistro);
                HTML = HTML.Replace("[Cedula]", Cedula);
                HTML = HTML.Replace("[Colaborador]", Colaborador);
                HTML = HTML.Replace("[Desde]", Desde);
                HTML = HTML.Replace("[Hasta]", Hasta);
                HTML = HTML.Replace("[TotalHoras]", TotalHoras);
                HTML = HTML.Replace("[Cargo]", Cargo);
                HTML = HTML.Replace("[Observacion]", Observacion);
                HTML = HTML.Replace("[Notificacion]", "");
                EnviarNotificacionOportunidad(CorreoJefe, TipoDocumento, HTML, _connectionString);
            }
            else
            {
                HTML = HTML.Replace("[NombresApelidos]", TipoDocumento + " "+ EstadoSolicitud);
                HTML = HTML.Replace("[Fecha]", strFechaRegistro);
                HTML = HTML.Replace("[Cedula]", Cedula);
                HTML = HTML.Replace("[Colaborador]", Colaborador);
                HTML = HTML.Replace("[Desde]", Desde);
                HTML = HTML.Replace("[Hasta]", Hasta);
                HTML = HTML.Replace("[TotalHoras]", TotalHoras);
                HTML = HTML.Replace("[Cargo]", Cargo);
                HTML = HTML.Replace("[Observacion]", Observacion);
                HTML = HTML.Replace("[Notificacion]", "");
                EnviarNotificacionOportunidad(CorreoJefe, TipoDocumento + " " + EstadoSolicitud, HTML, _connectionString);
            }

            return Resultado;
        }

        private string EnviarNotificacionOportunidad(string Correo, string Titulo, string Contienido, string _connectionString)
        {
            string resultado = "";
            EntParametrosCorreo correo = new EntParametrosCorreo();
            DataTable table = new DataTable();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            string mensaje = "";
            table = cargar1.RetornarConfigSMTP(ref mensaje);
            if (table.Rows.Count > 0)
            {
                correo.smtpAddress = table.Rows[0]["HostSmtp"].ToString();
                correo.portNumber = Convert.ToInt32(table.Rows[0]["PuertoSmtp"].ToString());
                correo.emailFrom = table.Rows[0]["UsuarioSmtp"].ToString();
                correo.password = table.Rows[0]["ClaveSmtp"].ToString();
                correo.enableSSL = Convert.ToBoolean(table.Rows[0]["enableSSL"].ToString());

                bool temp = cargar1.EnviarCorreo(Correo.ToString(), Titulo.ToString(), Contienido.ToString(), correo);

            }

            return resultado;
        }

    }

}
