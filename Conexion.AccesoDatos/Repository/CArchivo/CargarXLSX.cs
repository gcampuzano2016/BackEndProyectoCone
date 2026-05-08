using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Globalization;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Conexion.AccesoDatos.Repository.CArchivo
{
  
    public class EntRespuesta
    {
        // Codigo del estado de la respuesta
        // 0 - Error, 1 - Exito
        public string estado { get; set; }
        // Contendra el resultado de la consulta, puede ser: arreglo para grid.
        public dynamic resultado { get; set; }
        // Mensaje que se muestra al usaurio, puede ser de exito o error dependiendo del estado
        public string mensaje { get; set; }
        // Tipo de mensaje que permitira indicar si el mensaje a mostrarse es confirmación, warning, alerta, informativo
        // valores permitidos: success, info, warning, danger 
        public string tipoMensaje { get; set; }
        // Determina el codigo de error en caso de producirse y existir para evitar mostrar mensajes de error técnicos
        public string codigoError { get; set; }
        // Contendra el resultado de la consulta cuando retorna un datatable.
        public DataTable resultadoTabla { get; set; }

        public DataSet resultadoSet { get; set; }
    }
    public class CargarXLSX
    {
        private readonly string _connectionString;
        public CargarXLSX(string configuration)
        {
            _connectionString = configuration;
        }

        #region conectar
        public SqlConnection conectar()
        {
            SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = "Data Source=GUILLERMO; Initial Catalog=SistemaConexion; User Id=sa; Password=Sql$erver2014";
            cn.ConnectionString = _connectionString;// "Data Source=conexiondb2022.database.windows.net; Initial Catalog=CONEXIONDB; User Id=Administrator2022; Password=U$uar10conexion";
            return cn;

        }
        #endregion

        #region SubirArchivoMedios
        public string SubirArchivoMedios(string rutaDocumento)
        {
            //Create a new DataTable.
            DataTable dt = new DataTable();
            EntRespuesta entRespuesta = new EntRespuesta();
            List<CargarRelacionMedios> ll = new List<CargarRelacionMedios>();
            using (XLWorkbook workBook = new XLWorkbook(rutaDocumento))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                //rowCount = rowCount - 1;
                int column = 1;
                int row = 2;
                while (row <= rowCount)
                {
                    List<string> llString = new List<string>();
                    while (column <= columnCount)
                    {
                        string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                        llString.Add(title);
                        column++;
                    }

                    row++;
                    column = 1;
                    if (llString[1] != "")
                    {
                        #region LLenarDatos
                        CargarRelacionMedios detalle = new CargarRelacionMedios();
                        detalle.Medios = llString[0];
                        detalle.Canal = llString[1];
                        detalle.Programa = llString[2];
                        detalle.Derecho = llString[3];
                        detalle.Formato = llString[4];
                        detalle.Unidad = llString[5];
                        detalle.Generico = llString[6];                   
                        ll.Add(detalle);
                        #endregion
                    }
                }
                var json = JsonConvert.SerializeObject(ll);
                dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                entRespuesta = SubirRelacionMedios(dt, 1,1);
            }
            return entRespuesta.mensaje;
        }
        #endregion

        #region SubirArchivoForeCast
        public string SubirArchivoForeCast(string rutaDocumento)
        {
            //Create a new DataTable.
            string  json="";
            DataTable dt = new DataTable();
            EntRespuesta entRespuesta = new EntRespuesta();
            List<CargaForeCast> ll = new List<CargaForeCast>();
            using (XLWorkbook workBook = new XLWorkbook(rutaDocumento))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                //rowCount = rowCount - 1;
                int column = 1;
                int row = 2;
                while (row <= rowCount)
                {
                    List<string> llString = new List<string>();
                    while (column <= columnCount)
                    {
                        string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                        llString.Add(title);
                        column++;
                    }

                    row++;
                    column = 1;
                    if (llString[1] != "")
                    {
                        #region LLenarDatos
                        CargaForeCast detalle = new CargaForeCast();
                        detalle.canal = llString[0];
                        detalle.programa = llString[1];
                        detalle.franja = llString[2];
                        detalle.derecho = llString[3];
                        detalle.formato = llString[4];
                        detalle.unidad = llString[5];
                        detalle.cantidad =Convert.ToDecimal(llString[6]);
                        detalle.precio = Convert.ToDecimal(llString[7]);
                        detalle.detalle = llString[8];
                        detalle.versiones = llString[9];
                        ll.Add(detalle);
                        #endregion
                    }
                }
                 json = JsonConvert.SerializeObject(ll);
            }
            return (string)json;
        }
        #endregion


        #region SubirArchivoMapaPautaValidar
        public string SubirArchivoMapaPautaValidar(string rutaDocumento)
        {
            //Create a new DataTable.
            string json = "";
            DataTable dt = new DataTable();
            EntRespuesta entRespuesta = new EntRespuesta();
            List<CargaForeCast> ll = new List<CargaForeCast>();
            using (XLWorkbook workBook = new XLWorkbook(rutaDocumento))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                //rowCount = rowCount - 1;
                int column = 1;
                int row = 16;
                while (row <= rowCount)
                {
                    List<string> llString = new List<string>();
                    while (column <= columnCount)
                    {
                        string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                        llString.Add(title);
                        column++;
                    }

                    row++;
                    column = 1;
                    if (llString[1] != "" && llString[1] != "VALOR TOTAL")
                    {
                        #region LLenarDatos
                        CargaForeCast detalle = new CargaForeCast();
                        detalle.canal = llString[1];
                        detalle.programa = llString[2];
                        detalle.franja = llString[7];
                        detalle.derecho = llString[5];
                        detalle.formato = llString[6];
                        detalle.unidad = "";
                        detalle.cantidad = 0;
                        detalle.precio = 0;
                        detalle.detalle = "";
                        ll.Add(detalle);
                        #endregion
                    }
                }
                json = JsonConvert.SerializeObject(ll);
            }
            return (string)json;
        }
        #endregion

        #region SubirArchivoDetalleContrato
        public string SubirArchivoDetalleContrato(string rutaDocumento)
        {
            //Create a new DataTable.
            string json = "";
            DataTable dt = new DataTable();
            EntRespuesta entRespuesta = new EntRespuesta();
            List<CargaForeCast> ll = new List<CargaForeCast>();
            using (XLWorkbook workBook = new XLWorkbook(rutaDocumento))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                //rowCount = rowCount - 1;
                int column = 1;
                int row = 2;
                while (row <= rowCount)
                {
                    List<string> llString = new List<string>();
                    while (column <= columnCount)
                    {
                        string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                        llString.Add(title);
                        column++;
                    }

                    row++;
                    column = 1;
                    if (llString[1] != "")
                    {
                        #region LLenarDatos
                        CargaForeCast detalle = new CargaForeCast();
                        detalle.canal = llString[0];
                        detalle.programa = llString[1];
                        detalle.franja = llString[2];
                        detalle.derecho = llString[3];
                        detalle.formato = llString[4];
                        detalle.unidad = llString[5];
                        detalle.cantidad = Convert.ToDecimal(llString[6]);
                        detalle.precio = Convert.ToDecimal(llString[7]);
                        detalle.detalle = llString[8];
                        detalle.versiones = llString[9];
                        ll.Add(detalle);
                        #endregion
                    }
                }
                json = JsonConvert.SerializeObject(ll);
            }
            return (string)json;
        }
        #endregion


        #region SubirArchivo
        public string SubirArchivo(CargarArchivo cargar)
        {
            //Create a new DataTable.
            DataTable dt = new DataTable();
            EntRespuesta entRespuesta = new EntRespuesta();

            //Open the Excel file using ClosedXML.

            List<CargarArchivo> cargarArchivos = new List<CargarArchivo>();
          
            if (cargar.Tipo == 1)
            {
                cargarArchivos = MostrarCargaArhivo(cargar.Tipo,cargar.IdContrato);
                List<DetalleContratoExcel> ll = new List<DetalleContratoExcel>();
                foreach (CargarArchivo data in cargarArchivos)
                {
                    using (XLWorkbook workBook = new XLWorkbook(data.RutaArchivo))
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                        var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                        rowCount = rowCount - 8;
                        int column = 2;
                        int row = 16;


                        while (row <= rowCount)
                        {
                            List<string> llString = new List<string>();
                            while (column <= columnCount)
                            {
                                string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                                llString.Add(title);
                                column++;
                            }

                            row++;
                            column = 2;
                            if (llString[1] != "")
                            {
                                #region LLenarDatos
                                DetalleContratoExcel detalle = new DetalleContratoExcel();
                                detalle.Canal = llString[0];
                                detalle.Programa = llString[1];
                                detalle.Detalle = llString[2];
                                detalle.Versiones = llString[3];
                                detalle.Derecho = llString[4];
                                detalle.Duracion = llString[5];
                                detalle.Franja = llString[6];
                                detalle.Tarifa = llString[7];

                                detalle.Valor1 = llString[8];
                                detalle.Valor2 = llString[9];
                                detalle.Valor3 = llString[10];
                                detalle.Valor4 = llString[11];

                                detalle.TotalSegundo = llString[13];
                                detalle.ValorNegocio = llString[14];

                                detalle.data_1 = llString[15];
                                detalle.data_2 = llString[16];
                                detalle.data_3 = llString[17];
                                detalle.data_4 = llString[18];
                                detalle.data_5 = llString[19];
                                detalle.data_6 = llString[20];
                                detalle.data_7 = llString[21];
                                detalle.data_8 = llString[22];
                                detalle.data_9 = llString[23];
                                detalle.data_10 = llString[24];

                                detalle.data_11 = llString[25];
                                detalle.data_12 = llString[26];
                                detalle.data_13 = llString[27];
                                detalle.data_14 = llString[28];
                                detalle.data_15 = llString[29];
                                detalle.data_16 = llString[30];
                                detalle.data_17 = llString[31];
                                detalle.data_18 = llString[32];
                                detalle.data_19 = llString[33];
                                detalle.data_20 = llString[34];

                                detalle.data_21 = llString[35];
                                detalle.data_22 = llString[36];
                                detalle.data_23 = llString[37];
                                detalle.data_24 = llString[38];
                                detalle.data_25 = llString[39];
                                detalle.data_26 = llString[40];
                                detalle.data_27 = llString[41];
                                detalle.data_28 = llString[42];
                                detalle.data_29 = llString[43];
                                detalle.data_30 = llString[44];
                                detalle.data_31 = llString[45];
                                detalle.Impacto = llString[46];
                                ll.Add(detalle);
                                #endregion
                            }
                        }

                    }
                    var json = JsonConvert.SerializeObject(ll);
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                    entRespuesta = ActualizarDetalleContrato(1, data.IdContrato, dt, json, 1,cargar.perfil);
                }
            }
            else if(cargar.Tipo == 2)
            {
                cargarArchivos = MostrarCargaArhivo(cargar.Tipo, cargar.IdForeCast);
                List<DetalleForeCastCarga> ll = new List<DetalleForeCastCarga>();
                foreach (CargarArchivo data in cargarArchivos)
                {
                    using (XLWorkbook workBook = new XLWorkbook(data.RutaArchivo))
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                        var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                        rowCount = rowCount - 8;
                        int column = 2;
                        int row = 16;


                        while (row <= rowCount)
                        {
                            List<string> llString = new List<string>();
                            while (column <= columnCount)
                            {
                                string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                                llString.Add(title);
                                column++;
                            }

                            row++;
                            column = 2;
                            if (llString[0] != "")
                            {
                                #region LLenarDatos
                                DetalleForeCastCarga detalle = new DetalleForeCastCarga();
                                detalle.Canal = llString[0];
                                detalle.Programa = llString[1];
                                detalle.Franja = llString[2];
                                detalle.Derecho = llString[3];
                                detalle.Formato = llString[4];
                                detalle.Unidad = llString[5];
                                detalle.Cantidad = Convert.ToDecimal( llString[6]);
                                detalle.Precio = Convert.ToDecimal(llString[7]);                             
                                ll.Add(detalle);
                                #endregion
                            }
                        }

                    }
                    var json = JsonConvert.SerializeObject(ll);
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                    entRespuesta = InsertarModificarEliminarDetalleForecastCarga( data.IdForeCast, dt, 1);
                }
            }
            return entRespuesta.mensaje;
        }
        #endregion

        #region SubirArchivoContrato
        public EntRespuesta SubirArchivoContrato(CargarArchivo cargar)
        {
            //Create a new DataTable.
            DataTable dt = new DataTable();
            EntRespuesta entRespuesta = new EntRespuesta();

            //Open the Excel file using ClosedXML.

            List<CargarArchivo> cargarArchivos = new List<CargarArchivo>();

            if (cargar.Tipo == 1)
            {
                cargarArchivos = MostrarCargaArhivo(cargar.Tipo, cargar.IdContrato);
                List<DetalleContratoExcel> ll = new List<DetalleContratoExcel>();
                foreach (CargarArchivo data in cargarArchivos)
                {
                    using (XLWorkbook workBook = new XLWorkbook(data.RutaArchivo))
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                        var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                        rowCount = rowCount - 8;
                        int column = 2;
                        int row = 16;


                        while (row <= rowCount)
                        {
                            List<string> llString = new List<string>();
                            while (column <= columnCount)
                            {
                                string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                                llString.Add(title);
                                column++;
                            }

                            row++;
                            column = 2;
                            if (llString[1] != "")
                            {
                                #region LLenarDatos
                                DetalleContratoExcel detalle = new DetalleContratoExcel();
                                detalle.Canal = llString[0];
                                detalle.Programa = llString[1];
                                detalle.Detalle = llString[2];
                                detalle.Versiones = llString[3];
                                detalle.Derecho = llString[4];
                                detalle.Duracion = llString[5];
                                detalle.Franja = llString[6];
                                detalle.Tarifa = llString[7];

                                detalle.Valor1 = llString[8];
                                detalle.Valor2 = llString[9];
                                detalle.Valor3 = llString[10];
                                detalle.Valor4 = llString[11];

                                detalle.TotalSegundo = llString[13];
                                detalle.ValorNegocio = llString[14];

                                detalle.data_1 = llString[15];
                                detalle.data_2 = llString[16];
                                detalle.data_3 = llString[17];
                                detalle.data_4 = llString[18];
                                detalle.data_5 = llString[19];
                                detalle.data_6 = llString[20];
                                detalle.data_7 = llString[21];
                                detalle.data_8 = llString[22];
                                detalle.data_9 = llString[23];
                                detalle.data_10 = llString[24];

                                detalle.data_11 = llString[25];
                                detalle.data_12 = llString[26];
                                detalle.data_13 = llString[27];
                                detalle.data_14 = llString[28];
                                detalle.data_15 = llString[29];
                                detalle.data_16 = llString[30];
                                detalle.data_17 = llString[31];
                                detalle.data_18 = llString[32];
                                detalle.data_19 = llString[33];
                                detalle.data_20 = llString[34];

                                detalle.data_21 = llString[35];
                                detalle.data_22 = llString[36];
                                detalle.data_23 = llString[37];
                                detalle.data_24 = llString[38];
                                detalle.data_25 = llString[39];
                                detalle.data_26 = llString[40];
                                detalle.data_27 = llString[41];
                                detalle.data_28 = llString[42];
                                detalle.data_29 = llString[43];
                                detalle.data_30 = llString[44];
                                detalle.data_31 = llString[45];
                                detalle.Impacto = llString[46];
                                ll.Add(detalle);
                                #endregion
                            }
                        }

                    }
                    var json = JsonConvert.SerializeObject(ll);
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                    entRespuesta = ActualizarDetalleContrato(1, data.IdContrato, dt, json, 1,cargar.perfil);
                }
            }
            else if (cargar.Tipo == 2)
            {
                cargarArchivos = MostrarCargaArhivo(cargar.Tipo, cargar.IdForeCast);
                List<DetalleForeCastCarga> ll = new List<DetalleForeCastCarga>();
                foreach (CargarArchivo data in cargarArchivos)
                {
                    using (XLWorkbook workBook = new XLWorkbook(data.RutaArchivo))
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        var rowCount = workBook.Worksheet(1).LastRowUsed().RowNumber();
                        var columnCount = workBook.Worksheet(1).LastColumnUsed().ColumnNumber();
                        rowCount = rowCount - 8;
                        int column = 2;
                        int row = 16;


                        while (row <= rowCount)
                        {
                            List<string> llString = new List<string>();
                            while (column <= columnCount)
                            {
                                string title = workBook.Worksheets.Worksheet(1).Cell(row, column).GetString();
                                llString.Add(title);
                                column++;
                            }

                            row++;
                            column = 2;
                            if (llString[0] != "")
                            {
                                #region LLenarDatos
                                DetalleForeCastCarga detalle = new DetalleForeCastCarga();
                                detalle.Canal = llString[0];
                                detalle.Programa = llString[1];
                                detalle.Franja = llString[2];
                                detalle.Derecho = llString[3];
                                detalle.Formato = llString[4];
                                detalle.Unidad = llString[5];
                                detalle.Cantidad = Convert.ToDecimal(llString[6]);
                                detalle.Precio = Convert.ToDecimal(llString[7]);
                                ll.Add(detalle);
                                #endregion
                            }
                        }

                    }
                    var json = JsonConvert.SerializeObject(ll);
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                    entRespuesta = InsertarModificarEliminarDetalleForecastCarga(data.IdForeCast, dt, 1);
                }
            }
            return entRespuesta;
        }
        #endregion

        #region GenerarlRolDePagos

        #endregion

        #region ActualizarDetalleContrato
        public EntRespuesta ActualizarDetalleContrato(Int64 IdForeCast, Int64 IdContrato, DataTable detalleContrato,string Json, int Tipo,string Perfil)
        {
            EntRespuesta Respuesta = new EntRespuesta();
            DataTable dtResultados = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                
                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("InsertarModificarEliminarDetalleContrato", cnx);
                cmd.Parameters.AddWithValue("@IdForeCast", IdForeCast);
                cmd.Parameters.AddWithValue("@IdContrato", IdContrato);
                cmd.Parameters.AddWithValue("@SLQDetalleContrato", detalleContrato);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.Parameters.AddWithValue("@Perfil", Perfil);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                dtResultados.Load(dr);
                Respuesta.estado = "1";
                if (dtResultados.Rows.Count > 0)
                {
                    Respuesta.mensaje = dtResultados.Rows[0][0].ToString();
                    Respuesta.estado = dtResultados.Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                Respuesta.estado = "0";
                Respuesta.mensaje = ex.Message.ToString();
                Respuesta.tipoMensaje = "danger";
            }
            finally
            {
                cmd.Connection.Close();
            }
            return Respuesta;
        }
        #endregion

        #region SubirRelacionMedios
        public EntRespuesta SubirRelacionMedios( DataTable SLQRelacionMedio,int Estado, int Tipo)
        {
            EntRespuesta Respuesta = new EntRespuesta();
            DataTable dtResultados = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {

                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("InsertarModificarEliminarRelacionMedio", cnx);
                cmd.Parameters.AddWithValue("@SLQRelacionMedio", SLQRelacionMedio);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                dtResultados.Load(dr);
                Respuesta.estado = "1";
                if (dtResultados.Rows.Count > 0)
                {
                    Respuesta.mensaje = dtResultados.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                Respuesta.estado = "0";
                Respuesta.mensaje = ex.Message.ToString();
                Respuesta.tipoMensaje = "danger";
            }
            finally
            {
                cmd.Connection.Close();
            }
            return Respuesta;
        }
        #endregion

        #region RetornarConfigSMTP
        public DataTable RetornarConfigSMTP(ref string mensaje)
        {
            DataTable dtResultados = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("mostrarempresas", cnx);
                cmd.Parameters.AddWithValue("@RUC", "");
                cmd.Parameters.AddWithValue("@Tipo", "");
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                dtResultados.Load(dr);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message.ToString();
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dtResultados;
        }
        #endregion

        #region NotificacionOportunidad
        public DataTable NotificacionOportunidad(Int64 IdForeCast,int tipo, ref string mensaje)
        {
            DataTable dtResultados = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("NotificacionOportunidad", cnx);
                cmd.Parameters.AddWithValue("@IdForeCast", IdForeCast);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                dtResultados.Load(dr);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message.ToString();
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dtResultados;
        }
        #endregion

        #region InsertarModificarEliminarDetalleForecastCarga
        public EntRespuesta InsertarModificarEliminarDetalleForecastCarga(Int64 IdForeCast,  DataTable detalleForeCast, int Tipo)
        {
            EntRespuesta Respuesta = new EntRespuesta();
            DataTable dtResultados = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {

                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("InsertarModificarEliminarDetalleForecastCarga", cnx);
                cmd.Parameters.AddWithValue("@IdForeCast", IdForeCast);
                cmd.Parameters.AddWithValue("@SLQDetalleForeCast", detalleForeCast);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                dtResultados.Load(dr);
                Respuesta.estado = "1";
                if (dtResultados.Rows.Count > 0)
                {
                    Respuesta.mensaje = dtResultados.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                Respuesta.estado = "0";
                Respuesta.mensaje = ex.Message.ToString();
                Respuesta.tipoMensaje = "danger";
            }
            finally
            {
                cmd.Connection.Close();
            }
            return Respuesta;
        }
        #endregion

        #region MostrarCargaArhivo
        public List<CargarArchivo> MostrarCargaArhivo(int Tipo,Int64 IdProceso)
        {
            List<CargarArchivo> cargarArchivos = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("MostrarCargaArhivo", cnx);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.Parameters.AddWithValue("@IdProceso", IdProceso);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                cargarArchivos = new List<CargarArchivo>();
                while (dr.Read())
                {
                    CargarArchivo carga = new CargarArchivo();
                    carga.IdContrato =Convert.ToInt64( dr["IdContrato"].ToString());
                    carga.IdForeCast = Convert.ToInt64(dr["IdForeCast"].ToString());
                    carga.RutaArchivo = dr["RutaArchivo"].ToString();
                    cargarArchivos.Add(carga);
                }
            }
            catch (Exception)
            {
                cargarArchivos = null;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return cargarArchivos;
        }
        #endregion

        #region MostrarCargaArhivoConfig
        public PrmConfiguracionArchivo MostrarCargaArhivoConfig(Int64 IdContrato,Int64 IdForeCast,string TipoDocumento, Int32 TipoProceso)
        {
            PrmConfiguracionArchivo cargarArchivos = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("MostrarCargaArhivoConfig", cnx);
                cmd.Parameters.AddWithValue("@IdContrato", IdContrato);
                cmd.Parameters.AddWithValue("@IdForeCast", IdForeCast);
                cmd.Parameters.AddWithValue("@TipoDocumento", TipoDocumento);
                cmd.Parameters.AddWithValue("@TipoProceso", TipoProceso);
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();
                cargarArchivos = new PrmConfiguracionArchivo();
                while (dr.Read())
                {
                    cargarArchivos.RutaArchivo = dr["RutaArchivo"].ToString();
                    cargarArchivos.NombreArchivo = dr["NombreArchivo"].ToString();
                    cargarArchivos.Extencion = dr["Extencion"].ToString();
                    cargarArchivos.NombreArchivoSalida = dr["NombreArchivoSalida"].ToString();
                }
            }
            catch (Exception)
            {
                cargarArchivos = null;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return cargarArchivos;
        }
        #endregion

        #region EnviarCorreo
        public bool EnviarCorreo(string correosDestinatarios, string correoTitulo, string correoContenido, EntParametrosCorreo parametrosServidorCorreo)
        {
            bool Temp = false;
            string ErrorProceso = "";

            string smtpAddress = parametrosServidorCorreo.smtpAddress;
            string emailFrom = parametrosServidorCorreo.emailFrom;
            string password = parametrosServidorCorreo.password;
            string subject = correoTitulo;
            string body = correoContenido;
            int portNumber = parametrosServidorCorreo.portNumber;
            bool enableSSL = parametrosServidorCorreo.enableSSL;
            string emailFromName = parametrosServidorCorreo.emailFromName;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom, emailFromName);

                foreach (string correoIndividual in correosDestinatarios.Split(new Char[] { ';' }))
                {
                    if (correoIndividual != "")
                    {
                        mail.To.Add(correoIndividual);
                    }
                }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    try
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                        Temp = true;
                    }
                    catch (Exception ex)
                    {
                        Temp = false;
                        ErrorProceso = ex.Message.ToString().Trim();
                    }
                }
            }


            return Temp;
        }
        #endregion

        #region ConsultarATS
        public EntRespuesta ConsultarATS(DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            EntRespuesta respuesta = new EntRespuesta();
            DataSet dtResultados = new DataSet();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cnx = conectar();
                cnx.Open();
                cmd = new SqlCommand("Consulta_ats", cnx);
                SqlDataAdapter Adaptador = new SqlDataAdapter();
                cmd.Parameters.AddWithValue("@FechaInicio", FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFinal", FechaFinal);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.CommandType = CommandType.StoredProcedure;
                Adaptador = new SqlDataAdapter(cmd);
                Adaptador.Fill(dtResultados);
                respuesta.resultadoSet = dtResultados;
            }
            catch (Exception)
            {

            }
            finally
            {
                cmd.Connection.Close();
            }
            return respuesta;
        }
        #endregion

    }
}
