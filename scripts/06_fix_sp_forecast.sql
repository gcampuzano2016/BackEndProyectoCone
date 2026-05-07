ALTER PROCEDURE [dbo].[InsertarModificarEliminarForeCast]
@IdForeCast  bigint=null,
@IdCliente bigint=null,
@IdMarca   bigint=null,
@IdMedio   bigint=null,
@IdAgencia bigint=null,
@IdCanal   varchar(50)='',
@IdPrograma varchar(50)='',
@IdDerecho varchar(50)='',
@IdUnidad varchar(50)='',
@IdNegocio bigint=null,
@IdPropuesta bigint=null,
@IdEmpleado bigint=null,
@IdTipoRechazo  bigint=null,
@IdContacto  bigint=null,
@Agencia varchar(50)='',
@NombreProyecto varchar(350)='',
@Contacto varchar(150)='',
@FechaIngreso datetime=null,
@Cantidad int=0,
@Monto decimal(18,2)=0,
@ValorTotalBruto decimal(18,2)=0,
@PorcentajeAgencia decimal(18,2)=0,
@ValorAgencia decimal(18,2)=0,
@ValorTotalNeto decimal(18,2)=0,
@FechaInicioPauta datetime=null,
@FechaFinalPauta datetime=null,
@FechaTope datetime=0,
@FechaCierre datetime=null,
@Seguimientollamada varchar(250)='',
@FechaSeguimiento datetime=null,
@SeguimientoVisita varchar(250)='',
@FechaVisita datetime=null,
@Usuario varchar(50)='',
@MotivoRechazo  varchar(150)='',
@UltimaModificacion datetime=null,
@TotalNegocio as decimal(18,2)=0,
@TotalSegundos as decimal(18,2)=0,
@Estado as int=null,
@Tipo as int=0
AS
BEGIN
SET XACT_ABORT ON;
BEGIN TRANSACTION

        DECLARE @ESTADO_EJECUCION SMALLINT = -1;
        DECLARE @MENSAJES_DIRECCION VARCHAR(500);
        DECLARE @RESPUESTA VARCHAR(150)='';
        DECLARE @ErrorMsg VARCHAR(500) = '';

        IF(@MotivoRechazo ='')
            BEGIN
                SELECT @MotivoRechazo=Descripcion FROM [dbo].[TipoRechazo] WHERE [IdTipoRechazo]=@IdTipoRechazo;
            END

        IF(@Tipo=1)
            BEGIN
                BEGIN TRY
                    INSERT INTO [dbo].[ForeCast]
                               ([IdCliente],[IdMarca],[IdMedio],[IdAgencia],[IdCanal],[IdPrograma],[IdDerecho],[IdUnidad],
                                [IdNegocio],[IdPropuesta],[IdEmpleado],[IdTipoRechazo],[IdContacto],[Agencia],[NombreProyecto],
                                [Contacto],[FechaIngreso],[Cantidad],[Monto],[ValorTotalBruto],[PorcentajeAgencia],[ValorAgencia],
                                [ValorTotalNeto],[FechaInicioPauta],[FechaFinalPauta],[FechaTope],[FechaCierre],[Seguimientollamada],
                                [FechaSeguimiento],[SeguimientoVisita],[FechaVisita],[Usuario],[MotivoRechazo],[UltimaModificacion],
                                [FechaRegistro],[Estado],[TotalNegocio],[TotalSegundos])
                    VALUES
                               (@IdCliente,@IdMarca,@IdMedio,@IdAgencia,@IdCanal,@IdPrograma,@IdDerecho,@IdUnidad,
                                @IdNegocio,@IdPropuesta,@IdEmpleado,@IdTipoRechazo,@IdContacto,@Agencia,@NombreProyecto,
                                @Contacto,@FechaIngreso,@Cantidad,@Monto,@ValorTotalBruto,@PorcentajeAgencia,@ValorAgencia,
                                @ValorTotalNeto,@FechaInicioPauta,@FechaFinalPauta,@FechaTope,@FechaCierre,@Seguimientollamada,
                                @FechaSeguimiento,@SeguimientoVisita,@FechaVisita,@Usuario,@MotivoRechazo,@UltimaModificacion,
                                GETDATE(),1,@TotalNegocio,@TotalSegundos);
                    SELECT @ESTADO_EJECUCION = 1;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END
        ELSE IF(@Tipo=2)
            BEGIN
                BEGIN TRY
                    DECLARE @IdNegocioAnterior bigint=null;
                    SELECT @IdNegocioAnterior = IdNegocio FROM ForeCast WHERE IdForeCast = @IdForeCast;

                    INSERT INTO [dbo].[ForeCastHistorico]([IdForeCast],[IdCliente],[IdMarca],[IdMedio],[IdAgencia],[IdCanal],[IdPrograma],[IdDerecho],[IdUnidad],[IdNegocio],
                            [IdPropuesta],[IdEmpleado],[IdTipoRechazo],[IdContacto],[Agencia],[NombreProyecto],[Contacto],[FechaIngreso],[Cantidad],[Monto],[ValorTotalBruto],
                            [PorcentajeAgencia],[ValorAgencia],[ValorTotalNeto],[FechaInicioPauta],[FechaFinalPauta],[FechaTope],[FechaCierre],
                            [Seguimientollamada],[FechaSeguimiento],[SeguimientoVisita],[FechaVisita],[Usuario],[MotivoRechazo],[UltimaModificacion],[FechaRegistro],[Estado])
                    SELECT [IdForeCast],[IdCliente],[IdMarca],[IdMedio],[IdAgencia],[IdCanal],[IdPrograma],[IdDerecho],[IdUnidad],[IdNegocio],[IdPropuesta],[IdEmpleado],
                           [IdTipoRechazo],[IdContacto],[Agencia],[NombreProyecto],[Contacto],[FechaIngreso],[Cantidad],[Monto],[ValorTotalBruto],[PorcentajeAgencia],[ValorAgencia],
                           [ValorTotalNeto],[FechaInicioPauta],[FechaFinalPauta],[FechaTope],[FechaCierre],[Seguimientollamada],[FechaSeguimiento],
                           [SeguimientoVisita],[FechaVisita],[Usuario],[MotivoRechazo],[UltimaModificacion],[FechaRegistro],[Estado]
                    FROM [dbo].[ForeCast] WHERE IdForeCast=@IdForeCast;

                    UPDATE [dbo].[ForeCast]
                       SET [Agencia]=@Agencia,[IdCliente]=@IdCliente,[IdMarca]=@IdMarca,[IdMedio]=@IdMedio,[IdAgencia]=@IdAgencia,
                           [IdCanal]=@IdCanal,[IdPrograma]=@IdPrograma,[IdDerecho]=@IdDerecho,[IdUnidad]=@IdUnidad,[IdNegocio]=@IdNegocio,
                           [IdPropuesta]=@IdPropuesta,[IdEmpleado]=@IdEmpleado,[IdTipoRechazo]=@IdTipoRechazo,[IdContacto]=@IdContacto,
                           [NombreProyecto]=@NombreProyecto,[Contacto]=@Contacto,[FechaIngreso]=@FechaIngreso,[Cantidad]=@Cantidad,[Monto]=@Monto,
                           [ValorTotalBruto]=@ValorTotalBruto,[PorcentajeAgencia]=@PorcentajeAgencia,[ValorAgencia]=@ValorAgencia,
                           [ValorTotalNeto]=@ValorTotalNeto,[FechaInicioPauta]=@FechaInicioPauta,[FechaFinalPauta]=@FechaFinalPauta,
                           [FechaTope]=@FechaTope,[FechaCierre]=@FechaCierre,[Seguimientollamada]=@Seguimientollamada,
                           [FechaSeguimiento]=@FechaSeguimiento,[SeguimientoVisita]=@SeguimientoVisita,[FechaVisita]=@FechaVisita,
                           MotivoRechazo=@MotivoRechazo,[Usuario]=@Usuario,[UltimaModificacion]=@UltimaModificacion,
                           [FechaRegistro]=GETDATE(),[Estado]=@Estado,[TotalNegocio]=@TotalNegocio,[TotalSegundos]=@TotalSegundos
                    WHERE IdForeCast=@IdForeCast;

                    IF(EXISTS(SELECT * FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1))
                        BEGIN
                            IF(NOT EXISTS(SELECT * FROM Contrato WHERE IdForeCast=@IdForeCast))
                                BEGIN
                                    DECLARE @REVISTA VARCHAR(250)='';
                                    DECLARE @PROPUESTA VARCHAR(150)='';
                                    DECLARE @IdProNegocio BIGINT=0;
                                    SELECT @PROPUESTA=Descripcion FROM PrmEstadoPropuesta
                                    WHERE IdPropuesta IN(SELECT IdPropuesta FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                    SELECT @IdProNegocio=IdNegocio FROM PrmNegocio WHERE IdNegocio=@IdNegocio;

                                    IF(@PROPUESTA='ACEPTADA' AND @IdProNegocio=5)
                                        BEGIN
                                            SET @RESPUESTA=' Y GENERADO EL CONTRATO.';
                                            DECLARE @VALOR INT=0;
                                            DECLARE @INICIO INT=1;
                                            DECLARE @FINAL INT=0;
                                            DECLARE @FECHAINCIAL DATETIME=NULL;
                                            DECLARE @FECHAFINAL DATETIME=NULL;
                                            DECLARE @FECHAPROCESO DATETIME=NULL;

                                            SELECT @VALOR=DATEDIFF(MONTH,FechaInicioPauta,FechaFinalPauta)
                                            FROM ForeCast WHERE IdForeCast=@IdForeCast;
                                            SET @VALOR=@VALOR+1;

                                            DECLARE @Porcentajeconex DECIMAL(18,2)=0;
                                            DECLARE @IDCONTA BIGINT=0;
                                            DECLARE @SECUENCIAL VARCHAR(10)='';
                                            DECLARE @FECHA1 DATETIME=NULL;
                                            DECLARE @FECHA2 DATETIME=NULL;
                                            DECLARE @RUCVENDEDOR VARCHAR(13)='';
                                            DECLARE @ANUNCIANTE VARCHAR(150)='';
                                            DECLARE @CONTACTO1 VARCHAR(150)='';
                                            DECLARE @MEDIO VARCHAR(150)='';
                                            DECLARE @ULTIMO INT=0;
                                            DECLARE @LETRA VARCHAR(1)='';

                                            IF(@VALOR=1)
                                                BEGIN
                                                    SET @SECUENCIAL='';
                                                    SET @FECHA1=NULL;
                                                    SET @FECHA2=NULL;
                                                    SET @RUCVENDEDOR='';
                                                    SET @ANUNCIANTE='';
                                                    SET @CONTACTO1='';
                                                    SET @MEDIO='';
                                                    SET @ULTIMO=0;
                                                    SET @Porcentajeconex=0;
                                                    SELECT @RUCVENDEDOR=Rucedula FROM Empleado WHERE IdEmpleado IN(SELECT IdEmpleado FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                    SELECT @ANUNCIANTE=Descripcion FROM Cliente WHERE IdCliente IN(SELECT IdCliente FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                    SELECT @CONTACTO1=Contacto FROM ClienteAgencia WHERE IdClienteAgencia IN(SELECT IdContacto FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                    SELECT @MEDIO=Medios,@Porcentajeconex=ComisionCone FROM Medios WHERE IdMedio IN(SELECT IdMedio FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                    SELECT @SECUENCIAL=SecuencialLetra+RIGHT('0000'+Ltrim(Rtrim(Secuencial)),Incremento) FROM [dbo].[PrmSecuencialConex];
                                                    UPDATE PrmSecuencialConex SET Secuencial=Secuencial+1;

                                                    SET @FECHAFINAL=NULL;
                                                    SELECT @FECHAFINAL=DATEADD(dd,-(DAY(DATEADD(mm,1,FechaFinalPauta))),DATEADD(mm,1,FechaFinalPauta))
                                                    FROM ForeCast WHERE IdForeCast=@IdForeCast;

                                                    INSERT INTO [dbo].[Contrato]([IdForeCast],[IdOrdenRecibida],[IdOrdenEnviada],[IdMaterialR],
                                                        [IdMaterialE],[IdFacturado],[IdCertificado],[IdPagado],[FechaIngreso],[NumContrato],
                                                        [NumOrden],[FechaInicio],[FechaFinal],[ValorBruto],[ComiAgen],[ValorAgen],[Valor],
                                                        [ComiConex],[ValorConex],[RucVendedor],[ComiVendedor],[Anunciante],[Agencia],[Contacto],
                                                        [Medio],[Facturado],[FechaCobro],[FechaRegistro],[Estado])
                                                    SELECT IdForeCast,NULL,NULL,NULL,NULL,NULL,NULL,NULL,GETDATE(),@SECUENCIAL,'',FechaInicioPauta,@FECHAFINAL,0,PorcentajeAgencia,0,
                                                        0,@Porcentajeconex,0,@RUCVENDEDOR,0,@ANUNCIANTE,Agencia,@CONTACTO1,@MEDIO,0,NULL,GETDATE(),1
                                                    FROM ForeCast WHERE IdForeCast=@IdForeCast;
                                                    SET @IDCONTA=SCOPE_IDENTITY();

                                                    SELECT @REVISTA=M.Medios FROM ForeCast F INNER JOIN Medios M ON M.IdMedio=F.IdMedio WHERE IdForeCast=@IdForeCast;
                                                    IF(@REVISTA<>'COMPASS MAGAZINE ECCOCOMPASS S.A.')
                                                        BEGIN
                                                            INSERT INTO DetalleContrato(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle)
                                                            SELECT @IdForeCast,@IDCONTA,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,Formato,TotalSegundos,Precio,Unidad,Detalle
                                                            FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;
                                                        END
                                                    ELSE
                                                        BEGIN
                                                            INSERT INTO DetalleContrato(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle)
                                                            SELECT @IdForeCast,@IDCONTA,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,0,TotalSegundos,Precio,Unidad,Detalle
                                                            FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;
                                                        END
                                                    SELECT @ESTADO_EJECUCION=2;
                                                END
                                            ELSE IF(@VALOR>1)
                                                BEGIN
                                                    SELECT @SECUENCIAL=SecuencialLetra+RIGHT('0000'+Ltrim(Rtrim(Secuencial)),Incremento) FROM [dbo].[PrmSecuencialConex];
                                                    UPDATE PrmSecuencialConex SET Secuencial=Secuencial+1;

                                                    WHILE(@INICIO<=@VALOR)
                                                        BEGIN
                                                            SET @FECHA1=NULL;
                                                            SET @FECHA2=NULL;
                                                            SET @RUCVENDEDOR='';
                                                            SET @ANUNCIANTE='';
                                                            SET @MEDIO='';
                                                            SET @ULTIMO=0;
                                                            SET @LETRA='';
                                                            SET @Porcentajeconex=0;
                                                            SELECT @RUCVENDEDOR=Rucedula FROM Empleado WHERE IdEmpleado IN(SELECT IdEmpleado FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                            SELECT @ANUNCIANTE=Descripcion FROM Cliente WHERE IdCliente IN(SELECT IdCliente FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                            SELECT @MEDIO=Medios,@Porcentajeconex=ComisionCone FROM Medios WHERE IdMedio IN(SELECT IdMedio FROM ForeCast WHERE IdForeCast=@IdForeCast);
                                                            SELECT @LETRA=Letra FROM [dbo].[PrmMesesLetras] WHERE Numero=@INICIO;

                                                            IF(@INICIO=1)
                                                                BEGIN
                                                                    SET @FECHAFINAL=NULL;
                                                                    SET @FECHAINCIAL=NULL;
                                                                    SELECT @FECHAFINAL=DATEADD(dd,-(DAY(DATEADD(mm,1,FechaInicioPauta))),DATEADD(mm,1,FechaInicioPauta))
                                                                    FROM ForeCast WHERE IdForeCast=@IdForeCast;

                                                                    INSERT INTO [dbo].[Contrato]([IdForeCast],[IdOrdenRecibida],[IdOrdenEnviada],[IdMaterialR],
                                                                        [IdMaterialE],[IdFacturado],[IdCertificado],[IdPagado],[FechaIngreso],[NumContrato],
                                                                        [NumOrden],[FechaInicio],[FechaFinal],[ValorBruto],[ComiAgen],[ValorAgen],[Valor],
                                                                        [ComiConex],[ValorConex],[RucVendedor],[ComiVendedor],[Anunciante],[Agencia],[Contacto],
                                                                        [Medio],[Facturado],[FechaCobro],[FechaRegistro],[Estado])
                                                                    SELECT IdForeCast,NULL,NULL,NULL,NULL,NULL,NULL,NULL,GETDATE(),@SECUENCIAL+@LETRA,'',FechaInicioPauta,@FECHAFINAL,0,PorcentajeAgencia,0,
                                                                        0,@Porcentajeconex,0,@RUCVENDEDOR,0,@ANUNCIANTE,Agencia,Contacto,@MEDIO,0,NULL,GETDATE(),1
                                                                    FROM ForeCast WHERE IdForeCast=@IdForeCast;
                                                                    SET @IDCONTA=SCOPE_IDENTITY();

                                                                    SELECT @REVISTA=M.Medios FROM ForeCast F INNER JOIN Medios M ON M.IdMedio=F.IdMedio WHERE IdForeCast=@IdForeCast;
                                                                    IF(@REVISTA<>'COMPASS MAGAZINE ECCOCOMPASS S.A.')
                                                                        BEGIN
                                                                            INSERT INTO DetalleContrato(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle)
                                                                            SELECT @IdForeCast,@IDCONTA,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,Formato,TotalSegundos,Precio,Unidad,Detalle
                                                                            FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;
                                                                        END
                                                                    ELSE
                                                                        BEGIN
                                                                            INSERT INTO DetalleContrato(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle)
                                                                            SELECT @IdForeCast,@IDCONTA,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,0,TotalSegundos,Precio,Unidad,Detalle
                                                                            FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;
                                                                        END
                                                                END
                                                            ELSE
                                                                BEGIN
                                                                    SET @FECHAFINAL=NULL;
                                                                    SET @FECHAINCIAL=NULL;
                                                                    DECLARE @PROXIMO INT=0;
                                                                    SET @PROXIMO=@INICIO;
                                                                    SET @PROXIMO=@PROXIMO-1;
                                                                    SELECT @FECHAPROCESO=DATEADD(month,@PROXIMO,FechaInicioPauta) FROM ForeCast WHERE IdForeCast=@IdForeCast;
                                                                    SELECT @FECHAFINAL=DATEADD(dd,-(DAY(DATEADD(mm,1,@FECHAPROCESO))),DATEADD(mm,1,@FECHAPROCESO));
                                                                    SELECT @FECHAINCIAL=DATEADD(dd,-(DAY(@FECHAPROCESO)-1),@FECHAPROCESO);

                                                                    INSERT INTO [dbo].[Contrato]([IdForeCast],[IdOrdenRecibida],[IdOrdenEnviada],[IdMaterialR],
                                                                        [IdMaterialE],[IdFacturado],[IdCertificado],[IdPagado],[FechaIngreso],[NumContrato],
                                                                        [NumOrden],[FechaInicio],[FechaFinal],[ValorBruto],[ComiAgen],[ValorAgen],[Valor],
                                                                        [ComiConex],[ValorConex],[RucVendedor],[ComiVendedor],[Anunciante],[Agencia],[Contacto],
                                                                        [Medio],[Facturado],[FechaCobro],[FechaRegistro],[Estado])
                                                                    SELECT IdForeCast,NULL,NULL,NULL,NULL,NULL,NULL,NULL,GETDATE(),@SECUENCIAL+@LETRA,'',@FECHAINCIAL,@FECHAFINAL,0,PorcentajeAgencia,0,
                                                                        0,@Porcentajeconex,0,@RUCVENDEDOR,0,@ANUNCIANTE,Agencia,Contacto,@MEDIO,0,NULL,GETDATE(),1
                                                                    FROM ForeCast WHERE IdForeCast=@IdForeCast;
                                                                    SET @IDCONTA=SCOPE_IDENTITY();

                                                                    SELECT @REVISTA=M.Medios FROM ForeCast F INNER JOIN Medios M ON M.IdMedio=F.IdMedio WHERE IdForeCast=@IdForeCast;
                                                                    IF(@REVISTA<>'COMPASS MAGAZINE ECCOCOMPASS S.A.')
                                                                        BEGIN
                                                                            INSERT INTO DetalleContrato(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle)
                                                                            SELECT @IdForeCast,@IDCONTA,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,Formato,TotalSegundos,Precio,Unidad,Detalle
                                                                            FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;
                                                                        END
                                                                    ELSE
                                                                        BEGIN
                                                                            INSERT INTO DetalleContrato(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle)
                                                                            SELECT @IdForeCast,@IDCONTA,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,0,TotalSegundos,Precio,Unidad,Detalle
                                                                            FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;
                                                                        END
                                                                END
                                                            SELECT @INICIO=@INICIO+1;
                                                        END
                                                    SELECT @ESTADO_EJECUCION=2;
                                                END
                                            ELSE
                                                BEGIN
                                                    SELECT @ESTADO_EJECUCION=7;
                                                END
                                        END
                                END
                            ELSE
                                BEGIN
                                    DECLARE @AnuncianteCambio varchar(200)='';
                                    DECLARE @AgenciaCambio varchar(200)='';
                                    SELECT @AnuncianteCambio=Descripcion FROM Cliente WHERE IdCliente=@IdCliente;
                                    SELECT @AgenciaCambio=Descripcion FROM Agencia WHERE IdAgencia=@IdAgencia;
                                    UPDATE Contrato SET Anunciante=@AnuncianteCambio,Agencia=@AgenciaCambio WHERE IdForeCast=@IdForeCast;
                                    SELECT @ESTADO_EJECUCION=2;
                                END

                            SELECT @ESTADO_EJECUCION=2;
                        END
                    ELSE
                        BEGIN
                            UPDATE ForeCast SET IdNegocio=@IdNegocioAnterior,IdPropuesta=0 WHERE IdForeCast=@IdForeCast;
                            SELECT @ESTADO_EJECUCION=5;
                        END
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END
        ELSE IF(@Tipo=3)
            BEGIN
                BEGIN TRY
                    UPDATE ForeCast SET Estado=@Estado WHERE IdForeCast=@IdForeCast;
                    SELECT @ESTADO_EJECUCION=3;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END
        ELSE IF(@Tipo=6)
            BEGIN
                BEGIN TRY
                    UPDATE PAUTA SET Estado=0 WHERE IdForeCast=@IdForeCast AND Estado=1;
                    UPDATE DETALLEPAUTA SET Estado=0 WHERE IdForeCast=@IdForeCast AND Estado=1;

                    DECLARE @SECUENCIAL2 VARCHAR(10)='';
                    DECLARE @FECHA12 DATETIME=NULL;
                    DECLARE @FECHA22 DATETIME=NULL;
                    DECLARE @RUCVENDEDOR2 VARCHAR(13)='';
                    DECLARE @ANUNCIANTE2 VARCHAR(150)='';
                    DECLARE @MEDIO2 VARCHAR(150)='';
                    DECLARE @ULTIMO2 INT=0;
                    DECLARE @LETRA2 VARCHAR(1)='';
                    DECLARE @IDCONTA2 BIGINT=0;
                    DECLARE @FECHAFINAL2 DATETIME=NULL;

                    SELECT @RUCVENDEDOR2=Rucedula FROM Empleado WHERE IdEmpleado IN(SELECT IdEmpleado FROM ForeCast WHERE IdForeCast=@IdForeCast);
                    SELECT @ANUNCIANTE2=Descripcion FROM Cliente WHERE IdCliente IN(SELECT IdCliente FROM ForeCast WHERE IdForeCast=@IdForeCast);
                    SELECT @MEDIO2=Medios FROM Medios WHERE IdMedio IN(SELECT IdMedio FROM ForeCast WHERE IdForeCast=@IdForeCast);

                    SELECT @FECHAFINAL2=DATEADD(dd,-(DAY(DATEADD(mm,1,FechaFinalPauta))),DATEADD(mm,1,FechaFinalPauta))
                    FROM ForeCast WHERE IdForeCast=@IdForeCast;

                    INSERT INTO [dbo].[Pauta]
                    SELECT IdForeCast,NULL,NULL,NULL,NULL,NULL,NULL,NULL,GETDATE(),@SECUENCIAL2,'',FechaInicioPauta,@FECHAFINAL2,0,PorcentajeAgencia,0,
                        0,PorcentajeAgencia,0,@RUCVENDEDOR2,0,@ANUNCIANTE2,Agencia,Contacto,@MEDIO2,0,NULL,GETDATE(),1
                    FROM ForeCast WHERE IdForeCast=@IdForeCast;
                    SET @IDCONTA2=SCOPE_IDENTITY();

                    INSERT INTO DetallePauta(IdForeCast,IdContrato,Canal,Programa,Derecho,Duracion,Franja,Tarifa,TotalSegundos,ValorNegocio,Estado,Valor1,Valor2,Valor3,Valor4,Unidad,Detalle,Versiones)
                    SELECT @IdForeCast,@IDCONTA2,Canal,Programa,Derecho,'',Franja,TarifaSegundos,0,0,1,Cantidad,Formato,TotalSegundos,Precio,Unidad,Detalle,Versiones
                    FROM DetalleForecast WHERE IdForeCast=@IdForeCast AND Estado=1;

                    SELECT @ESTADO_EJECUCION=6;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END

    SALIR:
    SELECT @MENSAJES_DIRECCION =
        CASE @ESTADO_EJECUCION
            WHEN 1 THEN 'EL FORECAST REGISTRADO EXITOSAMENTE'
            WHEN 2 THEN 'EL FORECAST FUE MODIFICADO EXITOSAMENTE' + @RESPUESTA
            WHEN 3 THEN 'EL FORECAST FUE DESACTIVADO EXITOSAMENTE'
            WHEN 4 THEN 'YA EXISTE EL FORECAST'
            WHEN 5 THEN 'DEBE INGRESAR LA PROGRAMACION'
            WHEN 6 THEN 'PAUTA GENERADA CORRECTAMENTE'
            WHEN 7 THEN 'POR FAVOR VALIDAR LAS FECHAS DE LAS PAUTAS REGISTRADAS, AL PARCER ESTAN MAL'
            ELSE ISNULL('Error: ' + @ErrorMsg, 'Error interno, contacte al administrador')
        END;
    IF (@ESTADO_EJECUCION IN (1,2,3,4,5,6,7)) COMMIT; ELSE ROLLBACK;
    SELECT @MENSAJES_DIRECCION as valor2, @ESTADO_EJECUCION as valor1;
END
GO
