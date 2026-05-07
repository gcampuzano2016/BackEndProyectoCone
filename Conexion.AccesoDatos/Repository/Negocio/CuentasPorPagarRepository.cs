using Conexion.AccesoDatos.Repository.CArchivo;
using Conexion.AccesoDatos.Repository.CArchivo;
using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Conexion.AccesoDatos.Repository.Negocio
{
    public  class CuentasPorPagarRepository
    {
        private readonly string _connectionString;

        public CuentasPorPagarRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }
        public async Task<IEnumerable<Generica>> Insert(CuentasPorPagar porPagar)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarCuentaPorPagar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCuentaPorPagar", porPagar.IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@FechaAutorizacion", porPagar.FechaAutorizacion));
                    cmd.Parameters.Add(new SqlParameter("@FechaEmision", porPagar.FechaRegistro));
                    cmd.Parameters.Add(new SqlParameter("@EstadoServicio", porPagar.EstadoServicio));
                    cmd.Parameters.Add(new SqlParameter("@RuCedula", porPagar.RuCedula));
                    cmd.Parameters.Add(new SqlParameter("@RazonSocial", porPagar.RazonSocial));
                    cmd.Parameters.Add(new SqlParameter("@Email", porPagar.Email));
                    cmd.Parameters.Add(new SqlParameter("@AutorizacionSri", porPagar.AutorizacionSri));
                    cmd.Parameters.Add(new SqlParameter("@NumDocumento", porPagar.NumDocumento));
                    cmd.Parameters.Add(new SqlParameter("@PlazoVencimiento", porPagar.PlazoVencimiento));
                    cmd.Parameters.Add(new SqlParameter("@CompraTarifa0", porPagar.CompraTarifa0));
                    cmd.Parameters.Add(new SqlParameter("@CompraTarifa12", porPagar.CompraTarifa12));
                    cmd.Parameters.Add(new SqlParameter("@Iva", porPagar.Iva));
                    cmd.Parameters.Add(new SqlParameter("@ValorTotal", porPagar.ValorTotal));
                    cmd.Parameters.Add(new SqlParameter("@TipoDocumento", porPagar.TipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("@jsonContable", porPagar.jsonContable));
                    cmd.Parameters.Add(new SqlParameter("@Estado", porPagar.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", porPagar.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    var response = new List<Generica>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToGenerica(reader));
                        }


                        return response;
                    }
                }
            }
        }
        public async Task<IEnumerable<CuentasPorPagar>> GetByMostrarDocumentoSRI(string AutorizacionSri)
        {
            var response = new List<CuentasPorPagar>();
            CuentasPorPagar pagar = new CuentasPorPagar();
            pagar = AutorizacionSriDoc(AutorizacionSri);
            response.Add(pagar);
            return response;
        }

        public CuentasPorPagar AutorizacionSriDoc(string AutorizacionSri)
        {
            CuentasPorPagar pagar = new CuentasPorPagar();
            string TipoDocumento = AutorizacionSri.Substring(8, 2);
            string respuesta = "";

            if (TipoDocumento=="07")
            {
                respuesta = AutorizacionArchivos(AutorizacionSri, "https://cel.sri.gob.ec/comprobantes-electronicos-ws/AutorizacionComprobantesOffline");

            }
            else if (TipoDocumento == "01")
            {
                 respuesta = AutorizacionArchivos(AutorizacionSri, "https://cel.sri.gob.ec/comprobantes-electronicos-ws/AutorizacionComprobantesOffline");
                if(respuesta == "")
                {
                    pagar.Mensaje = "No hay respuesta del SRI ingresar el documento manualmente.";
                }
                else
                {
                    DataSet data = new DataSet();
                    data = GenerarXml(respuesta);
                    decimal SubTotal0 = 0;
                    decimal Subtotal12 = 0;
                    decimal Iva = 0;
                    decimal BaseImponible = 0;
                    if (data.Tables .Count > 0)
                    {
                        if(data.Tables.Contains("infoTributaria"))
                        {
                            if (data.Tables["infoTributaria"].Rows.Count > 0)
                            {
                                pagar.AutorizacionSri = data.Tables["infoTributaria"].Rows[0]["claveAcceso"].ToString();
                                pagar.NumDocumento = data.Tables["infoTributaria"].Rows[0]["estab"].ToString() + "-" + data.Tables["infoTributaria"].Rows[0]["ptoEmi"].ToString() + "-" + data.Tables["infoTributaria"].Rows[0]["secuencial"].ToString();
                            }
                        }

                        if (data.Tables.Contains("infoFactura"))
                        {
                            if (data.Tables["infoFactura"].Rows.Count > 0)
                            {
                                pagar.FechaAutorizacion = Convert.ToDateTime(data.Tables["infoFactura"].Rows[0]["fechaEmision"].ToString());
                                pagar.FechaRegistro = Convert.ToDateTime( data.Tables["infoFactura"].Rows[0]["fechaEmision"].ToString());
                                pagar.ValorTotal = Convert.ToDecimal(data.Tables["infoFactura"].Rows[0]["importeTotal"].ToString());
                                pagar.RuCedula = Convert.ToString(data.Tables["infoFactura"].Rows[0]["identificacionComprador"].ToString());
                                pagar.RazonSocial = Convert.ToString(data.Tables["infoFactura"].Rows[0]["razonSocialComprador"].ToString());
                            }
                        }

                        if (data.Tables.Contains("pago"))
                        {
                            if (data.Tables["pago"].Rows.Count > 0)
                            {
                                if(data.Tables["pago"].Columns.Contains("plazo"))
                                {
                                    pagar.PlazoVencimiento =Convert.ToInt32 ( data.Tables["pago"].Rows[0]["plazo"].ToString());
                                }
                                else
                                {
                                    pagar.PlazoVencimiento = 0;
                                }
                            }
                        }


                        if (data.Tables.Contains("totalImpuesto"))
                        {
                            if (data.Tables["totalImpuesto"].Rows.Count > 0)
                            {
                                foreach( DataRow dato in data.Tables["totalImpuesto"].Rows)
                                {
                                    BaseImponible = BaseImponible + Convert.ToDecimal(dato["baseImponible"].ToString());
                                    Iva = Iva + Convert.ToDecimal(dato["valor"].ToString());
                                    if(dato["codigoPorcentaje"].ToString() == "2")
                                    {
                                        Subtotal12 = Subtotal12 + Convert.ToDecimal(dato["baseImponible"].ToString());
                                    }
                                    else if (dato["codigoPorcentaje"].ToString() == "0")
                                    {
                                        SubTotal0 = SubTotal0 + Convert.ToDecimal(dato["baseImponible"].ToString());
                                    }
                                }
                                pagar.Iva = Iva;
                                pagar.CompraTarifa0 = SubTotal0;
                                pagar.CompraTarifa12 = Subtotal12;
                            }
                        }
                    }
                }
              
            }

            return pagar;
        }

        #region object ->AutorizacionArchivos
        public string AutorizacionArchivos(string ClaveAcceso, string url)
        {
            HttpWebRequest request = CreateWebRequest2(url, "autorizacionComprobante");
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ec=""http://ec.gob.sri.ws.autorizacion"">
                <soapenv:Header/>
                <soapenv:Body>
                    <ec:autorizacionComprobante>
                        <!--Optional:-->
                        <claveAccesoComprobante>" + ClaveAcceso.Trim() + @"</claveAccesoComprobante>" +
                "</ec:autorizacionComprobante>" +
            "</soapenv:Body>" +
            "</soapenv:Envelope>");
            string soapResult = "";
            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                }
            }
            catch
            {
                soapResult = "";
            }
            return soapResult;
        }
        public HttpWebRequest CreateWebRequest2(string url, string soapAction)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.ContentType = "text/xml;action=\"" + soapAction + "";
            webRequest.ContentType = "text/xml; charset=utf-8;action=\"" + soapAction + "";
            webRequest.Method = "POST";
            return webRequest;
        }

        #region GenerarXml
        public DataSet GenerarXml(string cadenaXml)
        {
            DataSet str = new DataSet();
            DataSet str2 = new DataSet();
            str.ReadXml(new XmlTextReader(new StringReader(cadenaXml)));

            if (str.Tables.Contains("autorizacion"))
            {
                string s = str.Tables["autorizacion"].Rows[0]["comprobante"].ToString();

                if (s.Contains("<comprobante>") & s.Contains("</comprobante>"))
                {
                    int startIndex = s.IndexOf("<comprobante>") + 13;
                    s = s.Substring(startIndex);
                    int index = s.IndexOf("</comprobante>");
                    s = s.Substring(0, index).Trim();
                }
                if (s.Contains("<ds:Signature") & s.Contains("</ds:Signature>"))
                {
                    int num4 = s.IndexOf("<ds:Signature");
                    int count = (s.IndexOf("</ds:Signature>") - num4) + 15;
                    s = s.Remove(num4, count).Trim();
                }

                str2.ReadXml(new XmlTextReader(new StringReader(s)));
            }

            return str2;
        }
        #endregion

        private Generica MapToGenerica(SqlDataReader reader)
        {
            return new Generica()
            {
                valor1 = (Int16)reader["valor1"],
                valor2 = reader["valor2"].ToString()
            };
        }


        #endregion

        public async Task<IEnumerable<CuentasPorPagar>> GetByMostrarFacturaPorPagarFecha(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaPorPagarFecha", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<CuentasPorPagar>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if(Tipo == 0)
                            {
                                response.Add(MapToFacturaPorPagarFecha(reader));
                            }
                            else if (Tipo == 1)
                            {
                                response.Add(MapToFacturaPorPagarFecha2(reader));
                            }
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Pagos>> GetByMostrarPagos(Int64 IdCuentaPorPagar, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPagos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCuentaPorPagar", IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Pagos>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPagos(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<PlantillaCuentas>> GetByMostrarPlantillaCuentaPorPagar(Int64 IdCuentaPorPagar,string Proveedor, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPlantillaCuentaPorPagar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCuentaPorPagar", IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@Proveedor", Proveedor));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PlantillaCuentas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (Tipo == 4)
                            {
                                response.Add(MapToPlantillaCuentasDetalle(reader));
                            }
                            else
                            {
                                response.Add(MapToPlantillaCuentas(reader));
                            }

                        }
                    }

                    return response;
                }
            }
        }


        public async Task<IEnumerable<PlantillaCuentas>> GetByBusquedaCuentasPorPagar(Int64 IdCuentaPorPagar, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BusquedaCuentasPorPagar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProceso", IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PlantillaCuentas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                                response.Add(MapToBusquedaCuenta(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<PlantillaCuentas>> GetByDescripcionCuentasPorPagar(Int64 IdCuentaPorPagar, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BusquedaCuentasPorPagar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProceso", IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PlantillaCuentas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDescripcionCuenta(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<Generica>> CargaArchivoGeneral(CargarArchivoBase64 cargar)
        {

            var response = new List<Generica>();
            string path = "";
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, cargar.TipoDocumento);
            path = archivo.RutaArchivo + cargar.NombreArchivo;
            byte[] stringBytes = Convert.FromBase64String(cargar.ArchivoBase64);//Encoding.ASCII.GetBytes(cargar.ArchivoBase64);
            System.IO.File.WriteAllBytes(path, stringBytes);
            Generica generica = new Generica();
            generica.valor1 = 1;
            generica.valor2 = "DOCUMENTO CARGADO..";
            response.Add(generica);

            return response;
        }

        private CuentasPorPagar MapToFacturaPorPagarFecha(SqlDataReader reader)
        {
            return new CuentasPorPagar()
            {

                IdCuentaPorPagar = (Int64)reader["IdCuentaPorPagar"],
                FechaAutorizacion = (DateTime)reader["FechaAutorizacion"],
                FechaRegistro = (DateTime)reader["FechaEmision"],
                EstadoServicio = reader["EstadoServicio"].ToString(),
                RazonSocial = reader["RazonSocial"].ToString(),
                Email = reader["Email"].ToString(),
                AutorizacionSri = reader["AutorizacionSri"].ToString(),
                NumDocumento = reader["NumDocumento"].ToString(),
                PlazoVencimiento = (Int32)reader["PlazoVencimiento"],
                CompraTarifa0 = (decimal)reader["CompraTarifa0"],
                CompraTarifa12 = (decimal)reader["CompraTarifa12"],
                Iva = (decimal)reader["Iva"],
                ValorTotal = (decimal)reader["ValorTotal"],
                EstadoPago = reader["EstadoPago"].ToString(),
                PorRegistrar = (Int32)reader["PorRegistrar"],
                Saldo = (decimal)reader["Saldo"],
            };
        }

        private CuentasPorPagar MapToFacturaPorPagarFecha2(SqlDataReader reader)
        {
            return new CuentasPorPagar()
            {
                Id = (Int64)reader["Id"],
                IdCuentaPorPagar = (Int64)reader["IdCuentaPorPagar"],
                FechaAutorizacion = (DateTime)reader["FechaAutorizacion"],
                FechaRegistro = (DateTime)reader["FechaEmision"],
                EstadoServicio = reader["EstadoServicio"].ToString(),
                RazonSocial = reader["RazonSocial"].ToString(),
                Email = reader["Email"].ToString(),
                AutorizacionSri = reader["AutorizacionSri"].ToString(),
                NumDocumento = reader["NumDocumento"].ToString(),
                PlazoVencimiento = (Int32)reader["PlazoVencimiento"],
                CompraTarifa0 = (decimal)reader["CompraTarifa0"],
                CompraTarifa12 = (decimal)reader["CompraTarifa12"],
                Iva = (decimal)reader["Iva"],
                ValorTotal = (decimal)reader["ValorTotal"],
                EstadoPago = reader["EstadoPago"].ToString(),
                RuCedula = reader["RuCedula"].ToString(),
                TipoDocumento = reader["TipoDocumento"].ToString(),
                PorRegistrar = (Int32)reader["PorRegistrar"],
                Saldo = (decimal)reader["Saldo"],
                IdProveedor = (Int64)reader["IdProveedor"],
                RutaDocumento = reader["RutaDocumento"].ToString(),
                stringArchivo64 = DevolverArchivoBase64(reader["RutaDocumento"].ToString()),
            };
        }
        public async Task<IEnumerable<Generica>> ModificarAsientoContable(Int64 IdCuentaPorPagar, string jsonFinal, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ModificarAsientoContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCuentaPorPagar", IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public async Task<IEnumerable<Generica>> ModificarAsientoContableDescripcion(Int64 IdCuentaPorPagar, string jsonFinal,DateTime FechaPago, string Concepto, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ModificarAsientoContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCuentaPorPagar", IdCuentaPorPagar));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@FechaPago", FechaPago));
                    cmd.Parameters.Add(new SqlParameter("@Concepto", Concepto));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public async Task<IEnumerable<Generica>> InsertarAsientoContable(string TipoTransaccion,string Descripcion, string jsonFinal,decimal ValorProceso, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarAsientoContable", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TipoTransaccion", TipoTransaccion));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    cmd.Parameters.Add(new SqlParameter("@jsonFinal", jsonFinal));
                    cmd.Parameters.Add(new SqlParameter("@ValorProceso", ValorProceso));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
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

        public string DevolverArchivoBase64(string rutaDocumentoResul)
        {
            string StringBase64 = "";
            if (File.Exists(rutaDocumentoResul))
            {
                byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                StringBase64 = Convert.ToBase64String(archivoBytes);
            }
            return StringBase64;
        }

        public string DevolverArchivoPDFBase64(string rutaDocumentoResul)
        {
            string StringBase64 = "";
            if (File.Exists(rutaDocumentoResul))
            {
                //SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                //SelectPdf.PdfDocument doc = converter.ConvertHtmlString("");
                //doc.Save(AppDomain.CurrentDomain.BaseDirectory + "Template\\invoice1.pdf");
                //byte[] data = doc.Save();
                //var result = Convert.ToBase64String(data);
                //doc.Close();

                //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                //StringBase64 = Convert.ToBase64String(archivoBytes);
            }
            return StringBase64;
        }

        



        private Pagos MapToPagos(SqlDataReader reader)
        {
            return new Pagos()
            {
                IdPago = (Int64)reader["IdPago"],
                IdCuentaPorPagar = (Int64)reader["IdCuentaPorPagar"],
                FormaPago = reader["FormaPago"].ToString(),
                Valor = (decimal)reader["Valor"],
                Saldo = (decimal)reader["Saldo"],
                Total = (decimal)reader["Total"],
                Observacion = reader["Observacion"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                FechaPago = (DateTime)reader["FechaPago"],
            };
        }

        private PlantillaCuentas MapToPlantillaCuentas(SqlDataReader reader)
        {
            return new PlantillaCuentas()
            {
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                Descripcion = reader["Descripcion"].ToString(),
                Debe = (decimal)reader["Debe"],
                Haber = (decimal)reader["Haber"],
                tipo = reader["tipo"].ToString(),
            };
        }

        private PlantillaCuentas MapToPlantillaCuentasDetalle(SqlDataReader reader)
        {
            return new PlantillaCuentas()
            {
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                Descripcion = reader["Descripcion"].ToString(),
                DescripcionMovimiento = reader["DescripcionMovimiento"].ToString(),
                Debe = (decimal)reader["Debe"],
                Haber = (decimal)reader["Haber"],
                tipo = reader["tipo"].ToString(),
            };
        }

        private PlantillaCuentas MapToBusquedaCuenta(SqlDataReader reader)
        {
            return new PlantillaCuentas()
            {
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                Descripcion = reader["Descripcion"].ToString(),
                Debe = (decimal)reader["Debe"],
                Haber = (decimal)reader["Haber"],
            };
        }

        private PlantillaCuentas MapToDescripcionCuenta(SqlDataReader reader)
        {
            return new PlantillaCuentas()
            {
                Descripcion = reader["Descripcion"].ToString(),
                FechaRegistro = (DateTime)reader["FechaRegistro"],
            };
        }
    }
}
