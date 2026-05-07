ALTER PROCEDURE [dbo].[InsertarModificarEliminarCuentaPorPagar]
@IdCuentaPorPagar as bigint=null,
@FechaAutorizacion as datetime=null,
@FechaEmision as datetime=null,
@EstadoServicio as varchar(150)='',
@RuCedula as varchar(50)='',
@RazonSocial AS varchar(150)='',
@Email AS varchar(150)='',
@AutorizacionSri as varchar(50)='',
@NumDocumento as varchar(50)='',
@PlazoVencimiento as int=0,
@CompraTarifa0 as decimal(18,2)=0,
@CompraTarifa12 as decimal(18,2)=0,
@Iva as decimal(18,2)=0,
@ValorTotal as decimal(18,2)=0,
@TipoDocumento as varchar(50)='',
@jsonContable as varchar(MAX)='',
@NumExtranjero as varchar(50)='',
@Estado as int=null,
@Tipo as int=0
AS
BEGIN
SET XACT_ABORT ON;
BEGIN TRANSACTION
        DECLARE @ESTADO_EJECUCION SMALLINT = -1;
        DECLARE @MENSAJES_DIRECCION VARCHAR(500);
        DECLARE @paso1 bigint=0;
        DECLARE @paso2 bigint=0;
        DECLARE @ErrorMsg VARCHAR(500) = '';

        IF(@Tipo=1)
            BEGIN
                BEGIN TRY
                    IF(ISJSON(@jsonContable)=0 OR @jsonContable IS NULL OR @jsonContable='')
                        BEGIN SET @MENSAJES_DIRECCION='JSON de asiento contable invalido o vacio'; GOTO SALIR; END

                    INSERT INTO CuentaPorPagar(FechaAutorizacion,FechaEmision,EstadoServicio,RuCedula,RazonSocial,
                        Email,AutorizacionSri,NumDocumento,PlazoVencimiento,CompraTarifa0,CompraTarifa12,Iva,
                        ValorTotal,Saldo,Estado,FechaRegistro,PorRegistrar,TipoDocumento)
                    VALUES(@FechaAutorizacion,@FechaEmision,@EstadoServicio,@RuCedula,@RazonSocial,
                        @Email,@AutorizacionSri,@NumDocumento,@PlazoVencimiento,@CompraTarifa0,@CompraTarifa12,@Iva,
                        @ValorTotal,@ValorTotal,1,GETDATE(),1,@TipoDocumento);
                    SET @paso1 = SCOPE_IDENTITY();

                    INSERT INTO [dbo].[UnionIngresoEgreso]([Venta],[Compra],[Pagos],[Cobros],[TipoTransaccion],
                        [Descripcion],[Valor],[FechaRegistro],[Estado],[FechaSistema],[NumDocumento],[RuCedula])
                    VALUES(0,@paso1,0,0,'COMPRAS','Registro de una compra N ' + @NumDocumento,
                        @ValorTotal,@FechaEmision,1,GETDATE(),@NumDocumento,@RuCedula);
                    SET @paso2 = SCOPE_IDENTITY();

                    INSERT INTO [dbo].[RegistroContable]([IdProcesoContable],[IdPlanCuenta],[Debe],[Haber],[FechaRegistro],[Estado],[EstadoMayorizado])
                    SELECT @paso2,IdPlanCuenta,Debe,Haber,@FechaEmision,1,1
                    FROM OPENJSON(@jsonContable) WITH (
                        IdPlanCuenta bigint '$.IdPlanCuenta',
                        Debe decimal(18,2) '$.Debe',
                        Haber decimal(18,2) '$.Haber',
                        FechaRegistro datetime '$.FechaRegistro'
                    );

                    IF(EXISTS(SELECT 1 FROM DetalleLiquidacion WHERE IdCuentaPorPagar=@IdCuentaPorPagar))
                        UPDATE DetalleLiquidacion SET NumFacturaEx=@NumExtranjero WHERE IdCuentaPorPagar=@IdCuentaPorPagar;

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
                    UPDATE CuentaPorPagar SET
                        FechaAutorizacion=@FechaAutorizacion, FechaEmision=@FechaEmision, EstadoServicio=@EstadoServicio,
                        RuCedula=@RuCedula, RazonSocial=@RazonSocial, Email=@Email, AutorizacionSri=@AutorizacionSri,
                        NumDocumento=@NumDocumento, PlazoVencimiento=@PlazoVencimiento, CompraTarifa0=@CompraTarifa0,
                        CompraTarifa12=@CompraTarifa12, Iva=@Iva, ValorTotal=@ValorTotal, Saldo=@ValorTotal,
                        Estado=1, FechaRegistro=GETDATE(), PorRegistrar=1, TipoDocumento=@TipoDocumento
                    WHERE IdCuentaPorPagar=@IdCuentaPorPagar;

                    DECLARE @Subtotal decimal(18,2)=0;
                    DECLARE @IdProcesoContable BIGINT=0;
                    DECLARE @CodContable VARCHAR(20)='';
                    SELECT @IdProcesoContable=ISNULL(IdProcesoContable,0) FROM UnionIngresoEgreso WHERE Compra=@IdCuentaPorPagar;
                    SELECT @CodContable=CodContable FROM Proveedor WHERE RuCedula=@RuCedula;

                    IF(@CompraTarifa0>0) SET @Subtotal=@CompraTarifa0;
                    ELSE IF(@CompraTarifa12>0) SET @Subtotal=@CompraTarifa12;

                    IF(@IdProcesoContable>0)
                        BEGIN
                            CREATE TABLE #CUENTAS(IdPlanCuenta BIGINT, Descripcion VARCHAR(250), Debe DECIMAL(18,2), Haber DECIMAL(18,2), tipo VARCHAR(2));
                            UPDATE RegistroContable SET Estado=0 WHERE IdProcesoContable=@IdProcesoContable;
                            UPDATE UnionIngresoEgreso SET FechaRegistro=@FechaEmision WHERE IdProcesoContable=@IdProcesoContable;
                            INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,0,@ValorTotal,'H' FROM PlanCuenta WHERE Codigo='2101';
                            IF(@CompraTarifa0<>0) INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,@CompraTarifa0,0,'D' FROM PlanCuenta WHERE Codigo=@CodContable;
                            IF(@CompraTarifa12<>0) INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,@CompraTarifa12,0,'D' FROM PlanCuenta WHERE Codigo=@CodContable;
                            INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,@Iva,0,'D' FROM PlanCuenta WHERE Codigo='1209';
                            INSERT INTO [dbo].[RegistroContable]([IdProcesoContable],[IdPlanCuenta],[Debe],[Haber],[FechaRegistro],[Estado],[EstadoMayorizado])
                            SELECT @IdProcesoContable,IdPlanCuenta,Debe,Haber,@FechaEmision,1,1 FROM #CUENTAS;
                        END
                    ELSE
                        BEGIN
                            -- No existia asiento previo: crear UnionIngresoEgreso nuevo
                            INSERT INTO [dbo].[UnionIngresoEgreso]([Venta],[Compra],[Pagos],[Cobros],[TipoTransaccion],
                                [Descripcion],[Valor],[FechaRegistro],[Estado],[FechaSistema],[NumDocumento],[RuCedula])
                            VALUES(0,@IdCuentaPorPagar,0,0,'COMPRAS','Registro de una compra N ' + @NumDocumento,
                                @ValorTotal,@FechaEmision,1,GETDATE(),@NumDocumento,@RuCedula);
                        END

                    IF(EXISTS(SELECT 1 FROM DetalleLiquidacion WHERE IdCuentaPorPagar=@IdCuentaPorPagar))
                        UPDATE DetalleLiquidacion SET NumFacturaEx=@NumExtranjero WHERE IdCuentaPorPagar=@IdCuentaPorPagar;

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
                    SET @ESTADO_EJECUCION = 3;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END

    SALIR:
    SELECT @MENSAJES_DIRECCION =
        CASE @ESTADO_EJECUCION
            WHEN 1 THEN 'LA CUENTA POR PAGAR REGISTRADA EXITOSAMENTE'
            WHEN 2 THEN 'LA CUENTA POR PAGAR FUE MODIFICADA EXITOSAMENTE'
            WHEN 3 THEN 'LA CUENTA POR PAGAR FUE DESACTIVADO EXITOSAMENTE'
            WHEN 4 THEN 'YA EXISTE LA CUENTA POR PAGAR'
            ELSE ISNULL('Error: ' + @ErrorMsg, ISNULL(@MENSAJES_DIRECCION, 'Error interno, contacte al administrador'))
        END;
    IF (@ESTADO_EJECUCION IN (1,2,3,4)) COMMIT; ELSE ROLLBACK;
    SELECT @MENSAJES_DIRECCION as valor2, @ESTADO_EJECUCION as valor1;
END
GO
