-- ============================================================
-- PASO 1: Cambiar contraseña de SA (expuesta en git history)
-- Ejecutar conectado como sysadmin
-- ============================================================
ALTER LOGIN [sa] WITH PASSWORD = 'g1GMD5ijLfI9tzoKxjDYOpP2';
GO

-- ============================================================
-- PASO 2: Crear login y usuario de aplicación (least privilege)
-- ============================================================
USE [master];
GO
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'conexion_app')
BEGIN
    CREATE LOGIN [conexion_app] WITH PASSWORD = 'MDs9bJlnnBa1dwbccmso',
        DEFAULT_DATABASE = [CONEXIONDB],
        CHECK_EXPIRATION = ON,
        CHECK_POLICY = ON;
END
GO

USE [CONEXIONDB];
GO
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'conexion_app')
BEGIN
    CREATE USER [conexion_app] FOR LOGIN [conexion_app];
END
GO

-- ============================================================
-- PASO 3: Dar EXECUTE en todos los stored procedures
-- ============================================================
GRANT EXECUTE ON [dbo].[ActualizarCorreo]                                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[ActualizarDetalleContrato]                           TO [conexion_app];
GRANT EXECUTE ON [dbo].[BusquedaCuentasPorPagar]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[Consulta_ats]                                        TO [conexion_app];
GRANT EXECUTE ON [dbo].[Consulta_ImpuestoIva]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[ConsultarDocumentosXClienteFechas]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[EnviarMailUsuarios]                                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[GenerarPDF]                                          TO [conexion_app];
GRANT EXECUTE ON [dbo].[GenerarXMLComprobanteElectronicoManual]              TO [conexion_app];
GRANT EXECUTE ON [dbo].[GuardarProcesoDocumento]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[guardarprocesodocumentorecepcion]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarAsientoContable]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarAgencia]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarAprobacionSolicitud]        TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarAsientosContable]           TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCanal]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCargarArchivoSpots]         TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCertificadoDigital]         TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCierrePlanCuentas]          TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCliente]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarClienteAgencia]             TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarClienteServicio]            TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCobrosAsientosContable]     TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarComision]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarConex]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarContrato]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarCuentaPorPagar]             TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDerecho]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleContrato]            TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleContratoPro]         TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleContratoTable]       TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleForecast]            TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleForecastCarga]       TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleForecastTable]       TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarDetalleForecastTablePro]    TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarEmpleado]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarFacturaServicio]            TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarForeCast]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarForeCast_copia]             TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarFormato]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarLiquidacion]                TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarMarca]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarMedios]                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarNotaDeCredito]              TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPagoContrato]               TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPDF]                        TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPermisoMenu]                TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPlanCuentas]                TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPlantilla]                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPresupuestoMedios]          TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPrmMenuEmpleado]            TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPrmPerfil]                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarPrograma]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarProveedor]                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarRegitroArchivo]             TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarRelacionMedio]              TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarRelacionMedios]             TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarRelacionMediosMasivo]       TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarRolDePagos]                 TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarSaldoVacaciones]            TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarTotalNegSegun]              TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarUnidad]                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarEliminarVacaciones]                 TO [conexion_app];
GRANT EXECUTE ON [dbo].[InsertarModificarFacturasCobradas]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[ModificarAsientoContable]                            TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarArchivoBase64]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarArchivoRolPago]                               TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarCargaArhivo]                                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarCargaArhivoConfig]                            TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarClienteAgencia]                               TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarClienteServicio]                              TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarCobros]                                       TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComboContrato]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComboForeCast]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComision]                                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComisionVendedor]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComisionVendedorMultiples]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComisionVendedorMultiples_copia]              TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarComprobanteElectronicoTODO]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarContrato]                                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarContratoExterior]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarContratoPorFacturar]                          TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarDescripcionCombo]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarDetalleForeCast]                              TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarDiasVacacionesGenerados]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarDocumentosProcesados]                         TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarEmpleado]                                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarEmpresa]                                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarEmpresas]                                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarFacturaNotaCredito]                           TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarFacturaPorCobrar]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarFacturaPorCobrarFecha]                        TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarFacturaPorPagarFecha]                         TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarFacturaPorPagarFecha_Copia]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarForeCast]                                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarImpuestos]                                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarMedio]                                        TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarMedioRelacionado]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarMenuDinamico]                                 TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarMenuDinamicoEmpleado]                         TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPagos]                                        TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPermisoMenu]                                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPlanCuentas]                                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPlanCuentasSaldoFinal]                        TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPlanCuentasSaldoFinalCero]                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPlantilla]                                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPlantillaCuentaPorPagar]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPresupuestoMedios]                            TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPrmEmpresa]                                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPrmEstadoSolicitud]                           TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPrmPerfil]                                    TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPrmRolDePagos]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarPrmTipoSolicitud]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarProveedor]                                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarRegistroContable]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarRegistroContableMensual]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteAnuncianteAgencia]                     TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteBalanceComprobacion]                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteComision]                              TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteComisionVendedor]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteConsumo]                               TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteDiario]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteDiario_Prueba]                         TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteEstadoCuenta]                          TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteEstadoCuentaLibroMayor]                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteEstadoCuentaLibroMayorConciliacion]    TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteEstadoFinanciero]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteEstadoResultados]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteFacturasCobradas]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteFacturasCobradas_Copia]                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReporteGeneral]                               TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarReportePresupuesto]                           TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarSecuencialDoc]                                TO [conexion_app];
GRANT EXECUTE ON [dbo].[MostrarVacaciones]                                   TO [conexion_app];
GRANT EXECUTE ON [dbo].[NotificacionOportunidad]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[ReporteDetallePagarCobrar]                           TO [conexion_app];
GRANT EXECUTE ON [dbo].[ReporteDetallePagarCobrar_Original]                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[ReportePendienteEnviado]                             TO [conexion_app];
GRANT EXECUTE ON [dbo].[ReporteRelacionMedios]                               TO [conexion_app];
GRANT EXECUTE ON [dbo].[ReporteSeguimiento]                                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[ValidarRelacionDeMapaPauta]                          TO [conexion_app];
GRANT EXECUTE ON [dbo].[ValidarRelacionesMedio]                              TO [conexion_app];
GRANT EXECUTE ON [dbo].[WebActualizarEmpleado]                               TO [conexion_app];
GRANT EXECUTE ON [dbo].[WebMostrarEmpleado]                                  TO [conexion_app];
GRANT EXECUTE ON [dbo].[WebMostrarMenuDinamicoEmpleado]                      TO [conexion_app];
GRANT EXECUTE ON [dbo].[WebMostrarPrmPerfil]                                 TO [conexion_app];
GRANT EXECUTE ON [dbo].[WebVerUsuario]                                       TO [conexion_app];
GO

-- ============================================================
-- PASO 4: Verificar que el usuario quedó bien configurado
-- ============================================================
SELECT dp.name AS usuario, p.permission_name, p.state_desc, o.name AS objeto
FROM sys.database_permissions p
JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
JOIN sys.objects o ON p.major_id = o.object_id
WHERE dp.name = 'conexion_app'
ORDER BY o.name;
GO
