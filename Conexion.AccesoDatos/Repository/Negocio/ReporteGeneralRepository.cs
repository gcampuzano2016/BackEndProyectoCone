using ClosedXML.Excel;
using Conexion.AccesoDatos.Repository.CArchivo;
using Conexion.Entidad.Administracion;
using Conexion.Entidad.Negocio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Conexion.AccesoDatos.Repository.Negocio
{
    public class ReporteGeneralRepository
    {
        private readonly string _connectionString;

        public ReporteGeneralRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<ReporteGeneral>> GetByMostrarReporteGeneral(Int64 IdMedio,int Anio, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteGeneral", sql))
                {
                    cmd.CommandTimeout = 60 * 5;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteGeneral>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (Tipo == 1)
                            {
                                response.Add(MapToReporteGeneral(reader));
                            }
                            else if (Tipo == 2){
                                response.Add(MapToReporteGeneral2(reader));
                            }
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ResumenCtasPagarCobrar>> GetByReporteDetallePagarCobrar(string NumDocumento, int Tipo)
        {
            string resul = "";
            int Valor = 0;
            //if (NumDocumento.Contains("-"))
            //{
            //    resul = NumDocumento.Substring(8, 9);
            //    Valor = Convert.ToInt32(resul);
            //}
            //else
            //{
            //    resul = NumDocumento;
            //    Valor = Convert.ToInt32(resul);
            //}

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ReporteDetallePagarCobrar", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@NumDocumento", NumDocumento));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ResumenCtasPagarCobrar>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteCobrarPagar(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ReporteGeneral>> GetByMostrarReportePresupuesto(Int64 IdMedio, Int64 IdEmpleado, int Anio, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReportePresupuesto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteGeneral>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReportePresupuesto(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReportePresupuestoArchivoBase64(Int64 IdMedio, Int64 IdEmpleado, int Anio, int Tipo,string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReportePresupuesto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteGeneral>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteGeneral(reader));
                        }
                    }


                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    foreach (ReporteGeneral s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Descripcion;
                        ws.Cell("B" + cont2.ToString()).Value = s.Enero;
                        ws.Cell("C" + cont2.ToString()).Value = s.Febrero;
                        ws.Cell("D" + cont2.ToString()).Value = s.Marzo;
                        ws.Cell("E" + cont2.ToString()).Value = s.Abril;
                        ws.Cell("F" + cont2.ToString()).Value = s.Mayo;
                        ws.Cell("G" + cont2.ToString()).Value = s.Junio;
                        ws.Cell("H" + cont2.ToString()).Value = s.Julio;
                        ws.Cell("I" + cont2.ToString()).Value = s.Agosto;
                        ws.Cell("J" + cont2.ToString()).Value = s.Septiembre;
                        ws.Cell("K" + cont2.ToString()).Value = s.Octubre;
                        ws.Cell("L" + cont2.ToString()).Value = s.Noviembre;
                        ws.Cell("M" + cont2.ToString()).Value = s.Diciembre;
                        ws.Cell("N" + cont2.ToString()).Value = s.Total;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteGeneralArchivoBase64(Int64 IdMedio, int Anio, int Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteGeneral", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteGeneral>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (Tipo == 1)
                            {
                                response.Add(MapToReporteGeneral(reader));
                            }
                            else if (Tipo == 2)
                            {
                                response.Add(MapToReporteGeneral2(reader));
                            }
                        }
                    }


                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    foreach (ReporteGeneral s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Descripcion;
                        ws.Cell("B" + cont2.ToString()).Value = s.NumeroConex;
                        ws.Cell("C" + cont2.ToString()).Value = s.Enero;
                        ws.Cell("D" + cont2.ToString()).Value = s.Febrero;
                        ws.Cell("E" + cont2.ToString()).Value = s.Marzo;
                        ws.Cell("F" + cont2.ToString()).Value = s.Abril;
                        ws.Cell("G" + cont2.ToString()).Value = s.Mayo;
                        ws.Cell("H" + cont2.ToString()).Value = s.Junio;
                        ws.Cell("I" + cont2.ToString()).Value = s.Julio;
                        ws.Cell("J" + cont2.ToString()).Value = s.Agosto;
                        ws.Cell("K" + cont2.ToString()).Value = s.Septiembre;
                        ws.Cell("L" + cont2.ToString()).Value = s.Octubre;
                        ws.Cell("M" + cont2.ToString()).Value = s.Noviembre;
                        ws.Cell("N" + cont2.ToString()).Value = s.Diciembre;
                        ws.Cell("O" + cont2.ToString()).Value = s.Total;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;

                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByGenerarATS(DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            string mes = FechaInicio.Month.ToString();
            if(mes .Length == 1)
            {
                mes = "0" + mes;
            }
            string Anio = FechaInicio.Year.ToString();
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            EntRespuesta ent = new EntRespuesta();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();


            CargarXLSX cargar1 = new CargarXLSX(_connectionString);
            archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
            ent = cargar1.ConsultarATS(FechaInicio, FechaFinal, Tipo);

            string ruta = archivo.RutaArchivo;
            cargar.RutaArchivo = ruta + cargar.nombreArchivo;
            string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + mes + Anio + "_" + fecha + archivo.Extencion;
            ruta = GenerarXml(ent.resultadoSet, rutaDocumentoResul);
            byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
            string archivoBase64 = Convert.ToBase64String(archivoBytes);

            CargarArchivoBase64 generica = new CargarArchivoBase64();

            generica.NombreArchivo = archivo.NombreArchivoSalida + mes + Anio + "_" + fecha + archivo.Extencion;
            generica.ArchivoBase64 = archivoBase64;

            response2.Add(generica);

            return response2;
        }

        public string GenerarXml(DataSet data, string rutaDocumento)
        {
            string ruta = rutaDocumento;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xml.DocumentElement;
            xml.InsertBefore(xmlDeclaration, root);
            XmlElement elementoLista = xml.CreateElement(string.Empty, "iva", string.Empty);
            xml.AppendChild(elementoLista);

            #region INFORMANTES
            for (int x = 0; x <= Convert.ToInt32(data.Tables[0].Rows.Count) - 1; x++)
            {
                for (int i = 0; i <= Convert.ToInt32(data.Tables[0].Columns.Count) - 1; i++)
                {
                    XmlElement elementoCompra = xml.CreateElement(string.Empty, data.Tables[0].Columns[i].ColumnName.ToString(), string.Empty);
                    XmlText text1 = null/* TODO Change to default(_) if this is not a reference type */;
                    text1 = xml.CreateTextNode(data.Tables[0].Rows[x][i].ToString().Trim());
                    elementoCompra.AppendChild(text1);
                    elementoLista.AppendChild(elementoCompra);
                }
            }
            #endregion

            #region COMPRAS
            XmlElement elementoCompras = xml.CreateElement(string.Empty, "compras", string.Empty);
            elementoLista.AppendChild(elementoCompras);
            bool Bandera = false;
            for (int x = 0; x <= Convert.ToInt32(data.Tables[1].Rows.Count) - 1; x++)
            {
                XmlElement elementoDetalleCompras = xml.CreateElement(string.Empty, "detalleCompras", string.Empty);
                elementoCompras.AppendChild(elementoDetalleCompras);
                int Valor = 0;
                XmlElement elementopagoExterior = null;
                XmlElement elementoFormaPago = null;
                XmlElement elementoair = null;
                XmlElement detalleAir = null;
                for (int i = 1; i <= Convert.ToInt32(data.Tables[1].Columns.Count) - 1; i++)
                {
                    XmlElement elementoCompra = xml.CreateElement(string.Empty, data.Tables[1].Columns[i].ColumnName.ToString(), string.Empty);
                    XmlText text1 = null;

                    if (data.Tables[1].Columns[i].ColumnName.ToString() == "pagoLocExt" || data.Tables[1].Columns[i].ColumnName.ToString() == "paisEfecPago" || data.Tables[1].Columns[i].ColumnName.ToString() == "aplicConvDobTrib" || data.Tables[1].Columns[i].ColumnName.ToString() == "pagExtSujRetNorLeg" || data.Tables[1].Columns[i].ColumnName.ToString() == "pagoRegFis")
                    {
                        if (Bandera == false)
                        {
                            elementopagoExterior = xml.CreateElement(String.Empty, "pagoExterior", String.Empty);
                            elementoDetalleCompras.AppendChild(elementopagoExterior);
                            Bandera = true;
                        }
                        text1 = xml.CreateTextNode(data.Tables[1].Rows[x][i].ToString().Trim());
                        elementoCompra.AppendChild(text1);
                        elementopagoExterior.AppendChild(elementoCompra);
                        Valor = Valor + 1;
                        if (Valor == 4)
                        {
                            Bandera = false;
                            # region forma de pago
                            DataTable dtResp = new DataTable();
                            DataView Resul = data.Tables[2].DefaultView;
                            Resul.RowFilter = "[Código de compra]=" + data.Tables[1].Rows[x][0].ToString() + "";
                            dtResp = Resul.ToTable("UniqueLastNames", true, "formapago");
                            if (dtResp.Rows.Count > 0)
                            {
                                elementoFormaPago = xml.CreateElement(string.Empty, "formasDePago", string.Empty);
                                elementoDetalleCompras.AppendChild(elementoFormaPago);
                                elementoCompra = xml.CreateElement(string.Empty, "formaPago", string.Empty);
                                text1 = xml.CreateTextNode(dtResp.Rows[0][0].ToString().Trim());
                                elementoCompra.AppendChild(text1);
                                elementoFormaPago.AppendChild(elementoCompra);
                            }

                            #endregion

                            #region DetalleImpuestos
                            DataTable dtResp2 = new DataTable();
                            DataView Resul2 = data.Tables[3].DefaultView;
                            Resul2.RowFilter = "[Código de compra]=" + data.Tables[1].Rows[x][0].ToString() + "";
                            dtResp2 = Resul2.ToTable("UniqueLastNames", true, "codRetAir", "baseImpAir", "porcentajeAir", "valRetAir");
                            if (dtResp2.Rows.Count > 0)
                            {
                                elementoair = xml.CreateElement(string.Empty, "air", string.Empty);
                                elementoDetalleCompras.AppendChild(elementoair);
                                for (int y = 0; y <= Convert.ToInt32(dtResp2.Rows.Count) - 1; y++)
                                {
                                    detalleAir = xml.CreateElement(string.Empty, "detalleAir", string.Empty);
                                    elementoair.AppendChild(detalleAir);
                                    for (int z = 0; z <= Convert.ToInt32(dtResp2.Columns.Count) - 1; z++)
                                    {
                                        elementoCompra = xml.CreateElement(string.Empty, dtResp2.Columns[z].ColumnName.ToString(), string.Empty);
                                        text1 = xml.CreateTextNode(dtResp2.Rows[y][z].ToString().Trim());
                                        elementoCompra.AppendChild(text1);
                                        detalleAir.AppendChild(elementoCompra);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        text1 = xml.CreateTextNode(data.Tables[1].Rows[x][i].ToString());
                        if (text1.InnerText == "   " || text1.InnerText == "")
                        {

                        }
                        else
                        {
                            elementoCompra.AppendChild(text1);
                            elementoDetalleCompras.AppendChild(elementoCompra);
                        }
                    }
                }
            }
            #endregion

            #region VENTAS
            if (data.Tables[4].Rows.Count > 0)
            {
                XmlElement elementoVentas = xml.CreateElement(string.Empty, "ventas", string.Empty);
                elementoLista.AppendChild(elementoVentas);
                int Valor = 0;
                int tipoCom = 0;
                for (int x = 0; x <= Convert.ToInt32(data.Tables[4].Rows.Count) - 1; x++)
                {
                    XmlElement elementoDetalleVentas = xml.CreateElement(string.Empty, "detalleVentas", string.Empty);
                    elementoVentas.AppendChild(elementoDetalleVentas);
                    tipoCom = 0;
                    Valor = 0;
                    for (int i = 0; i <= Convert.ToInt32(data.Tables[4].Columns.Count) - 1; i++)
                    {
                        XmlElement elementoVenta = xml.CreateElement(string.Empty, data.Tables[4].Columns[i].ColumnName.ToString(), string.Empty);
                        XmlText text2 = null;
                        text2 = xml.CreateTextNode(data.Tables[4].Rows[x][i].ToString().Trim());
                        if (elementoVenta.Name == "tipoComprobante" && text2.InnerText == "04")
                        {
                            tipoCom = 1;
                        }

                        if (text2.InnerText == "" && elementoVenta.Name == "parteRelVtas")
                        {
                        }
                        else
                        {
                            elementoVenta.AppendChild(text2);
                            elementoDetalleVentas.AppendChild(elementoVenta);
                        }
                        Valor = Valor + 1;
                        if (Valor == 13 && tipoCom == 0)
                        {
                            XmlText text1 = null/* TODO Change to default(_) if this is not a reference type */;
                            XmlElement elementoFormaPago = null/* TODO Change to default(_) if this is not a reference type */;
                            DataTable dtResp = new DataTable();
                            DataView Resul = data.Tables[6].DefaultView;
                            Resul.RowFilter = "Rucedula='" + data.Tables[4].Rows[x][1].ToString() + "'";
                            dtResp = Resul.ToTable("UniqueLastNames", true, "formapago");
                            if (dtResp.Rows.Count > 0)
                            {
                                elementoFormaPago = xml.CreateElement(string.Empty, "formasDePago", string.Empty);
                                elementoDetalleVentas.AppendChild(elementoFormaPago);
                                elementoVenta = xml.CreateElement(string.Empty, "formaPago", string.Empty);
                                text1 = xml.CreateTextNode(dtResp.Rows[0][0].ToString().Trim());
                                elementoVenta.AppendChild(text1);
                                elementoFormaPago.AppendChild(elementoVenta);
                            }
                            Valor = 0;
                        }
                    }
                }

                #region VENTAS ESTABLECIMIENTO
                XmlElement ventasEstablecimientos = xml.CreateElement(string.Empty, "ventasEstablecimiento", string.Empty);
                elementoLista.AppendChild(ventasEstablecimientos);

                XmlElement elementoventaEst = xml.CreateElement(string.Empty, "ventaEst", string.Empty);
                ventasEstablecimientos.AppendChild(elementoventaEst);
                for (int x = 0; x <= Convert.ToInt32(data.Tables[5].Rows.Count) - 1; x++)
                {
                    for (int i = 0; i <= Convert.ToInt32(data.Tables[5].Columns.Count) - 1; i++)
                    {
                        XmlElement elementoEstablecimiento = xml.CreateElement(string.Empty, data.Tables[5].Columns[i].ColumnName.ToString(), string.Empty);
                        XmlText text3 = null;
                        text3 = xml.CreateTextNode(data.Tables[5].Rows[x][i].ToString().Trim());
                        elementoEstablecimiento.AppendChild(text3);
                        elementoventaEst.AppendChild(elementoEstablecimiento);
                    }
                }
                #endregion

            }
            #endregion

            #region ESXPORTACION
            if (data.Tables[7].Rows.Count > 0)
            {
                XmlElement elementoExportacion = xml.CreateElement(string.Empty, "exportaciones", string.Empty);
                elementoLista.AppendChild(elementoExportacion);
                for (int x = 0; x <= Convert.ToInt32(data.Tables[7].Rows.Count) - 1; x++)
                {
                    XmlElement elementodetalleExportaciones = xml.CreateElement(string.Empty, "detalleExportaciones", string.Empty);
                    elementoExportacion.AppendChild(elementodetalleExportaciones);
                    for (int i = 0; i <= Convert.ToInt32(data.Tables[7].Columns.Count) - 1; i++)
                    {
                        XmlElement elemento = xml.CreateElement(string.Empty, data.Tables[7].Columns[i].ColumnName.ToString(), string.Empty);
                        XmlText text3 = null;
                        text3 = xml.CreateTextNode(data.Tables[7].Rows[x][i].ToString().Trim());
                        elemento.AppendChild(text3);
                        elementodetalleExportaciones.AppendChild(elemento);
                    }
                }
            }
            #endregion

            xml.InnerText.Replace("<pagoRegFis>NA</pagoRegFis>", "");
            xml.Save(ruta);
            return ruta;
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteBalanceComprobacionArchivoBase64(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo,string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

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

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    ws.Cell("A1").Value = FechaFinal;
                    ws.Cell("A1").Style.Font.Bold = true;

                    foreach (BalanceComprobacion s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.CODIGO;
                        ws.Cell("B" + cont2.ToString()).Value = s.CUENTA;
                        ws.Cell("C" + cont2.ToString()).Value = s.INICIAL;
                        ws.Cell("D" + cont2.ToString()).Value = s.DEBITOS;
                        ws.Cell("E" + cont2.ToString()).Value = s.CREDITOS;
                        ws.Cell("F" + cont2.ToString()).Value = s.SALDOFINAL;                      
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);


                    return response2;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByConsultaImpuestoIvaBase64(DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Consulta_ImpuestoIva", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ImpuestoIva>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToImpuestoIva(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento, 0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);

                    foreach (ImpuestoIva s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = "'" + s.ruc;
                        ws.Cell("B" + cont2.ToString()).Value = s.fechaemision;
                        ws.Cell("C" + cont2.ToString()).Value = s.serie;
                        ws.Cell("D" + cont2.ToString()).Value = "'" + s.secuencial;
                        ws.Cell("E" + cont2.ToString()).Value = "'" + s.claveacceso;
                        ws.Cell("F" + cont2.ToString()).Value = s.totalfactura;
                        ws.Cell("G" + cont2.ToString()).Value = s.porcentaje;
                        ws.Cell("H" + cont2.ToString()).Value = s.valorRetenido;
                        cont2++;
                    }

                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);


                    return response2;
                }
            }
        }

        private ImpuestoIva MapToImpuestoIva(SqlDataReader reader)
        {
            return new ImpuestoIva()
            {
                ruc = reader["ruc"].ToString(),
                fechaemision = reader["fechaemision"].ToString(),
                serie = reader["serie"].ToString(),
                secuencial = reader["secuencial"].ToString(),
                claveacceso = reader["claveacceso"].ToString(),
                totalfactura = (decimal)reader["totalfactura"],
                porcentaje = reader["porcentaje"].ToString(),
                valorRetenido = (decimal)reader["valorRetenido"],
            };
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteEstadoResultadosArchivoBase64(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteEstadoResultados", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<EstadoFinanciero>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToEstadoResultados(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    ws.Cell("A1").Value = FechaFinal;
                    ws.Cell("A1").Style.Font.Bold = true;
                    if(Tipo == 2)
                    {
                        foreach (EstadoFinanciero s in response)
                        {
                            ws.Cell("A" + cont2.ToString()).Value = s.CODIGO;

                            if (s.CUENTA == "TOTAL ACTIVO" || s.CUENTA == "TOTAL PASIVO" || s.CUENTA== "TOTAL PATRIMONIO" || s.CUENTA == "TOTAL PASIVO Y PATRIMONIO ======>")
                            {
                                ws.Cell("B" + cont2.ToString()).Value = s.CUENTA;
                                ws.Cell("B" + cont2.ToString()).Style.Font.Bold = true;
                                ws.Cell("B" + cont2.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            }
                            else
                            {
                                ws.Cell("B" + cont2.ToString()).Value = s.CUENTA;

                            }
                            ws.Cell("C" + cont2.ToString()).Value = s.PARCIAL;

                            if (s.CUENTA == "TOTAL ACTIVO" || s.CUENTA == "TOTAL PASIVO" || s.CUENTA == "TOTAL PATRIMONIO" || s.CUENTA == "TOTAL PASIVO Y PATRIMONIO ======>")
                            {
                                ws.Cell("D" + cont2.ToString()).Value = s.SUBTOTAL;
                                ws.Cell("D" + cont2.ToString()).Style.Font.Bold = true;
                            }
                            else
                            {
                                ws.Cell("D" + cont2.ToString()).Value = s.SUBTOTAL;
                            }
                            //ws.Cell("E" + cont2.ToString()).Value = s.TOTAL;
                            cont2++;
                        }
                    }
                    else
                    {
                        foreach (EstadoFinanciero s in response)
                        {

                            ws.Cell("A" + cont2.ToString()).Value = s.CODIGO;
                            if (s.CUENTA == "TOTAL INGRESOS -->" || s.CUENTA == "TOTAL COSTOS -->" || s.CUENTA == "TOTAL GASTOS -->" || s.CUENTA == "UTILIDAD ======>")
                            {
                                ws.Cell("B" + cont2.ToString()).Value = s.CUENTA;
                                ws.Cell("B" + cont2.ToString()).Style.Font.Bold = true;
                                ws.Cell("B" + cont2.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            }
                            else
                            {
                                ws.Cell("B" + cont2.ToString()).Value = s.CUENTA;
                            }
                            ws.Cell("C" + cont2.ToString()).Value = s.PARCIAL;
                            if (s.CUENTA == "TOTAL INGRESOS -->" || s.CUENTA == "TOTAL COSTOS -->" || s.CUENTA == "TOTAL GASTOS -->" || s.CUENTA == "UTILIDAD ======>")
                            {
                                ws.Cell("D" + cont2.ToString()).Value = s.SUBTOTAL;
                                ws.Cell("D" + cont2.ToString()).Style.Font.Bold = true;
                            }
                            else
                            {
                                ws.Cell("D" + cont2.ToString()).Value = s.SUBTOTAL;
                            }
                            //ws.Cell("E" + cont2.ToString()).Value = s.TOTAL;
                            cont2++;
                        }
                    }


                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);


                    return response2;
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarProveedorArchivoBase64(Int64 IdProveedor, Int32 Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarProveedor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdProveedor", IdProveedor));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<Proveedor>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToProveedor(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    //ws.Cell("A1").Value = FechaFinal;
                    //ws.Cell("A1").Style.Font.Bold = true;

                    foreach (Proveedor s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Nombre;
                        ws.Cell("B" + cont2.ToString()).Value = s.NombreComercial;
                        ws.Cell("C" + cont2.ToString()).Value = "'"+s.RuCedula;
                        ws.Cell("D" + cont2.ToString()).Value = s.Direccion;
                        ws.Cell("E" + cont2.ToString()).Value = s.Telefono;
                        ws.Cell("F" + cont2.ToString()).Value = s.CodContable;
                        ws.Cell("G" + cont2.ToString()).Value = s.Descripcion;
                        cont2++;
                    }

                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }

                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);


                    return response2;
                }
            }
        }

        public async Task<IEnumerable<SeguimientoForeCast>> GetByReporteSeguimiento(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ReporteSeguimiento", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<SeguimientoForeCast>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteSeguimiento(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ComisionFinal>> GetByMostrarReporteComision(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteComision", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ComisionFinal>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteComision(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteComisionArchivoBase64(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo,string Perfil, string TipoDocumento)
        {

            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteComision", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ComisionFinal>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteComision(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    foreach (ComisionFinal s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.NumContrato;
                        ws.Cell("B" + cont2.ToString()).Value = s.Medios;
                        ws.Cell("C" + cont2.ToString()).Value = s.Anunciante;
                        ws.Cell("D" + cont2.ToString()).Value = s.FechaInicio;
                        ws.Cell("E" + cont2.ToString()).Value = s.FechaFinal;
                        ws.Cell("F" + cont2.ToString()).Value = s.NumDocumento;
                        ws.Cell("G" + cont2.ToString()).Value = s.EstadoPago;
                        ws.Cell("H" + cont2.ToString()).Value = s.NombresApellidos;
                        ws.Cell("I" + cont2.ToString()).Value = s.ValorBruto;
                        ws.Cell("J" + cont2.ToString()).Value = s.ValorNeto;
                        ws.Cell("K" + cont2.ToString()).Value = s.Porcentaje;
                        ws.Cell("L" + cont2.ToString()).Value = s.Comision;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);
                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }


        public async Task<IEnumerable<CobrosFacturas>> GetByMostrarReporteFacturasCobradas(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteFacturasCobradas", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<CobrosFacturas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteCobrosFacturas(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ConsumoPautas>> GetByMostrarReporteConsumo(Int64 IdMedio,Int64 IdEmpleado,string FechaInicio,string FechaFinal,string NumConex ,Int32 Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteConsumo", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@NumConex", NumConex));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ConsumoPautas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteConsumoPautas(reader));
                        }
                    }

                    return response;
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteFacturasCobradasArchivoBase64(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {

            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteFacturasCobradas", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<CobrosFacturas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteCobrosFacturas(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    foreach (CobrosFacturas s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Medios;
                        ws.Cell("B" + cont2.ToString()).Value = s.Valor;
                        ws.Cell("C" + cont2.ToString()).Value = s.NumDocumento;
                        ws.Cell("D" + cont2.ToString()).Value = s.EstadoPago;
                        ws.Cell("E" + cont2.ToString()).Value = s.Conex;
                        ws.Cell("F" + cont2.ToString()).Value = s.ValorBruto;
                        ws.Cell("G" + cont2.ToString()).Value = s.ValorNeto;
                        ws.Cell("H" + cont2.ToString()).Value = s.Porcentaje;
                        ws.Cell("I" + cont2.ToString()).Value = s.Comision;
                        ws.Cell("J" + cont2.ToString()).Value = s.Vendedor;
                        ws.Cell("K" + cont2.ToString()).Value = s.EstadoComision;
                        ws.Cell("L" + cont2.ToString()).Value = s.FechaCobroFactura;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteConsumoArchivoBase64(Int64 IdMedio, Int64 IdEmpleado, string FechaInicio, string FechaFinal, string NumConex, Int32 Tipo, string TipoDocumento)
        {

            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteConsumo", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@NumConex", NumConex));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ConsumoPautas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteConsumoPautas(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    foreach (ConsumoPautas s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.NumContrato;
                        ws.Cell("B" + cont2.ToString()).Value = s.Anunciante;
                        ws.Cell("C" + cont2.ToString()).Value = s.TotalNegocio;
                        ws.Cell("D" + cont2.ToString()).Value = s.TotalSegundos;
                        ws.Cell("E" + cont2.ToString()).Value = s.TotalNegocioConsumido;
                        ws.Cell("F" + cont2.ToString()).Value = s.TotalSegundosConsumido;
                        ws.Cell("G" + cont2.ToString()).Value = s.SaldoTotalNegocio;
                        ws.Cell("H" + cont2.ToString()).Value = s.SaldoTotalSegundos;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarPlanCuentasSaldoFinalCeroBase64(Int64 IdPlanCuenta, DateTime FechaInicio, Int32 Tipo, string TipoDocumento)
        {

            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPlanCuentasSaldoFinalCero", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PlanCuenta>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPlanCuentas(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    foreach (PlanCuenta s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Codigo;
                        ws.Cell("B" + cont2.ToString()).Value = s.Descripcion;
                        ws.Cell("C" + cont2.ToString()).Value = s.SaldoInicial;
                        ws.Cell("D" + cont2.ToString()).Value = s.SaldoFinal;                     
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarPlanCuentasSaldoFinalBase64(Int64 IdPlanCuenta, DateTime FechaInicio, Int32 Tipo, string TipoDocumento)
        {

            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarPlanCuentasSaldoFinal", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<PlanCuenta>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPlanCuentas(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);


                    foreach (PlanCuenta s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Codigo;
                        ws.Cell("B" + cont2.ToString()).Value = s.Descripcion;
                        ws.Cell("C" + cont2.ToString()).Value = s.SaldoInicial;
                        ws.Cell("D" + cont2.ToString()).Value = s.SaldoFinal;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteEstadoCuentaArchivoBase64(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            string[] datosDoc;
            datosDoc = TipoDocumento.Split(';');
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

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

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, datosDoc[0],0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);

                    ws.Cell("A1").Value = datosDoc[1];
                    ws.Cell("A1").Style.Font.Bold = true;
                    ws.Cell("B1").Value = datosDoc[2];
                    ws.Cell("B1").Style.Font.Bold = true;
                    ws.Cell("C1").Value = datosDoc[3];
                    ws.Cell("C1").Style.Font.Bold = true;
                    ws.Cell("E1").Value = datosDoc[4];
                    ws.Cell("E1").Style.Font.Bold = true;


                    foreach (EstadoCuenta s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.FechaRegistro;
                        ws.Cell("B" + cont2.ToString()).Value = s.Debito;
                        ws.Cell("C" + cont2.ToString()).Value = s.Credito;
                        ws.Cell("D" + cont2.ToString()).Value = s.Concepto;
                        ws.Cell("E" + cont2.ToString()).Value = s.Saldo;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteEstadoCuentaLibroMayorArchivoBase64(Int64 IdPlanCuenta, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            string[] datosDoc;
            datosDoc = TipoDocumento.Split(';');
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteEstadoCuentaLibroMayor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPlanCuenta", IdPlanCuenta));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<LibroMayor>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToLibroMayor(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, datosDoc[0],0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);

                    ws.Cell("A1").Value = datosDoc[1];
                    ws.Cell("A1").Style.Font.Bold = true;
                    ws.Cell("B1").Value = datosDoc[2];
                    ws.Cell("B1").Style.Font.Bold = true;
                   
                    foreach (LibroMayor s in response)
                    {

                        ws.Cell("A" + cont2.ToString()).Value = s.FECHA;
                        
                        if (s.BANDERA == "PINTAR")
                        {
                            ws.Cell("B" + cont2.ToString()).Value = s.CONCEPTO;
                            ws.Cell("B" + cont2.ToString()).Style.Font.Bold = true;
                        }
                        else
                        {
                            ws.Cell("B" + cont2.ToString()).Value = s.CONCEPTO;
                        }

                        ws.Cell("C" + cont2.ToString()).Value = s.NUMDOCUMENTO;
                        ws.Cell("D" + cont2.ToString()).Value = "'"+s.RUCEDULA;
                        ws.Cell("E" + cont2.ToString()).Value = s.BENEFICIARIO;

                        if (s.CONCEPTO == "TOTAL:")
                        {
                            ws.Cell("B" + cont2.ToString()).Value = s.CONCEPTO;
                            ws.Cell("B" + cont2.ToString()).Style.Font.Bold = true;
                            ws.Cell("F" + cont2.ToString()).Value = s.DEBITO;
                            ws.Cell("F" + cont2.ToString()).Style.Font.Bold = true;
                            ws.Cell("G" + cont2.ToString()).Value = s.CREDITO;
                            ws.Cell("G" + cont2.ToString()).Style.Font.Bold = true;
                        }
                        else
                        {
                            ws.Cell("B" + cont2.ToString()).Value = s.CONCEPTO;
                            ws.Cell("F" + cont2.ToString()).Value = s.DEBITO;
                            ws.Cell("G" + cont2.ToString()).Value = s.CREDITO;
                        }

                        if (s.CONCEPTO == "TOTAL MOVIMIENTOS")
                        {
                            ws.Cell("F" + cont2.ToString()).Value = s.DEBITO;
                            ws.Cell("F" + cont2.ToString()).Style.Font.Bold = true;
                            ws.Cell("G" + cont2.ToString()).Value = s.CREDITO;
                            ws.Cell("G" + cont2.ToString()).Style.Font.Bold = true;
                        }
                        else
                        {
                            ws.Cell("F" + cont2.ToString()).Value = s.DEBITO;
                            ws.Cell("G" + cont2.ToString()).Value = s.CREDITO;
                        }

                        if (s.BANDERA == "PINTAR")
                        {
                            ws.Cell("H" + cont2.ToString()).Value = s.SALDO;
                            ws.Cell("H" + cont2.ToString()).Style.Font.Bold = true;
                        }
                        else
                        {
                            ws.Cell("H" + cont2.ToString()).Value = s.SALDO;
                        }
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        private LibroMayor MapToLibroMayor(SqlDataReader reader)
        {
            return new LibroMayor()
            {
                FECHA = reader["FECHA"].ToString(),
                CONCEPTO = reader["CONCEPTO"].ToString(),
                NUMDOCUMENTO = reader["NUMDOCUMENTO"].ToString(),
                RUCEDULA = reader["RUCEDULA"].ToString(),
                BENEFICIARIO = reader["BENEFICIARIO"].ToString(),
                DEBITO = reader["DEBITO"].ToString(),
                CREDITO = reader["CREDITO"].ToString(),
                SALDO = reader["SALDO"].ToString(),
                BANDERA = reader["BANDERA"].ToString(),
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
                FechaRegistro = (DateTime)reader["FechaRegistro"],
            };
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarFacturaPorPagarFechaArchivoBase64(Int64 IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento, string TipoDocumentos)
        {

            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaPorPagarFecha", sql))
                {
                    cmd.CommandTimeout = 60 * 5;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@TipoDocumento", TipoDocumentos));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<CuentasPorPagar>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                                response.Add(MapToFacturaPorPagarFecha2(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    decimal SaldoTotal = 0;
                    foreach (CuentasPorPagar s in response)
                    {
                        if (s.RazonSocial.Trim() != "TOTALES:")
                        {
                            SaldoTotal = SaldoTotal + s.Saldo;
                        }
                    }

                    ws.Cell("D1").Value = FechaFinal;
                    ws.Cell("D1").Style.Font.Bold = true;

                    ws.Cell("F1").Value = SaldoTotal;
                    ws.Cell("F1").Style.Font.Bold = true;

                    foreach (CuentasPorPagar s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.RazonSocial;
                        ws.Cell("B" + cont2.ToString()).Value = s.CompraTarifa0;
                        ws.Cell("C" + cont2.ToString()).Value = s.CompraTarifa12;
                        ws.Cell("D" + cont2.ToString()).Value = s.Iva;
                        ws.Cell("E" + cont2.ToString()).Value = s.ValorTotal;
                        ws.Cell("F" + cont2.ToString()).Value = s.Saldo;
                        ws.Cell("G" + cont2.ToString()).Value = s.NumDocumento;
                        ws.Cell("H" + cont2.ToString()).Value = s.EstadoPago;
                        ws.Cell("I" + cont2.ToString()).Value = s.FechaRegistro;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    if(Tipo == 1)
                    {
                        generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    }
                    else if(Tipo == 2)
                    {
                        generica.NombreArchivo = archivo.NombreArchivoSalida +"_a_la_fecha" + "_" + fecha + archivo.Extencion;
                    }
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarMovimientoContableArchivoBase64(Int64 IdProcesoContable, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo,string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

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

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);

                    ws.Cell("A1").Value = FechaInicio;
                    ws.Cell("A1").Style.Font.Bold = true;

                    ws.Cell("B1").Value = FechaFinal;
                    ws.Cell("B1").Style.Font.Bold = true;

                    foreach (RegistroContable s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.FechaRegistro;
                        ws.Cell("B" + cont2.ToString()).Value = s.Concepto;
                        ws.Cell("C" + cont2.ToString()).Value = s.Debe;
                        ws.Cell("D" + cont2.ToString()).Value = s.Haber;
                        ws.Cell("E" + cont2.ToString()).Value = s.Codigo;
                        ws.Cell("F" + cont2.ToString()).Value = s.Descripcion;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarFacturaPorCobrarFechaArchivoBase64(Int64 IdMedio, DateTime FechaInicio, DateTime FechaFinal, Int32 Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarFacturaPorCobrarFecha", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@FechaInicio", FechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@FechaFinal", FechaFinal));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<FacturaContrato>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToFacturaPorCobrarFecha(reader));
                        }
                    }
                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 3;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);

                    decimal SaldoTotal = 0;
                    foreach (FacturaContrato s in response)
                    {
                        if(s.NombreMedio.Trim() != "TOTALES:")
                        {
                            SaldoTotal = SaldoTotal + s.Saldo;
                        }
                    }

                    ws.Cell("B1").Value = FechaFinal;
                    ws.Cell("B1").Style.Font.Bold = true;

                    ws.Cell("D1").Value = SaldoTotal;
                    ws.Cell("D1").Style.Font.Bold = true;

                    foreach (FacturaContrato s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.NumDocumento;
                        ws.Cell("B" + cont2.ToString()).Value = s.NombreMedio;
                        ws.Cell("C" + cont2.ToString()).Value = s.ValorCobrar;
                        ws.Cell("D" + cont2.ToString()).Value = s.Iva;
                        ws.Cell("E" + cont2.ToString()).Value = s.ValorCobrar + s.Iva;
                        ws.Cell("F" + cont2.ToString()).Value = s.ValorRenta;
                        ws.Cell("G" + cont2.ToString()).Value = s.ValorIva; 
                        ws.Cell("H" + cont2.ToString()).Value = s.ValorRenta + s.ValorIva;
                        ws.Cell("I" + cont2.ToString()).Value = (s.ValorCobrar + s.Iva)-(s.ValorRenta + s.ValorIva);
                        ws.Cell("J" + cont2.ToString()).Value = s.Saldo;
                        ws.Cell("K" + cont2.ToString()).Value = s.EstadoPago;
                        ws.Cell("L" + cont2.ToString()).Value = s.FechaRegistro;
                        ws.Cell("M" + cont2.ToString()).Value = s.Anunciante;
                        ws.Cell("N" + cont2.ToString()).Value = s.Agencia;
                        ws.Cell("O" + cont2.ToString()).Value = s.NumContrato;
                        cont2++;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();
                    if(Tipo == 2){
                        generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    }
                    else if(Tipo == 3)
                    {
                        generica.NombreArchivo = archivo.NombreArchivoSalida + "_a_la_Fecha" + "_" + fecha + archivo.Extencion;
                    }
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        private FacturaContrato MapToFacturaPorCobrarFecha(SqlDataReader reader)
        {
            return new FacturaContrato()
            {

                IdPagocontrato = (Int64)reader["IdPagocontrato"],
                ValorBruto = (decimal)reader["ValorBruto"],
                ValorCobrar = (decimal)reader["ValorCobrar"],
                ValorRetencion = (decimal)reader["ValorRetencion"],
                Saldo = (decimal)reader["Saldo"],
                ValorNeto = (decimal)reader["ValorNeto"],
                ComisionVendedor = (decimal)reader["ComisionVendedor"],
                ComisionPorcentaje = (decimal)reader["ComisionPorcentaje"],
                NumDocumento = reader["NumDocumento"].ToString(),
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                EstadoPago = reader["EstadoPago"].ToString(),
                NombreMedio = reader["NombreMedio"].ToString(),
                Vendedor = reader["Vendedor"].ToString(),
                Anunciante = reader["Anunciante"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                NumContrato = reader["NumContrato"].ToString(),
                ValorRenta = (decimal)reader["ValorRenta"],
                ValorIva = (decimal)reader["ValorIva"],
                Iva = (decimal)reader["Iva"],
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
                FechaRegistro = (DateTime)reader["FechaRegistro"]
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
                PorRegistrar = (Int32)reader["PorRegistrar"],
                Saldo = (decimal)reader["Saldo"],
            };
        }

        private ReporteGeneral MapToReporteGeneral(SqlDataReader reader)
        {
            return new ReporteGeneral()
            {
                Id = (Int64)reader["Id"],
                Descripcion = reader["Descripcion"].ToString(),
                Enero =reader["Enero"].ToString(),
                Febrero = reader["Febrero"].ToString(),
                Marzo = reader["Marzo"].ToString(),
                Abril = reader["Abril"].ToString(),
                Mayo = reader["Mayo"].ToString(),
                Junio = reader["Junio"].ToString(),
                Julio = reader["Julio"].ToString(),
                Agosto = reader["Agosto"].ToString(),
                Septiembre = reader["Septiembre"].ToString(),
                Octubre = reader["Octubre"].ToString(),
                Noviembre  = reader["Noviembre"].ToString(),
                Diciembre = reader["Diciembre"].ToString(),
                Total = reader["Total"].ToString(),
            };
        }

        private ReporteGeneral MapToReporteGeneral2(SqlDataReader reader)
        {
            return new ReporteGeneral()
            {
                Id = (Int64)reader["Id"],
                Descripcion = reader["Descripcion"].ToString(),
                NumeroConex = (Int32)reader["NumeroConex"],
                Enero = reader["Enero"].ToString(),
                Febrero = reader["Febrero"].ToString(),
                Marzo = reader["Marzo"].ToString(),
                Abril = reader["Abril"].ToString(),
                Mayo = reader["Mayo"].ToString(),
                Junio = reader["Junio"].ToString(),
                Julio = reader["Junio"].ToString(),
                Agosto = reader["Agosto"].ToString(),
                Septiembre = reader["Septiembre"].ToString(),
                Octubre = reader["Octubre"].ToString(),
                Noviembre = reader["Noviembre"].ToString(),
                Diciembre = reader["Diciembre"].ToString(),
                Total = reader["Total"].ToString(),
            };
        }

        private ResumenCtasPagarCobrar MapToReporteCobrarPagar(SqlDataReader reader)
        {
            return new ResumenCtasPagarCobrar()
            {
                IdProcesoContable = (Int64)reader["IdProcesoContable"],
                FechaRegistro = (DateTime)reader["FechaRegistro"],
                Descripcion = reader["Descripcion"].ToString(),
                Valor = (decimal)reader["Valor"],
            };
        }

        private EstadoFinanciero MapToEstadoResultados(SqlDataReader reader)
        {
            return new EstadoFinanciero()
            {
                CODIGO = reader["CODIGO"].ToString(),
                CUENTA = reader["CUENTA"].ToString(),
                PARCIAL = (decimal)reader["PARCIAL"],
                SUBTOTAL = (decimal)reader["SUBTOTAL"],
                TOTAL = (decimal)reader["TOTAL"],
            };
        }

        private Proveedor MapToProveedor(SqlDataReader reader)
        {
            return new Proveedor()
            {
                IdProveedor = (Int64)reader["IdProveedor"],
                IdPlanCuenta = (Int64)reader["IdPlanCuenta"],
                Descripcion = reader["Descripcion"].ToString(),
                Nombre = reader["Nombre"].ToString(),
                NombreComercial = reader["NombreComercial"].ToString(),
                RuCedula = reader["RuCedula"].ToString(),
                Direccion = reader["Direccion"].ToString(),
                Telefono = reader["Telefono"].ToString(),
                Email = reader["Email"].ToString(),
                CodContable = reader["CodContable"].ToString(),
                AutorizacionSri = reader["AutorizacionSri"].ToString(),
                FechaAutorizacion = (DateTime)reader["FechaAutorizacion"],
                FechaCaducidad = (DateTime)reader["FechaCaducidad"],
                Estado = (Int32)reader["Estado"],
                Retencion = reader["Retencion"].ToString(),
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
        private ReporteGeneral MapToReportePresupuesto(SqlDataReader reader)
        {
            return new ReporteGeneral()
            {
                Id = (Int64)reader["Id"],
                Descripcion = reader["Descripcion"].ToString(),
                Enero = reader["Enero"].ToString(),
                Febrero = reader["Febrero"].ToString(),
                Marzo = reader["Marzo"].ToString(),
                Abril = reader["Abril"].ToString(),
                Mayo = reader["Mayo"].ToString(),
                Junio = reader["Junio"].ToString(),
                Julio = reader["Julio"].ToString(),
                Agosto = reader["Agosto"].ToString(),
                Septiembre = reader["Septiembre"].ToString(),
                Octubre = reader["Octubre"].ToString(),
                Noviembre = reader["Noviembre"].ToString(),
                Diciembre = reader["Diciembre"].ToString(),
                Total = reader["Total"].ToString(),
            };
        }

        private ComisionFinal MapToReporteComision(SqlDataReader reader)
        {
            return new ComisionFinal()
            {
                NumContrato = reader["NumContrato"].ToString(),
                Medios = reader["Medios"].ToString(),
                Anunciante = reader["Anunciante"].ToString(),
                FechaInicio = (DateTime)reader["FechaInicio"],
                FechaFinal = (DateTime)reader["FechaFinal"],
                NumDocumento = reader["NumDocumento"].ToString(),
                EstadoPago = reader["EstadoPago"].ToString(),
                NombresApellidos = reader["NombresApellidos"].ToString(),
                ValorBruto = (decimal)reader["ValorBruto"],
                ValorNeto = (decimal)reader["ValorNeto"],
                Porcentaje = (decimal)reader["Porcentaje"],
                Comision = (decimal)reader["Comision"],
            };
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

        private CobrosFacturas MapToReporteCobrosFacturas(SqlDataReader reader)
        {
            return new CobrosFacturas()
            {
                Medios = reader["Medios"].ToString(),
                Valor = (decimal)reader["Valor"],
                NumDocumento = reader["NumDocumento"].ToString(),
                EstadoPago = reader["EstadoPago"].ToString(),
                Conex = reader["Conex"].ToString(),
                ValorBruto = (decimal)reader["ValorBruto"],
                ValorNeto = (decimal)reader["ValorNeto"],
                Porcentaje = (decimal)reader["Porcentaje"],
                Comision = (decimal)reader["Comision"],
                Vendedor = reader["Vendedor"].ToString(),
                EstadoComision = reader["EstadoComision"].ToString(),
                FechaCobroFactura =(DateTime)reader["FechaCobroFactura"],
            };
        }

        private ConsumoPautas MapToReporteConsumoPautas(SqlDataReader reader)
        {
            return new ConsumoPautas()
            {
                NumContrato = reader["NumContrato"].ToString(),
                Anunciante = reader["Anunciante"].ToString(),
                TotalNegocio = (decimal)reader["TotalNegocio"],
                TotalSegundos = (decimal)reader["TotalSegundos"],
                TotalNegocioConsumido = (decimal)reader["TotalNegocioConsumido"],
                TotalSegundosConsumido = (decimal)reader["TotalSegundosConsumido"],
                SaldoTotalNegocio = (decimal)reader["SaldoTotalNegocio"],
                SaldoTotalSegundos = (decimal)reader["SaldoTotalSegundos"],
            };
        }

        public async Task<IEnumerable<ReporteDiario>> GetByMostrarReporteDiario(Int64 IdMedio, int Anio, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteDiario", sql))
                {
                    cmd.CommandTimeout = 60 * 5;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteDiario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteDiario(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ReporteRelacionMedios>> GetByReporteRelacionMedios(Int64 IdMedio, int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ReporteRelacionMedios", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteRelacionMedios>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteRelacionMedio(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<ReporteContacto>> GetByMostrarReporteAnuncianteAgencia( int Tipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteAnuncianteAgencia", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteContacto>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteContacto(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteAnuncianteAgenciaArchivoBase64(int Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("MostrarReporteAnuncianteAgencia", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                        var response = new List<ReporteContacto>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToReporteContacto(reader));
                            }
                        }

                        CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                        archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                        string ruta = archivo.RutaArchivo;
                        //VerErrores("ruta: " + ruta.ToString(), "Log", "Detalle", 1);
                        cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                        string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                        //VerErrores("rutaDocumento: " + rutaDocumento.ToString(), "Log", "Detalle", 1);
                        string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                        //VerErrores("rutaDocumentoResul: " + rutaDocumentoResul.ToString(), "Log", "Detalle", 1);
                        int cont2 = 2;
                        using var wbook = new XLWorkbook(rutaDocumento);
                        var ws = wbook.Worksheet(1);
                        foreach (ReporteContacto s in response)
                        {
                            ws.Cell("A" + cont2.ToString()).Value = s.ANUNCIANTE;
                            ws.Cell("B" + cont2.ToString()).Value = s.CONTACTO_ANUNCIANTE;
                            ws.Cell("C" + cont2.ToString()).Value = s.TELEFONO_CONVENCIONAL;
                            ws.Cell("D" + cont2.ToString()).Value = s.E_MAIL_ANUNCIANTE;
                            ws.Cell("E" + cont2.ToString()).Value = s.AGENCIA;
                            ws.Cell("F" + cont2.ToString()).Value = s.CONTACTO_AGENCIA;
                            ws.Cell("G" + cont2.ToString()).Value = s.TELEFONO_CONVENCIONAL_C;
                            ws.Cell("H" + cont2.ToString()).Value = s.E_MAIL_AGENCIA;
                            cont2++;
                        }
                        //wbook.SaveAs(rutaDocumentoResul);
                        //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                        byte[] archivoBytes = null;
                        using (var msA = new MemoryStream())
                        {
                            wbook.SaveAs(msA);
                            archivoBytes = msA.ToArray();
                        }
                        string archivoBase64 = Convert.ToBase64String(archivoBytes);

                        CargarArchivoBase64 generica = new CargarArchivoBase64();

                        generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                        generica.ArchivoBase64 = archivoBase64;

                        response2.Add(generica);

                    }
                }
            }
            catch(Exception ex)
            {
                VerErrores("error: " + ex.Message.ToString(), "Log", "Detalle", 1);
            }
            return response2;
        }


        #region VerErrores
        public void VerErrores(string valor, string Carpeta, string rucEmpresa, int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    string fecha;
                    fecha = DateTime.Now.ToString("dd-MM-yyyy");//DateTime.Now.ToShortDateString().Replace("/", "-");
                    if (!Directory.Exists(@"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha))
                    {
                        Directory.CreateDirectory(@"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha);
                    }

                    string path = @"C:\\" + rucEmpresa + "\\" + Carpeta + "\\" + fecha + "\\log.txt";
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ": " + valor);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.EventLog.WriteEntry("Application", "Exception: " + ex.Message);
            }
        }
        #endregion

        public async Task<IEnumerable<CargarArchivoBase64>> GetReporteRelacionMediosArchivoBase64(Int64 IdMedio, int Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ReporteRelacionMedios", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteRelacionMedios>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteRelacionMedio(reader));
                        }
                    }


                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    foreach (ReporteRelacionMedios s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.Medios;
                        ws.Cell("B" + cont2.ToString()).Value = s.Canal;
                        ws.Cell("C" + cont2.ToString()).Value = s.Programa;
                        ws.Cell("D" + cont2.ToString()).Value = s.Derecho;
                        ws.Cell("E" + cont2.ToString()).Value = s.Formato;
                        ws.Cell("F" + cont2.ToString()).Value = s.Unidad;
                        ws.Cell("G" + cont2.ToString()).Value = s.Generico;                        
                        cont2++;
                    }
                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteDiarioArchivoBase64(Int64 IdMedio, int Anio, int Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteDiario", sql))
                {
                    cmd.CommandTimeout = 60 * 5;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteDiario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteDiario(reader));
                        }
                    }


                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    foreach (ReporteDiario s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.FECHA;
                        ws.Cell("B" + cont2.ToString()).Value = s.NUMCONTRAT;
                        ws.Cell("C" + cont2.ToString()).Value = s.NUMERORDEN;
                        ws.Cell("D" + cont2.ToString()).Value = s.DESDE;
                        ws.Cell("E" + cont2.ToString()).Value = s.HASTA;
                        ws.Cell("F" + cont2.ToString()).Value = s.ANUNCIANTE;
                        ws.Cell("G" + cont2.ToString()).Value = s.MEDIOPUBLI;
                        ws.Cell("H" + cont2.ToString()).Value = s.AGENCIA;
                        ws.Cell("I" + cont2.ToString()).Value = s.RUCVENDOR;
                        ws.Cell("J" + cont2.ToString()).Value = s.CANTSPOTS;
                        ws.Cell("K" + cont2.ToString()).Value = s.CANAL;
                        ws.Cell("L" + cont2.ToString()).Value = s.PROGRAMA;
                        ws.Cell("M" + cont2.ToString()).Value = s.DERECHO;
                        ws.Cell("N" + cont2.ToString()).Value = s.FRANJA;
                        ws.Cell("O" + cont2.ToString()).Value = s.PARCIAL;
                        ws.Cell("P" + cont2.ToString()).Value = s.VALORBRUTO;
                        if (s.COMISAGEN == "")
                        {
                            ws.Cell("Q" + cont2.ToString()).Value = s.COMISAGEN;
                        }
                        else
                        {
                            ws.Cell("Q" + cont2.ToString()).Value = Convert.ToDecimal(s.COMISAGEN) / 100;
                        }
                        ws.Cell("R" + cont2.ToString()).Value = s.VALORAGEN;
                        ws.Cell("S" + cont2.ToString()).Value = s.VALOR;
                        if (s.COMISCONEX == "")
                        {
                            ws.Cell("T" + cont2.ToString()).Value = s.COMISCONEX;
                        }
                        else
                        {
                            ws.Cell("T" + cont2.ToString()).Value = Convert.ToDecimal(s.COMISCONEX) / 100;
                        }
                        ws.Cell("U" + cont2.ToString()).Value = s.VALORCONEX;
                        ws.Cell("V" + cont2.ToString()).Value = s.FACTURADOPORMEDIO;
                        ws.Cell("W" + cont2.ToString()).Value = s.CERTIFICADOPORMEDIO;
                        ws.Cell("X" + cont2.ToString()).Value = s.PAGADOALMEDIO;
                        ws.Cell("Y" + cont2.ToString()).Value = s.FACTXCONEX;
                        ws.Cell("Z" + cont2.ToString()).Value = s.XFACTCONEX;
                        ws.Cell("AA" + cont2.ToString()).Value = s.SEGUNDAJE_DERECHO;
                        ws.Cell("AB" + cont2.ToString()).Value = s.TOTAL_SEGUNDOS;
                        cont2++;
                    }
                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }


        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarReporteSegundoArchivoBase64(Int64 IdMedio, int Anio, int Tipo, string TipoDocumento)
        {
            var response2 = new List<CargarArchivoBase64>();
            string fecha;
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarReporteDiario", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdMedio", IdMedio));
                    cmd.Parameters.Add(new SqlParameter("@Anio", Anio));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Tipo));
                    var response = new List<ReporteDiario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteDiario(reader));
                        }
                    }
                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                    string ruta = archivo.RutaArchivo;
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    int cont2 = 2;
                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    foreach (ReporteDiario s in response)
                    {
                        ws.Cell("A" + cont2.ToString()).Value = s.FECHA;
                        ws.Cell("B" + cont2.ToString()).Value = s.NUMCONTRAT;
                        ws.Cell("C" + cont2.ToString()).Value = s.NUMERORDEN;
                        ws.Cell("D" + cont2.ToString()).Value = s.DESDE;
                        ws.Cell("E" + cont2.ToString()).Value = s.HASTA;
                        ws.Cell("F" + cont2.ToString()).Value = s.ANUNCIANTE;
                        ws.Cell("G" + cont2.ToString()).Value = s.MEDIOPUBLI;
                        ws.Cell("H" + cont2.ToString()).Value = s.AGENCIA;
                        ws.Cell("I" + cont2.ToString()).Value = s.RUCVENDOR;
                        ws.Cell("J" + cont2.ToString()).Value = s.CANTSPOTS;
                        ws.Cell("K" + cont2.ToString()).Value = s.CANAL;
                        ws.Cell("L" + cont2.ToString()).Value = s.PROGRAMA;
                        ws.Cell("M" + cont2.ToString()).Value = s.DERECHO;
                        ws.Cell("N" + cont2.ToString()).Value = s.FRANJA;                       
                        ws.Cell("O" + cont2.ToString()).Value = s.SEGUNDAJE_DERECHO;
                        ws.Cell("P" + cont2.ToString()).Value = s.TOTAL_SEGUNDOS;
                        cont2++;
                    }
                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    CargarArchivoBase64 generica = new CargarArchivoBase64();

                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = archivoBase64;

                    response2.Add(generica);

                    return response2;
                }
            }
        }

        public async Task<IEnumerable<RolPagos>> GetByMostrarArchivoRolPago(string RuCedula,DateTime FechaPago)
        {
            string fecha;
            //DataTable dt = new DataTable();
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            var response2 = new List<CargarArchivoBase64>();
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MostrarArchivoRolPago", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@RuCedula", RuCedula));
                    cmd.Parameters.Add(new SqlParameter("@FechaPago", FechaPago));
                    var response = new List<RolPagos>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReporteRolPagos(reader));
                        }
                    }

                    CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                    archivo = cargar1.MostrarCargaArhivoConfig(0, 0, "ROL DE PAGO",0);
                    string ruta = archivo.RutaArchivo;
                    var response3 = new List<RolPagos>();
                    cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                    string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                    string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;

                    using var wbook = new XLWorkbook(rutaDocumento);
                    var ws = wbook.Worksheet(1);
                    string[] datosDoc;
                    foreach (RolPagos s in response)
                    {
                        datosDoc = s.CadenaValores.Split(';');

                        ws.Cell("D14").Value = s.NombresApellidos;
                        ws.Cell("D14").Style.Font.Bold = true;

                        ws.Cell("D21").Value = datosDoc[0].ToString();
                        ws.Cell("D21").Style.Font.Bold = true;

                        ws.Cell("D22").Value = datosDoc[1].ToString();
                        ws.Cell("D22").Style.Font.Bold = true;

                        ws.Cell("E24").Value = Convert.ToDouble(datosDoc[2].ToString()) * -1;
                        ws.Cell("E24").Style.Font.Bold = true;

                        ws.Cell("D25").Value = Convert.ToDouble(datosDoc[3].ToString());
                        ws.Cell("D25").Style.Font.Bold = true;

                        ws.Cell("D26").Value = Convert.ToDouble(datosDoc[4].ToString());
                        ws.Cell("D26").Style.Font.Bold = true;

                        ws.Cell("D28").Value = Convert.ToDouble(datosDoc[5].ToString());
                        ws.Cell("D28").Style.Font.Bold = true;

                        ws.Cell("D29").Value = Convert.ToDouble(datosDoc[6].ToString());
                        ws.Cell("D29").Style.Font.Bold = true;

                        ws.Cell("D30").Value = Convert.ToDouble(datosDoc[7].ToString());
                        ws.Cell("D30").Style.Font.Bold = true;

                        ws.Cell("D15").Value = s.RuCedula;
                        ws.Cell("D15").Style.Font.Bold = true;

                        ws.Cell("D41").Value = "CI: " + s.RuCedula;
                        ws.Cell("D41").Style.Font.Bold = true;

                        ws.Cell("D16").Value = s.FechaPago;
                        ws.Cell("D16").Style.Font.Bold = true;

                        ws.Cell("D35").Value = s.Banco ;
                        ws.Cell("D35").Style.Font.Bold = true;
                    }

                    //wbook.SaveAs(rutaDocumentoResul);
                    //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                    byte[] archivoBytes = null;
                    using (var msA = new MemoryStream())
                    {
                        wbook.SaveAs(msA);
                        archivoBytes = msA.ToArray();
                    }
                    string archivoBase64 = Convert.ToBase64String(archivoBytes);

                    RolPagos generica = new RolPagos();

                    generica.ArchivoBase64 = archivoBase64;

                    response3.Add(generica);

                    return response3;
                }
            }
        }
        public async Task<IEnumerable<Generica>> CargaRolPagos(RolPagos rolPagos)
        {
            var response2 = new List<Generica>();
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertarModificarEliminarRolDePagos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPagoRol", rolPagos.IdPagoRol));
                    cmd.Parameters.Add(new SqlParameter("@RuCedula", rolPagos.RuCedula));
                    cmd.Parameters.Add(new SqlParameter("@NombresApellidos", rolPagos.NombresApellidos));
                    cmd.Parameters.Add(new SqlParameter("@CadenaValores", rolPagos.CadenaValores));
                    cmd.Parameters.Add(new SqlParameter("@FechaPago", rolPagos.FechaPago));
                    cmd.Parameters.Add(new SqlParameter("@ArchivoBase64", rolPagos.ArchivoBase64));
                    cmd.Parameters.Add(new SqlParameter("@Estado", rolPagos.Estado));
                    cmd.Parameters.Add(new SqlParameter("@Tipo", rolPagos.Tipo));
                    await sql.OpenAsync();
                    //await cmd.ExecuteNonQueryAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response2.Add(MapToGenerica(reader));
                        }
                    }
                }
            }

            return response2;
        }
        public async Task<IEnumerable<CargarArchivoBase64>> GetByMostrarRolPagoArchivoBase64(Int64 IdContrato, Int64 IdForeCast, string TipoDocumento,string JsonRol,string NombreEMpleado)
        {
            string fecha;
            //DataTable dt = new DataTable();
            fecha = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "_");
            var response = new List<CargarArchivoBase64>();
            CargarArchivo cargar = new CargarArchivo();
            PrmConfiguracionArchivo archivo = new PrmConfiguracionArchivo();
            try
            {
                CargarXLSX cargar1 = new CargarXLSX(_connectionString);
                archivo = cargar1.MostrarCargaArhivoConfig(0, 0, TipoDocumento,0);
                string ruta = archivo.RutaArchivo;
                var response2 = new List<Generica>();
                cargar.RutaArchivo = ruta + cargar.nombreArchivo;
                string rutaDocumento = archivo.RutaArchivo + archivo.NombreArchivo + archivo.Extencion;
                string rutaDocumentoResul = archivo.RutaArchivo + archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;

                using var wbook = new XLWorkbook(rutaDocumento);
                var ws = wbook.Worksheet(1);
                string[] datosDoc = JsonRol.Split(';');
                string[] datosNom = NombreEMpleado.Split(';');

                //ws.Cell("C5").Value = datosNom[0];
                //ws.Cell("C5").Style.Font.Bold = true;

                //ws.Cell("D8").Value = datosDoc[0].ToString();
                //ws.Cell("D8").Style.Font.Bold = true;

                //ws.Cell("D9").Value = datosDoc[1].ToString();
                //ws.Cell("D9").Style.Font.Bold = true;

                //ws.Cell("E10").Value = Convert.ToDouble(datosDoc[2].ToString()) * -1;
                //ws.Cell("E10").Style.Font.Bold = true;

                //ws.Cell("E11").Value = Convert.ToDouble(datosDoc[3].ToString());
                //ws.Cell("E11").Style.Font.Bold = true;

                //ws.Cell("E12").Value = Convert.ToDouble(datosDoc[4].ToString());
                //ws.Cell("E12").Style.Font.Bold = true;

                //ws.Cell("D14").Value = Convert.ToDouble(datosDoc[5].ToString()) * -1;
                //ws.Cell("D14").Style.Font.Bold = true;

                //ws.Cell("D15").Value = Convert.ToDouble(datosDoc[6].ToString()) * -1;
                //ws.Cell("D15").Style.Font.Bold = true;

                //ws.Cell("D16").Value = Convert.ToDouble(datosDoc[7].ToString()) * -1; ;
                //ws.Cell("D16").Style.Font.Bold = true;

                //ws.Cell("E5").Value = datosNom[2];
                //ws.Cell("E5").Style.Font.Bold = true;

                ws.Cell("D14").Value = datosNom[0];
                ws.Cell("D14").Style.Font.Bold = true;

                ws.Cell("D21").Value = datosDoc[0].ToString();
                ws.Cell("D21").Style.Font.Bold = true;

                ws.Cell("D22").Value = datosDoc[1].ToString();
                ws.Cell("D22").Style.Font.Bold = true;

                ws.Cell("E24").Value = Convert.ToDouble(datosDoc[2].ToString()) * -1;
                ws.Cell("E24").Style.Font.Bold = true;

                ws.Cell("D25").Value = Convert.ToDouble(datosDoc[3].ToString());
                ws.Cell("D25").Style.Font.Bold = true;

                ws.Cell("D26").Value = Convert.ToDouble(datosDoc[4].ToString());
                ws.Cell("D26").Style.Font.Bold = true;

                ws.Cell("D28").Value = Convert.ToDouble(datosDoc[5].ToString());
                ws.Cell("D28").Style.Font.Bold = true;

                ws.Cell("D29").Value = Convert.ToDouble(datosDoc[6].ToString());
                ws.Cell("D29").Style.Font.Bold = true;

                ws.Cell("D30").Value = Convert.ToDouble(datosDoc[7].ToString());
                ws.Cell("D30").Style.Font.Bold = true;

                ws.Cell("D15").Value = datosNom[1];
                ws.Cell("D15").Style.Font.Bold = true;


                ws.Cell("D41").Value = "CI: " + datosNom[1];
                ws.Cell("D41").Style.Font.Bold = true;

                ws.Cell("D16").Value = datosNom[2];
                ws.Cell("D16").Style.Font.Bold = true;

                //wbook.SaveAs(rutaDocumentoResul);
                //byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaDocumentoResul);
                byte[] archivoBytes = null;
                using (var msA = new MemoryStream())
                {
                    wbook.SaveAs(msA);
                    archivoBytes = msA.ToArray();
                }
                string archivoBase64 = Convert.ToBase64String(archivoBytes);

                RolPagos pagos = new RolPagos();
                pagos.RuCedula = datosNom[1];
                pagos.NombresApellidos = datosNom[0];
                pagos.CadenaValores = JsonRol;
                pagos.FechaPago = Convert.ToDateTime(datosNom[2]);
                pagos.Estado = 1;
                pagos.Tipo = 1;
                pagos.ArchivoBase64 = archivoBase64;
                var response3 = await CargaRolPagos(pagos);

                var response4 = await GetByMostrarArchivoRolPago(datosNom[1], Convert.ToDateTime(datosNom[2]));
                CargarArchivoBase64 generica = new CargarArchivoBase64();
                foreach (RolPagos s in response4)
                {
                    generica.NombreArchivo = archivo.NombreArchivoSalida + "_" + fecha + archivo.Extencion;
                    generica.ArchivoBase64 = s.ArchivoBase64;
                }

                response.Add(generica);

            }
            catch(Exception ex)
            {

            }
            
            return response;
        }
        private ReporteDiario MapToReporteDiario(SqlDataReader reader)
        {
            //try
            //{
                return new ReporteDiario()
                {
                    FECHA = reader["FECHA"].ToString(),
                    NUMCONTRAT = reader["NUMCONTRAT"].ToString(),
                    NUMERORDEN = reader["NUMERORDEN"].ToString(),
                    DESDE = reader["DESDE"].ToString(),
                    HASTA = reader["HASTA"].ToString(),
                    ANUNCIANTE = reader["ANUNCIANTE"].ToString(),
                    MEDIOPUBLI = reader["MEDIOPUBLI"].ToString(),
                    AGENCIA = reader["AGENCIA"].ToString(),
                    RUCVENDOR = reader["RUCVENDOR"].ToString(),
                    CANTSPOTS = reader["CANTSPOTS"].ToString(),
                    CANAL = reader["CANAL"].ToString(),
                    PROGRAMA = reader["PROGRAMA"].ToString(),
                    DERECHO = reader["DERECHO"].ToString(),
                    FRANJA = reader["FRANJA"].ToString(),
                    PARCIAL = reader["PARCIAL"].ToString(),
                    VALORBRUTO = reader["VALORBRUTO"].ToString(),
                    COMISAGEN = reader["COMISAGEN"].ToString(),
                    VALORAGEN = reader["VALORAGEN"].ToString(),
                    VALOR = reader["VALOR"].ToString(),
                    COMISCONEX = reader["COMISCONEX"].ToString(),
                    VALORCONEX = reader["VALORCONEX"].ToString(),
                    FACTURADOPORMEDIO = reader["FACTURADOPORMEDIO"].ToString(),
                    CERTIFICADOPORMEDIO = reader["CERTIFICADOPORMEDIO"].ToString(),
                    PAGADOALMEDIO = reader["PAGADOALMEDIO"].ToString(),
                    FACTXCONEX = reader["FACTXCONEX"].ToString(),
                    XFACTCONEX = reader["XFACTCONEX"].ToString(),
                    SEGUNDAJE_DERECHO = reader["SEGUNDAJE_DERECHO"].ToString(),
                    TOTAL_SEGUNDOS = reader["TOTAL_SEGUNDOS"].ToString(),
                };
            //}
            //catch(Exception ex)
            //{

            //}
        }

        private ReporteRelacionMedios MapToReporteRelacionMedio(SqlDataReader reader)
        {
            return new ReporteRelacionMedios()
            {
                IdRelacion = (Int64)reader["IdRelacion"],
                Medios = reader["MEDIO"].ToString(),
                Canal = reader["CANAL"].ToString(),
                Programa = reader["PROGRAMA"].ToString(),
                Derecho = reader["DERECHO"].ToString(),
                Formato = reader["FORMATO"].ToString(),
                Unidad = reader["UNIDAD"].ToString(),
                Generico = reader["GENERICO"].ToString(),
            };
        }

        private ReporteContacto MapToReporteContacto(SqlDataReader reader)
        {
            return new ReporteContacto()
            {
                ANUNCIANTE = reader["ANUNCIANTE"].ToString(),
                CONTACTO_ANUNCIANTE = reader["CONTACTO_ANUNCIANTE"].ToString(),
                TELEFONO_CONVENCIONAL = reader["TELEFONO_CONVENCIONAL"].ToString(),
                E_MAIL_ANUNCIANTE = reader["E_MAIL_ANUNCIANTE"].ToString(),
                AGENCIA = reader["AGENCIA"].ToString(),
                CONTACTO_AGENCIA = reader["CONTACTO_AGENCIA"].ToString(),
                TELEFONO_CONVENCIONAL_C = reader["TELEFONO_CONVENCIONAL_C"].ToString(),
                E_MAIL_AGENCIA = reader["E_MAIL_AGENCIA"].ToString(),
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

        private RolPagos MapToReporteRolPagos(SqlDataReader reader)
        {
            return new RolPagos()
            {
                IdPagoRol = (Int64)reader["IdPagoRol"],
                RuCedula = reader["RuCedula"].ToString(),
                NombresApellidos = reader["NombresApellidos"].ToString(),
                CadenaValores = reader["CadenaValores"].ToString(),
                FechaPago =(DateTime)reader["FechaPago"],
                ArchivoBase64 = reader["ArchivoBase64"].ToString(),
                Banco = reader["Banco"].ToString(),
            };
        }

        private SeguimientoForeCast MapToReporteSeguimiento(SqlDataReader reader)
        {
            return new SeguimientoForeCast()
            {
                IdForeCast = (Int64)reader["IdForeCast"],
                NombresApellidos = reader["NombresApellidos"].ToString(),
                FechaSeguimiento = reader["FechaSeguimiento"].ToString(),
                Seguimientollamada = reader["Seguimientollamada"].ToString(),
                SeguimientoVisita = reader["SeguimientoVisita"].ToString(),
                Cliente = reader["Cliente"].ToString(),
                Agencia = reader["Agencia"].ToString(),
                Negocio = reader["Negocio"].ToString(),
                ValorTotalBruto = (decimal)reader["ValorTotalBruto"],
            };
        }
    }
}
