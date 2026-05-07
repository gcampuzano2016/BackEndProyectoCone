ALTER PROCEDURE [dbo].[InsertarModificarEliminarContrato]
@IdContrato as bigint=null,
@IdForeCast as bigint=null,
@IdOrdenRecibida as bigint=null,
@IdOrdenEnviada as bigint=null,
@IdMaterialR as bigint=null,
@IdMaterialE as bigint=null,
@IdFacturado as bigint=null,
@IdCertificado as bigint=null,
@IdPagado as bigint=null,
@FechaIngreso as datetime=null,
@NumContrato as varchar(50)='',
@NumOrden as varchar(50)='',
@FechaInicio as datetime=null,
@FechaFinal as datetime=null,
@ValorBruto as decimal(18,2)=0,
@ComiAgen as decimal(18,2)=0,
@ValorAgen as decimal(18,2)=0,
@Valor as decimal(18,2)=0,
@ComiConex as decimal(18,2)=0,
@ValorConex as decimal(18,2)=0,
@RucVendedor as varchar(50)='',
@ComiVendedor as decimal(18,2)=0,
@Anunciante as varchar(250)='',
@Agencia as varchar(150)='',
@Contacto as varchar(150)='',
@Medio as varchar(150)='',
@Facturado as decimal(18,2)=0,
@FechaCobro as datetime=null,
@Estado as int=null,
@Tipo as int=0
AS
BEGIN
SET XACT_ABORT ON;
BEGIN TRANSACTION
        DECLARE @ESTADO_EJECUCION SMALLINT = -1;
        DECLARE @MENSAJES_DIRECCION VARCHAR(500);
        DECLARE @ErrorMsg VARCHAR(500) = '';

        IF(@Tipo=1)
            BEGIN
                BEGIN TRY
                    INSERT INTO [dbo].[Contrato]([IdForeCast],[IdOrdenRecibida],[IdOrdenEnviada],[IdFacturado],[IdCertificado],[IdPagado],
                        [FechaIngreso],[NumContrato],[NumOrden],[FechaInicio],[FechaFinal],[ValorBruto],[ComiAgen],[ValorAgen],[Valor],
                        [ComiConex],[ValorConex],[RucVendedor],[ComiVendedor],[Anunciante],[Agencia],[Contacto],[Medio],[Facturado],
                        [FechaCobro],[FechaRegistro],[Estado])
                    VALUES(@IdForeCast,@IdOrdenRecibida,@IdOrdenEnviada,@IdFacturado,@IdCertificado,@IdPagado,
                        @FechaIngreso,@NumContrato,@NumOrden,@FechaInicio,@FechaFinal,@ValorBruto,@ComiAgen,@ValorAgen,@Valor,
                        @ComiConex,@ValorConex,@RucVendedor,@ComiVendedor,@Anunciante,@Agencia,@Contacto,@Medio,@Facturado,
                        @FechaCobro,GETDATE(),@Estado);
                    SET @ESTADO_EJECUCION = 1;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END
        ELSE IF(@Tipo=2)
            BEGIN
                BEGIN TRY
                    INSERT INTO [dbo].[ContratoHistorico]([IdContrato],[IdForeCast],[IdOrdenRecibida],[IdOrdenEnviada],[IdFacturado],[IdCertificado],
                        [IdPagado],[FechaIngreso],[NumContrato],[NumOrden],[FechaInicio],[FechaFinal],[ValorBruto],[ComiAgen],[ValorAgen],[Valor],
                        [ComiConex],[ValorConex],[RucVendedor],[ComiVendedor],[Anunciante],[Agencia],[Contacto],[Medio],[Facturado],[FechaCobro],
                        [FechaRegistro],[Estado])
                    SELECT [IdContrato],[IdForeCast],[IdOrdenRecibida],[IdOrdenEnviada],[IdFacturado],[IdCertificado],[IdPagado],[FechaIngreso],
                        [NumContrato],[NumOrden],[FechaInicio],[FechaFinal],[ValorBruto],[ComiAgen],[ValorAgen],[Valor],[ComiConex],[ValorConex],
                        [RucVendedor],[ComiVendedor],[Anunciante],[Agencia],[Contacto],[Medio],[Facturado],[FechaCobro],[FechaRegistro],[Estado]
                    FROM [dbo].[Contrato] WHERE IdContrato=@IdContrato;

                    IF(@IdOrdenRecibida=0) SET @IdOrdenRecibida=NULL;
                    IF(@IdOrdenEnviada=0)  SET @IdOrdenEnviada=NULL;
                    IF(@IdFacturado=0)     SET @IdFacturado=NULL;
                    IF(@IdCertificado=0)   SET @IdCertificado=NULL;
                    IF(@IdPagado=0)        SET @IdPagado=NULL;

                    UPDATE [dbo].[Contrato] SET
                        [IdForeCast]=@IdForeCast,[IdOrdenRecibida]=@IdOrdenRecibida,[IdOrdenEnviada]=@IdOrdenEnviada,
                        [IdFacturado]=@IdFacturado,[IdCertificado]=@IdCertificado,[IdPagado]=@IdPagado,[FechaIngreso]=@FechaIngreso,
                        [NumContrato]=@NumContrato,[NumOrden]=@NumOrden,[FechaInicio]=@FechaInicio,[FechaFinal]=@FechaFinal,
                        [ValorBruto]=@ValorBruto,[ComiAgen]=@ComiAgen,[ValorAgen]=@ValorAgen,[Valor]=@Valor,[ComiConex]=@ComiConex,
                        [ValorConex]=@ValorConex,[RucVendedor]=@RucVendedor,[ComiVendedor]=@ComiVendedor,[Anunciante]=@Anunciante,
                        [Agencia]=@Agencia,[Contacto]=@Contacto,[Medio]=@Medio,[Facturado]=@Facturado,[FechaCobro]=@FechaCobro,
                        [FechaRegistro]=GETDATE(),[Estado]=@Estado
                    WHERE IdContrato=@IdContrato;
                    SET @ESTADO_EJECUCION = 2;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END
        ELSE IF(@Tipo=3)
            BEGIN
                BEGIN TRY
                    UPDATE [dbo].[Contrato] SET Estado=@Estado WHERE IdForeCast=@IdForeCast;
                    UPDATE DetalleContrato SET Estado=@Estado, FechaIngreso=GETDATE() WHERE Estado=1 AND IdForeCast=@IdForeCast;
                    SET @ESTADO_EJECUCION = 3;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END
        ELSE IF(@Tipo=5)
            BEGIN
                BEGIN TRY
                    UPDATE Contrato SET Estado=@Estado, FechaIngreso=GETDATE() WHERE Estado=1 AND IdContrato=@IdContrato;
                    UPDATE DetalleContrato SET Estado=@Estado, FechaIngreso=GETDATE() WHERE Estado=1 AND IdContrato=@IdContrato;
                    SET @ESTADO_EJECUCION = 5;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END

    SALIR:
    SELECT @MENSAJES_DIRECCION =
        CASE @ESTADO_EJECUCION
            WHEN 1 THEN 'EL CONTRATO REGISTRADO EXITOSAMENTE'
            WHEN 2 THEN 'EL CONTRATO FUE MODIFICADO EXITOSAMENTE'
            WHEN 3 THEN 'EL CONTRATO FUE DESACTIVADO EXITOSAMENTE'
            WHEN 4 THEN 'YA EXISTE EL CONTRATO'
            WHEN 5 THEN 'EL CONEX FUE DESACTIVADO EXITOSAMENTE'
            ELSE ISNULL('Error: ' + @ErrorMsg, 'Error interno, contacte al administrador')
        END;
    IF (@ESTADO_EJECUCION IN (1,2,3,4,5)) COMMIT; ELSE ROLLBACK;
    SELECT @MENSAJES_DIRECCION as valor2, @ESTADO_EJECUCION as valor1;
END
GO
