ALTER PROCEDURE [dbo].[InsertarModificarEliminarPagoContrato]
@json varchar(MAX)='',
@jsonFinal varchar(MAX)='',
@Descripcion varchar(250)='',
@Estado as int=null,
@FechaEmision datetime=null,
@FechaMesContrato datetime=null,
@IdIva as int,
@Tipo as int=0
AS
BEGIN
SET XACT_ABORT ON;
BEGIN TRANSACTION
        DECLARE @ESTADO_EJECUCION SMALLINT = -1;
        DECLARE @MENSAJES_DIRECCION VARCHAR(500);
        DECLARE @NumDocumento VARCHAR(20)='';
        DECLARE @paso1 bigint=0;
        DECLARE @paso2 bigint=0;
        DECLARE @ErrorMsg VARCHAR(500) = '';

        IF(@Tipo=1)
            BEGIN
                BEGIN TRY
                    IF(ISJSON(@json)=0 OR @json IS NULL OR @json='')
                        BEGIN SET @MENSAJES_DIRECCION='JSON de contratos invalido o vacio'; GOTO SALIR; END
                    IF(ISJSON(@jsonFinal)=0 OR @jsonFinal IS NULL OR @jsonFinal='')
                        BEGIN SET @MENSAJES_DIRECCION='JSON final invalido o vacio'; GOTO SALIR; END

                    DECLARE @IdEmpresa int=0;
                    SELECT @IdEmpresa=ID FROM Empresa;

                    INSERT INTO [dbo].[PagoContrato]([IdContrato],[IdEmpresa],[ValorBruto],[ValorNeto],[ValorCobrar],
                        [NumDocumento],[ComisionVendedor],[ComisionPorcentaje],[Descripcion],[FechaRegistro],[Estado])
                    SELECT IdContrato,@IdEmpresa,ValorBruto,ValorNeto,ValorCobrar,NumDocumento,
                        ComisionVendedor,ComisionPorcentaje,@Descripcion,@FechaEmision,1
                    FROM OPENJSON(@json) WITH (
                        IdContrato bigint '$.idContrato',
                        ValorBruto decimal(18,2) '$.valorBruto',
                        ValorNeto decimal(18,2) '$.valorNeto',
                        ValorCobrar decimal(18,2) '$.valorConex',
                        NumDocumento varchar(20) '$.NumDocumento',
                        ComisionVendedor decimal(18,2) '$.ComisionVendedor',
                        ComisionPorcentaje decimal(18,2) '$.ComisionPorcentaje'
                    );

                    INSERT INTO [dbo].[PagoContratoFinal]([IdContrato],[IdEmpresa],[IdMedio],[ValorBruto],[ValorNeto],
                        [ValorCobrar],[NumDocumento],[ComisionVendedor],[ComisionPorcentaje],[Descripcion],
                        [FechaRegistro],[Estado],[IdImpuesto])
                    SELECT IdContrato,@IdEmpresa,IdMedio,ValorBruto,ValorNeto,ValorCobrar,NumDocumento,
                        ComisionVendedor,ComisionPorcentaje,@Descripcion,@FechaEmision,1,@IdIva
                    FROM OPENJSON(@jsonFinal) WITH (
                        IdContrato varchar(200) '$.idContrato',
                        IdMedio bigint '$.IdMedio',
                        ValorBruto decimal(18,2) '$.valorBruto',
                        ValorNeto decimal(18,2) '$.valorNeto',
                        ValorCobrar decimal(18,2) '$.valorConex',
                        NumDocumento varchar(200) '$.NumDocumento',
                        ComisionVendedor decimal(18,2) '$.ComisionVendedor',
                        ComisionPorcentaje decimal(18,2) '$.ComisionPorcentaje'
                    );

                    SET @paso1 = SCOPE_IDENTITY();

                    -- Limpiar separador final del IdContrato concatenado
                    DECLARE @String VARCHAR(MAX)='';
                    SELECT @String=IdContrato FROM PagoContratoFinal WHERE IdPagocontrato=@paso1;
                    SET @String = CASE WHEN @String IS NULL THEN NULL
                                       WHEN LEN(@String)=0 THEN @String
                                       ELSE LEFT(@String, LEN(@String)-1) END;
                    UPDATE PagoContratoFinal SET IdContrato=@String WHERE IdPagocontrato=@paso1;

                    DECLARE @Idmedio bigint=0;
                    DECLARE @Porcentaje decimal(18,2)=0;
                    DECLARE @medio varchar(150)='';
                    DECLARE @NumFactura varchar(20)='';
                    DECLARE @Valor decimal(18,2)=0;
                    DECLARE @FechaPago datetime=null;
                    DECLARE @Iva decimal(18,2)=0;
                    DECLARE @ValorBruto decimal(18,2)=0;
                    DECLARE @ValorNeto decimal(18,2)=0;
                    DECLARE @IdContratoString varchar(max)='';
                    DECLARE @RuCedula varchar(63);

                    SELECT @Idmedio=IdMedio, @Valor=ValorCobrar, @NumFactura=NumDocumento, @FechaPago=FechaRegistro,
                           @ValorBruto=ValorBruto, @ValorNeto=ValorNeto, @IdContratoString=IdContrato
                    FROM PagoContratoFinal WHERE Estado=1 AND IdPagocontrato=@paso1;

                    SELECT @RuCedula=RuCedula FROM Medios WHERE IdMedio=@Idmedio;
                    SELECT @Porcentaje=Iva, @medio=Medios FROM Medios WHERE Estado=1 AND IdMedio=@Idmedio;

                    IF(@Porcentaje=0)
                        BEGIN
                            SET @Iva=0;
                            UPDATE PagoContratoFinal SET ValorRetencion=0, Total=ValorCobrar, Saldo=ValorCobrar, Iva=0
                            WHERE IdPagocontrato=@paso1;
                        END
                    ELSE
                        BEGIN
                            UPDATE PagoContratoFinal SET Iva=(ValorCobrar*(@Porcentaje/100)) WHERE IdPagocontrato=@paso1;
                            UPDATE PagoContratoFinal SET ValorRetencion=0, Total=ValorCobrar+Iva, Saldo=ValorCobrar+Iva WHERE IdPagocontrato=@paso1;
                            SET @Iva=(@Valor*(@Porcentaje/100));
                        END

                    INSERT INTO UnionIngresoEgreso(Venta,Compra,Pagos,Cobros,TipoTransaccion,Descripcion,Valor,
                        FechaRegistro,Estado,VariosProceso,NumDocumento,RuCedula,FechaSistema)
                    VALUES(@paso1,0,0,0,'INGRESO','F-'+@NumFactura+' - '+@medio,(@Valor+@Iva),
                        @FechaEmision,1,NULL,@NumFactura,@RuCedula,GETDATE());
                    SET @paso2 = SCOPE_IDENTITY();

                    IF(@Porcentaje<>0)
                        BEGIN
                            CREATE TABLE #CUENTAS(IdPlanCuenta BIGINT, Descripcion VARCHAR(250), Debe DECIMAL(18,2), Haber DECIMAL(18,2), tipo VARCHAR(2));
                            INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,(@Iva+@Valor),0,'D' FROM PlanCuenta WHERE Codigo='1201';
                            INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,0,@Iva,'H' FROM PlanCuenta WHERE Codigo='2105';
                            INSERT INTO #CUENTAS SELECT IdPlanCuenta,Descripcion,0,@Valor,'H' FROM PlanCuenta WHERE Codigo='4101';
                            INSERT INTO [dbo].[RegistroContable]([IdProcesoContable],[IdPlanCuenta],[Debe],[Haber],[FechaRegistro],[Estado],[EstadoMayorizado])
                            SELECT @paso2,IdPlanCuenta,Debe,Haber,@FechaEmision,1,1 FROM #CUENTAS;
                        END

                    EXEC MostrarComisionVendedorMultiples @IdContratoString,@ValorBruto,@ValorNeto,@FechaPago,@FechaMesContrato,0;

                    UPDATE Sucursal SET Secuencial=Secuencial+1
                    WHERE IDocumento IN (SELECT ID FROM Documento WHERE Nombre='FACTURA');

                    SET @ESTADO_EJECUCION = 1;
                END TRY
                BEGIN CATCH
                    SET @ErrorMsg = ERROR_MESSAGE();
                    GOTO SALIR;
                END CATCH
            END

    SALIR:
    SELECT @MENSAJES_DIRECCION =
        CASE @ESTADO_EJECUCION
            WHEN 1 THEN 'FACTURA REGISTRADA EXITOSAMENTE'
            WHEN 2 THEN 'MODIFICADO EXITOSAMENTE'
            WHEN 3 THEN 'DESACTIVADO EXITOSAMENTE'
            WHEN 4 THEN 'YA EXISTE EL REGISTRO'
            ELSE ISNULL('Error: ' + @ErrorMsg, ISNULL(@MENSAJES_DIRECCION, 'Error interno, contacte al administrador'))
        END;
    IF (@ESTADO_EJECUCION IN (1,2,3,4)) COMMIT; ELSE ROLLBACK;
    SELECT @MENSAJES_DIRECCION as valor2, @ESTADO_EJECUCION as valor1;
END
GO
