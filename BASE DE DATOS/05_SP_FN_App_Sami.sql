---Referencia Accion
--1:Alta-Insert
--2:Modificacion-Update
--3:Borrado-Delete


CREATE PROCEDURE CONTROL_ZETA.SP_BUSCA_PAIS(@NOMBRE_PAIS VARCHAR,
                                            @CODIGO_PAIS INT OUTPUT )
AS
BEGIN

SET @CODIGO_PAIS=0

SELECT @CODIGO_PAIS = PAIS_ID 
  FROM  CONTROL_ZETA.PAIS 
 WHERE  UPPER(PAIS_DETALLE) = UPPER(@NOMBRE_PAIS)


IF (@CODIGO_PAIS = 0)
BEGIN
INSERT INTO CONTROL_ZETA.PAIS VALUES(UPPER(@NOMBRE_PAIS))
set @CODIGO_PAIS = @@IDENTITY
END

END

CREATE PROCEDURE CONTROL_ZETA.SP_BUSCA_NACIONALIDAD
                         (@NOMBRE_NAC VARCHAR,
                          @CODIGO_NAC INT OUTPUT )
AS
BEGIN

SET @CODIGO_NAC=0

SELECT @CODIGO_NAC = NAC_ID 
  FROM  CONTROL_ZETA.NACIONALIDAD 
 WHERE  UPPER(NAC_DETALLE) = UPPER(@NOMBRE_NAC)


IF (@CODIGO_NAC = 0)
BEGIN
INSERT INTO CONTROL_ZETA.NACIONALIDAD VALUES(UPPER(@NOMBRE_NAC))
set @CODIGO_NAC = @@IDENTITY
END

END




/*CREATE PROCEDURE CONTROL_ZETA.SP_BUSCA_PAIS(@NOMBRE_PAIS VARCHAR,
                                            @CODIGO_PAIS INT OUTPUT )

*/
CREATE PROCEDURE CONTROL_ZETA.SP_BUSCA_LOCALIDAD(@NOMBRE_LOCALIDAD VARCHAR(50),
                                                @CODIGO_LOCAL INT OUTPUT )

AS
BEGIN

SET @CODIGO_LOCAL=0

SELECT @CODIGO_LOCAL = LOC_ID 
  FROM  CONTROL_ZETA.LOCALIDAD 
 WHERE  UPPER(LOC_DETALLE) = UPPER(@NOMBRE_LOCALIDAD)

IF (@CODIGO_LOCAL = 0)
BEGIN
INSERT INTO CONTROL_ZETA.LOCALIDAD VALUES(@NOMBRE_LOCALIDAD)
set @CODIGO_LOCAL = @@IDENTITY
END

END

-- 

CREATE FUNCTION CONTROL_ZETA.FN_REGISTRADO_HOTEL (@ID_HOTEL INT,
                                                  @RESERVA_ID  NUMERIC
                                                  )
RETURNS  INT
AS  
BEGIN
RETURN (select CASE 
               WHEN C.CLIENTE_ESTADO = 'H' 
                    THEN COUNT(*) 
               ELSE -1
               END 
          from CONTROL_ZETA.RESERVA R, CONTROL_ZETA.CLIENTE C
         where R.RESERVA_ID = @RESERVA_ID
           and R.RESERVA_ID_HOTEL = @ID_HOTEL
           AND R.CLIENTE_ID = C.CLIENTE_ID
           AND C.CLIENTE_ESTADO = 'H'
           GROUP BY R.RESERVA_ID, R.CLIENTE_ID, C.CLIENTE_ESTADO)
END;

GO
DROP PROCEDURE CONTROL_ZETA.SP_ABM_USUARIO
GO
---ABM USUARIO (Carga tabla USUARIO y EMPLEADO) 
CREATE PROCEDURE CONTROL_ZETA.SP_ABM_USUARIO
 @ACCION SMALLINT,
 @USUARIO VARCHAR (50),
 @PASS VARCHAR(70),
 @NOMBRE VARCHAR(50),
 @APELLIDO VARCHAR(50),
 @TIPO_DOC TINYINT,
 @DOC VARCHAR(15),
 @MAIL VARCHAR(50),
 @TEL VARCHAR(10),
 @DOM VARCHAR(50),
 @FECHA_NAC DATE,
 @ESTADO VARCHAR(1),
 @HOTEL_ID INT,
 @ERROR TINYINT OUTPUT 
AS
BEGIN

IF (@ACCION=1)
BEGIN
	IF NOT EXISTS (SELECT 1 FROM CONTROL_ZETA.USUARIO WHERE USR_USERNAME = @USUARIO)
	BEGIN
		INSERT INTO CONTROL_ZETA.USUARIO
		(USR_USERNAME, USR_PASS, USR_ESTADO, USR_INTENTOS)
		VALUES(@USUARIO, @PASS, 'H',1)
		
		INSERT INTO CONTROL_ZETA.EMPLEADO
		(USR_USERNAME, EMP_NOMBRE, EMP_APELLIDO, EMP_ID_TIPO_DOC, EMP_DOC,
		 EMP_MAIL, EMP_TEL, EMP_DOM, EMP_FECHA_NAC)
		 VALUES
		 (@USUARIO, @NOMBRE, @APELLIDO,@TIPO_DOC, @DOC,
		  @MAIL, @TEL, @DOM, @FECHA_NAC)
				
		SET @ERROR=1
	END;
	ELSE
	SET @ERROR=3
END;
ELSE IF @ACCION=2
BEGIN
--MODIFICACION
	IF EXISTS (SELECT 1 FROM CONTROL_ZETA.USUARIO WHERE USR_USERNAME = @USUARIO)
	BEGIN

			UPDATE CONTROL_ZETA.EMPLEADO 
			SET EMP_NOMBRE = @NOMBRE, 
			    EMP_APELLIDO = @APELLIDO , 
			    EMP_ID_TIPO_DOC = @TIPO_DOC , 
			    EMP_DOC = @DOC,
		        EMP_MAIL = @MAIL, 
		        EMP_TEL = @TEL, 
		        EMP_DOM = @DOM, 
		        EMP_FECHA_NAC = @FECHA_NAC
			WHERE USR_USERNAME = @USUARIO
			
			IF @PASS IS NOT NULL --Si es null es que no modifico la pass
			BEGIN
				UPDATE CONTROL_ZETA.USUARIO
				SET USR_PASS=@PASS
				WHERE USR_USERNAME = @USUARIO
			END


			DELETE CONTROL_ZETA.USR_ROL_HOTEL 
			WHERE USR_USERNAME = @USUARIO
			AND HOTEL_ID=@HOTEL_ID
			
			SET @ERROR=1
		END;
		ELSE
		SET @ERROR=3
	
END

ELSE IF @ACCION=3
BEGIN
--BAJA
IF EXISTS (SELECT 1 FROM CONTROL_ZETA.USUARIO WHERE USR_USERNAME = @USUARIO)
	BEGIN
		UPDATE CONTROL_ZETA.USUARIO 
		SET USR_ESTADO = 'I' 
		WHERE USR_USERNAME= @USUARIO
		
		SET @ERROR=1
	END
	ELSE
		SET @ERROR=2
END


END;



---ASIGNACION DE USUARIO_ROL_HOTEL (Carga tabla USR_ROL_HOTEL)
CREATE PROCEDURE CONTROL_ZETA.SP_USR_ROL_HOTEL 
@USUARIO VARCHAR (50),
@ROL_ID TINYINT,
@HOTEL_ID INT
as
begin

 INSERT INTO CONTROL_ZETA.USR_ROL_HOTEL
 (USR_USERNAME, HOTEL_ID, ROL_ID)
 VALUES
 (@USUARIO,  @HOTEL_ID, @ROL_ID)


end;
 GO
DROP FUNCTION CONTROL_ZETA.FN_EXISTE_CLIENTE
GO
CREATE FUNCTION CONTROL_ZETA.FN_EXISTE_CLIENTE (@TIPO_IDENT TINYINT,
												@NRO_IDENT VARCHAR(15),
												@EMAIL VARCHAR(50))
RETURNS  INT
AS  
BEGIN
RETURN (SELECT COUNT(*) 
          FROM CONTROL_ZETA.CLIENTE C
      WHERE (C.CLIENTE_ID_TIPO_DOC = @TIPO_IDENT AND C.CLIENTE_DOC = @NRO_IDENT)
	   OR (C.CLIENTE_MAIL = @EMAIL))

END;

GO
DROP PROCEDURE CONTROL_ZETA.MB_CLIENTE
GO
--funcion a utilizar cuando ya se validó y se eligió que cliente borrar o modificar
CREATE PROCEDURE CONTROL_ZETA.MB_CLIENTE(@NOMBRE VARCHAR(50),
@APELLIDO VARCHAR(50),
@TIPO_IDENT TINYINT,
@NRO_IDENT VARCHAR(15),
@EMAIL VARCHAR(50),
@TEL VARCHAR(10),
@NOMBRE_LOC VARCHAR(50),
@NOMBRE_PAIS VARCHAR(50),
@DOM_CALLE VARCHAR(50),
@DOM_NRO INT,
@DEPTO VARCHAR(2),
@DOM_PISO VARCHAR(10),
@NACIONALIDAD_NOMBRE VARCHAR(50),
@FECHA_NAC DATETIME,
@CODIGO_ENTRADA TINYINT,
@HOTEL INT,
@RESERVA_ID NUMERIC,
@CLIENTE_ID NUMERIC,
@CODIGO TINYINT OUTPUT)
AS
BEGIN
DECLARE @ID_PAIS TINYINT 
DECLARE @ID_LOCALIDAD TINYINT 
DECLARE @CODIGO_NAC TINYINT
DECLARE @HAY_MAS_DE_UNO TINYINT

IF @CODIGO_ENTRADA =2 
BEGIN
	EXEC CONTROL_ZETA.SP_BUSCA_PAIS @NOMBRE_PAIS, @ID_PAIS OUTPUT
	EXEC CONTROL_ZETA.SP_BUSCA_LOCALIDAD @NOMBRE_LOC, @ID_LOCALIDAD OUTPUT
	EXEC CONTROL_ZETA.SP_BUSCA_NACIONALIDAD @NACIONALIDAD_NOMBRE, @CODIGO_NAC OUTPUT 
END

SET @HAY_MAS_DE_UNO= CONTROL_ZETA.FN_EXISTE_CLIENTE(@TIPO_IDENT, @NRO_IDENT, @EMAIL)

IF (@CODIGO_ENTRADA =  2  ) --MODIFICO
BEGIN
	IF @HAY_MAS_DE_UNO >1
	BEGIN
		UPDATE CONTROL_ZETA.CLIENTE
		SET CLIENTE_NOMBRE = @NOMBRE,
			CLIENTE_APELLIDO = @APELLIDO,
			CLIENTE_ID_TIPO_DOC = @TIPO_IDENT,
			CLIENTE_DOC = @NRO_IDENT , 
			CLIENTE_MAIL = @EMAIL , 
			CLIENTE_TEL = @TEL ,
			CLIENTE_ID_LOC = @ID_LOCALIDAD , 
			CLIENTE_ID_PAIS_ORIGEN = @ID_PAIS , 
			CLIENTE_DOM_CALLE = @DOM_CALLE ,
			CLIENTE_DOM_NRO = @DOM_NRO ,
			CLIENTE_DPTO = @DEPTO ,
			CLIENTE_DOM_PISO = @DOM_PISO ,
			CLIENTE_NAC_ID = @CODIGO_NAC ,
			CLIENTE_ESTADO = 'H' ,
			CLIENTE_FECHA_NAC = @FECHA_NAC 
		WHERE CLIENTE_ID = @CLIENTE_ID    
		SET @CODIGO= 1 --Se realizo modif OK
	END
	ELSE SET @CODIGO=2-->Ya existe cliente con ese doc o ese mail

END
ELSE IF @CODIGO_ENTRADA =  3
BEGIN
	UPDATE CONTROL_ZETA.CLIENTE
	SET CLIENTE_ESTADO = 'I' 
	WHERE CLIENTE_ID = @CLIENTE_ID 
	
	SET @CODIGO= 1 --Se realizo baja logica OK   
END
END
GO


--Funcion para usar la primera vez
---ABM CLIENTE (Carga tabla clientes)
/*
--Codigo de salida
-1-->Alta/Update/Baja OK
-2-->Modif/Baja de usuario inexistente
-3-->Alta de usuario existente (Existe su documento o existe su mail informar en pantalla)
-4-->Se devuelve listado de clientes porque se consulto (para update o baja) un cliente que era inconsistente (informar)

--BUSCO
1) NO EXISTE -> ALTA
2) EXISTE 1 SOLO -> ELIMINAR, MODIFICAR
3) EXISTE MAS DE 1 --> DEVUELVO TODO Y QUE MODIFIQUE LO QUE QUIERA EL RECEPCIONISTA
*/
 
CREATE PROCEDURE CONTROL_ZETA.ABM_CLIENTE 

(@NOMBRE VARCHAR(50),
@APELLIDO VARCHAR(50),
@TIPO_IDENT TINYINT,
@NRO_IDENT VARCHAR(15),
@EMAIL VARCHAR(50),
@TEL VARCHAR(10),
@NOMBRE_LOC VARCHAR(50),
@NOMBRE_PAIS VARCHAR(50),
@DOM_CALLE VARCHAR(50),
@DOM_NRO INT,
@DEPTO VARCHAR(2),
@DOM_PISO VARCHAR(10),
@NACIONALIDAD_NOMBRE VARCHAR(50),
@FECHA_NAC DATETIME,
@CODIGO_ENTRADA TINYINT,
@HOTEL INT,
@RESERVA_ID NUMERIC,
@CLIENTE_ID NUMERIC,
@CODIGO TINYINT OUTPUT
)
AS

BEGIN

DECLARE @ID_PAIS TINYINT 
DECLARE @ID_LOCALIDAD TINYINT 
DECLARE @CODIGO_GEN TINYINT
DECLARE @CODIGO_NAC TINYINT
DECLARE @ROW_SELECT INT

IF @CODIGO_ENTRADA IN (1,2)
BEGIN
EXEC CONTROL_ZETA.SP_BUSCA_PAIS @NOMBRE_PAIS, @ID_PAIS OUTPUT

EXEC CONTROL_ZETA.SP_BUSCA_LOCALIDAD @NOMBRE_LOC, @ID_LOCALIDAD OUTPUT

EXEC CONTROL_ZETA.SP_BUSCA_NACIONALIDAD @NACIONALIDAD_NOMBRE, @CODIGO_NAC OUTPUT 

END

 SET @CODIGO_GEN = CONTROL_ZETA.FN_EXISTE_CLIENTE(@TIPO_IDENT, @NRO_IDENT, @EMAIL)

 -- SI @CODIGO_GEN = 1 ENTONCES YA ESTA DADO DE ALTA EN EL HOTEL

IF (@CODIGO_ENTRADA =  1  AND @CODIGO_GEN = 0 ) --ALTA
BEGIN

	INSERT INTO CONTROL_ZETA.CLIENTE
	(CLIENTE_NOMBRE,CLIENTE_APELLIDO,CLIENTE_ID_TIPO_DOC,
	 CLIENTE_DOC, CLIENTE_MAIL,CLIENTE_TEL,CLIENTE_ID_LOC, 
	 CLIENTE_ID_PAIS_ORIGEN, CLIENTE_DOM_CALLE,CLIENTE_DOM_NRO,
	 CLIENTE_DPTO,CLIENTE_DOM_PISO,CLIENTE_NAC_ID,CLIENTE_ESTADO,
	 CLIENTE_FECHA_NAC)
	VALUES
	(@NOMBRE,@APELLIDO,@TIPO_IDENT,@NRO_IDENT, @EMAIL, @TEL,
  	 @ID_LOCALIDAD, @ID_PAIS, @DOM_CALLE,@DOM_NRO,@DEPTO,
	 @DOM_PISO,@CODIGO_NAC,'H',@FECHA_NAC)
	 SET @CODIGO= 1 --Se realizo alta OK
	 
	 
END
ELSE 
BEGIN
 SET @CODIGO = 3 -- ALTA DE USUARIO EXISTENTE
END

IF (@CODIGO_ENTRADA =  2 AND @CODIGO_GEN = 1 ) --MODIFICO
BEGIN
	UPDATE CONTROL_ZETA.CLIENTE
	SET CLIENTE_NOMBRE = @NOMBRE,
		CLIENTE_APELLIDO = @APELLIDO,
		CLIENTE_ID_TIPO_DOC = @TIPO_IDENT,
		CLIENTE_DOC = @NRO_IDENT , 
		CLIENTE_MAIL = @EMAIL , 
		CLIENTE_TEL = @TEL ,
		CLIENTE_ID_LOC = @ID_LOCALIDAD , 
		CLIENTE_ID_PAIS_ORIGEN = @ID_PAIS , 
		CLIENTE_DOM_CALLE = @DOM_CALLE ,
		CLIENTE_DOM_NRO = @DOM_NRO ,
		CLIENTE_DPTO = @DEPTO ,
		CLIENTE_DOM_PISO = @DOM_PISO ,
		CLIENTE_NAC_ID = @CODIGO_NAC ,
		CLIENTE_ESTADO = 'H' ,
		CLIENTE_FECHA_NAC = @FECHA_NAC 
	WHERE CLIENTE_ID = @CLIENTE_ID    
	SET @CODIGO= 1 --Se realizo modif OK
	
END

ELSE 
BEGIN
  SET @CODIGO = 2 -- MODIFICACION DE CLIENTE INEXISTENTE
END

IF @CODIGO_ENTRADA =  3 AND @CODIGO_GEN = 1  -- ELIMINO, BAJA LOGICA 
BEGIN
	UPDATE CONTROL_ZETA.CLIENTE
	SET CLIENTE_ESTADO = 'I' 
	WHERE CLIENTE_ID = @CLIENTE_ID 
	
	SET @CODIGO= 1 --Se realizo baja logica OK   
END
ELSE
BEGIN
  SET @CODIGO = 2 -- BAJA DE CLIENTE INEXISTENTE
END


-- CONSULTO SI PERTENECE AL HOTEL
-- @CODIGO_GEN > 1 INDICA QUE HUBO MAS DE UN RESULTADO CON LOS MISMOS DATOS
--IF ((@CODIGO_ENTRADA = 0 AND @CODIGO_GEN = 1)  OR @CODIGO_GEN > 1 ) 
IF  @CODIGO_GEN > 1 
BEGIN
    -- DEVUELVO LOS DATOS DEL CLIENTE EXISTENTE
	SELECT * 
	FROM CONTROL_ZETA.CLIENTE
	WHERE (CLIENTE_NOMBRE = @NOMBRE AND CLIENTE_APELLIDO = @APELLIDO)
	   OR (CLIENTE_ID_TIPO_DOC = @TIPO_IDENT AND CLIENTE_DOC = @NRO_IDENT)
	   OR (CLIENTE_MAIL = @EMAIL)
	SET @CODIGO = 4

--IF @CODIGO_GEN > 1
--BEGIN
--SET @CODIGO = 4 -- MAS DE UN CLIENTE CON LOS MISMOS DATOS
--END

END

END


CREATE PROCEDURE CONTROL_ZETA.SP_REGISTRAR_ESTADIA (@RESERVA_ID NUMERIC,
                                                    @USUARIO VARCHAR(50),
                                                    @CODIGO_IN_OUT TINYINT,
                                                    @FECHA DATETIME,
                                                    @CODIGO INT OUTPUT)
AS
BEGIN
DECLARE @CANT INT
DECLARE @CLIENTE_ID NUMERIC
DECLARE @ESTADIA INT
SET @CANT = 0
SET @ESTADIA = 0
SET @CLIENTE_ID = 0
SET @CODIGO = 1 --TODO OK

IF @CODIGO_IN_OUT = 1 -- INGRESO
BEGIN

-- DUDAS EN CAMBIAR EL SELECT, CORREGIRLO SI FALLA
Select @CANT = COUNT(*) , 
       @CLIENTE_ID = CLIENTE_ID
  from CONTROL_ZETA.RESERVA 
 where RESERVA_ID = @RESERVA_ID 
   and RESERVA_FECHA_INICIO = CONVERT(VARCHAR(12),@FECHA,103)
   --AND RESERVA_FECHA_HASTA IS NULL
   and RESERVA_ESTADO in ('RM', 'RC')
   GROUP BY RESERVA_ID,CLIENTE_ID ;
   
	IF @CANT = 1  AND EXISTS (SELECT 1 
                                      FROM CONTROL_ZETA.RESERVA 
                                      WHERE RESERVA_ID = @RESERVA_ID
                                        AND RESERVA_ESTADO in ('RM', 'RC')) -- check in
	BEGIN   
		-- CONTINUA OK

		INSERT INTO CONTROL_ZETA.ESTADIA (EST_RESERVA_ID, EST_FECHA_DESDE)
		VALUES
		(@RESERVA_ID, CONVERT(VARCHAR(12),@FECHA,103))

		SET @ESTADIA = @@IDENTITY

		INSERT INTO CONTROL_ZETA.ESTADIA_CLIENTE (EST_ID, CLIENTE_ID)
		VALUES
		(@ESTADIA, @CLIENTE_ID)
		
		UPDATE CONTROL_ZETA.RESERVA
		  SET  RESERVA_ESTADO = 'REC'
		 WHERE RESERVA_ID = @RESERVA_ID 
	END
	ELSE IF NOT EXISTS (SELECT 1  FROM CONTROL_ZETA.RESERVA 
                                      WHERE RESERVA_ID = @RESERVA_ID
                                        AND RESERVA_ESTADO in ('RM', 'RC'))
        SET @codigo = 6                                
	ELSE 
     BEGIN
		SET @codigo = 5 -- RESERVA FUERA DE TIEMPO
	END
END

IF (@CODIGO_IN_OUT = 2) AND EXISTS (SELECT 1 
                                      FROM CONTROL_ZETA.RESERVA 
                                      WHERE RESERVA_ID = @RESERVA_ID
                                        AND RESERVA_ESTADO = 'REC')  
-- EGRESO
BEGIN 
   
	UPDATE CONTROL_ZETA.ESTADIA
	  SET EST_FECHA_HASTA = @FECHA
	 WHERE EST_RESERVA_ID = @RESERVA_ID
	 
	 UPDATE CONTROL_ZETA.RESERVA
		  SET  RESERVA_ESTADO = 'RFSF'
		 WHERE RESERVA_ID = @RESERVA_ID 

END
ELSE IF @CODIGO_IN_OUT = 2
	SET @CODIGO = 6
END


---FACTURAR ESTADIA
CREATE FUNCTION CONTROL_ZETA.FN_BUSCA_FACTURA()
returns NUMERIC
AS
BEGIN
RETURN (SELECT MAX(FACTURA_NRO)+1 FROM CONTROL_ZETA.FACTURA)
END

CREATE PROCEDURE CONTROL_ZETA.SP_REALIZAR_FACTURACION (@RESERVA_ID NUMERIC(18,0) ,
                                                       @FORMAPAGO VARCHAR(2),
													   @NROTARJETA INT,
 													   @COD_VERIF INT,
 													   @FECHA_VENC DATETIME,
 													   @NRO_CUOTAS TINYINT,
													   @CODIGO TINYINT OUTPUT,
													   @FACTURA_NRO NUMERIC OUTPUT)
AS
BEGIN

DECLARE @ESTADIA_ID   NUMERIC
DECLARE @DIAS_RESERVA INT
DECLARE @DIAS_ESTADIA INT
DECLARE @TIPO_REGIMEN TINYINT
DECLARE @NRO_FACT     NUMERIC
DECLARE @TOTAL_NOCHE  NUMERIC


BEGIN TRANSACTION
IF EXISTS (SELECT 1 FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID = @RESERVA_ID AND R.RESERVA_ESTADO ='RFSF')
BEGIN


SELECT @DIAS_RESERVA = DATEDIFF (DAY,R.RESERVA_FECHA_INICIO,R.RESERVA_FECHA_HASTA),
       @DIAS_ESTADIA = DATEDIFF (DAY,E.EST_FECHA_DESDE ,E.EST_FECHA_HASTA),
       @ESTADIA_ID = E.EST_ID, 
       @TIPO_REGIMEN = CASE WHEN UPPER(RG.REG_DESCRIPCION) = 'ALL INCLUSIVE' THEN 1
        WHEN UPPER(RG.REG_DESCRIPCION) = 'ALL INCLUSIVE MODERADO' THEN 2
        ELSE 3 END,
       @TOTAL_NOCHE = R.RES_PRECIO_TOTAL 
FROM CONTROL_ZETA.RESERVA R, 
     CONTROL_ZETA.ESTADIA E,
     CONTROL_ZETA.REGIMEN RG
WHERE R.RESERVA_ID = @RESERVA_ID
  AND R.RESERVA_ESTADO IN ('RFSF')
  AND R.RESERVA_ID = E.EST_RESERVA_ID
  AND R.RESERVA_ID_REGIMEN = RG.REG_ID

SET @NRO_FACT = CONTROL_ZETA.FN_BUSCA_FACTURA(); 
SET @FACTURA_NRO=@NRO_FACT

INSERT INTO CONTROL_ZETA.FACTURA 
(FACTURA_NRO, FACTURA_FECHA,FACTURA_FORMA_PAGO,FACTURA_TARJ_NRO, FACTURA_TARJ_COD_SEG, EST_ID, FACTURA_TARJ_FECHA_VENCIMIENTO, FACTURA_TARJ_NRO_CUOTAS)
VALUES
(@NRO_FACT, GETDATE(),@FORMAPAGO,@NROTARJETA,@COD_VERIF,@ESTADIA_ID, @FECHA_VENC, @NRO_CUOTAS)

IF (@DIAS_RESERVA =  @DIAS_ESTADIA )
BEGIN

INSERT INTO CONTROL_ZETA.ITEM_FACTURA (FACTURA_NRO,ITEM_FACTURA_CANTIDAD,ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
VALUES
(@NRO_FACT, 1, @TOTAL_NOCHE * @DIAS_RESERVA, 'COSTO POR RESERVA')

END
ELSE IF (@DIAS_RESERVA > @DIAS_ESTADIA )
BEGIN

INSERT INTO CONTROL_ZETA.ITEM_FACTURA (FACTURA_NRO,ITEM_FACTURA_CANTIDAD,ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
VALUES
(@NRO_FACT, 1, @TOTAL_NOCHE * @DIAS_ESTADIA, 'COSTO POR RESERVA')

INSERT INTO CONTROL_ZETA.ITEM_FACTURA (FACTURA_NRO,ITEM_FACTURA_CANTIDAD,ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
VALUES
(@NRO_FACT, 1, @TOTAL_NOCHE * (@DIAS_RESERVA - @DIAS_ESTADIA), 'COSTO POR ESTADIA') -- DIAS QUE ME FUI ANTES

END


INSERT INTO CONTROL_ZETA.ITEM_FACTURA (FACTURA_NRO,
                                       ITEM_FACTURA_CANTIDAD,
                                       ITEM_FACTURA_MONTO, 
                                       ITEM_DESCRIPCION,
                                       ITEM_ID_EST_CON)
SELECT @NRO_FACT, 
       EHC.CANTIDAD , 
       EHC.CANTIDAD*C.CON_PRECIO AS ITEM_FACTURA_MONTO, 
       C.CON_DESCRIPCION + ' - HABITACION: ' + CAST(H.HAB_NRO AS CHAR(5) ) AS 'CONSUMIBLES',
       EHC.EST_HAB_CON_ID
FROM CONTROL_ZETA.ESTADIA_HAB_CON EHC, 
     CONTROL_ZETA.CONSUMIBLE C, 
     CONTROL_ZETA.HABITACION H
WHERE EHC.EST_ID = @ESTADIA_ID
AND EHC.CON_ID = C.CON_ID
AND EHC.HAB_ID = H.HAB_ID
  GROUP BY  C.CON_PRECIO,EHC.CANTIDAD,C.CON_DESCRIPCION,EHC.EST_HAB_CON_ID, H.HAB_NRO

IF @TIPO_REGIMEN = 1 -- ALL INCLUSIVE
BEGIN
	INSERT INTO CONTROL_ZETA.ITEM_FACTURA 
	(FACTURA_NRO,ITEM_FACTURA_CANTIDAD,ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
	SELECT @NRO_FACT, 1,  -SUM(C.CON_PRECIO), 'DESCUENTO ALL INCLUSIVE'
	FROM CONTROL_ZETA.ESTADIA_HAB_CON EH, CONTROL_ZETA.CONSUMIBLE C
	WHERE EH.EST_ID = @ESTADIA_ID
	AND EH.CON_ID = C.CON_ID
	GROUP BY C.CON_PRECIO

END
ELSE IF @TIPO_REGIMEN = 2 -- REGIMEN ALL MODERADO
BEGIN
	INSERT INTO CONTROL_ZETA.ITEM_FACTURA 
	(FACTURA_NRO,ITEM_FACTURA_CANTIDAD,ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
	SELECT @NRO_FACT, 1,  SUM(C.CON_PRECIO), 'DESCUENTO ALL INCLUSIVE MODERADO'
	FROM CONTROL_ZETA.ESTADIA_HAB_CON EH, CONTROL_ZETA.CONSUMIBLE C
	WHERE EH.EST_ID = @ESTADIA_ID
	AND EH.CON_ID = C.CON_ID
	AND C.CON_ES_MODERADO = 'N'
END
 

UPDATE CONTROL_ZETA.FACTURA 
  SET CONTROL_ZETA.FACTURA.FACTURA_TOTAL =
 (select SUM(IFA.item_factura_monto) AS MONTO
 from CONTROL_ZETA.ITEM_FACTURA IFA
 where IFA.factura_nro = CONTROL_ZETA.FACTURA.factura_nro)
 WHERE FACTURA_NRO = @NRO_FACT -- 89603
 
 UPDATE CONTROL_ZETA.RESERVA
	  SET  RESERVA_ESTADO = 'RI'
	 WHERE RESERVA_ID = @RESERVA_ID 


SET @CODIGO = 1 -- OK, FACTURADO

COMMIT
END 

ELSE
BEGIN 
 SET @CODIGO = 6 -- FALLO EN LA FACTURACION
END 

END 









---ESTADISTICA

-- ADECUAR A TRIMESTRES



--@P_CURSOR CURSOR VARYING OUTPUT
-- HOTELES CON MAYOR CANTIDAD DE RESERVAS CANCELADAS

CREATE PROCEDURE CONTROL_ZETA.SP_ESTADISTICAS (@CODIGO_LISTADO TINYINT,
                                               @FECHA DATETIME
                                               )

AS
BEGIN

-- HOTELES CON MAYOR CANTIDAD DE RESERVAS CANCELADAS
IF @CODIGO_LISTADO = 1
BEGIN
SELECT TOP 5  R.RESERVA_ID_HOTEL,H.HOTEL_PAIS, H.HOTEL_CALLE,
       H.HOTEL_NRO_CALLE, COUNT(*) CANTIDAD
  FROM CONTROL_ZETA.RESERVA R, CONTROL_ZETA.HOTEL H
 WHERE R.RESERVA_ESTADO = 'RI'
  AND R.RESERVA_ID_HOTEL = H.HOTEL_ID
  AND R.RESERVA_FECHA_INICIO BETWEEN @FECHA AND DATEADD(MONTH,3, @FECHA)
 GROUP BY R.RESERVA_ID_HOTEL , H.HOTEL_PAIS, H.HOTEL_CALLE,H.HOTEL_NRO_CALLE
 ORDER BY COUNT(*) DESC, R.RESERVA_ID_HOTEL
END

-- HOTELES CON MAYOR CONSUMIBLES FACTURADOS
IF @CODIGO_LISTADO = 2
BEGIN
SELECT TOP 5  COUNT(*) CANT, H.HOTEL_NOMBRE, H.HOTEL_CALLE, H.HOTEL_NRO_CALLE
FROM CONTROL_ZETA.FACTURA F, 
     CONTROL_ZETA.ITEM_FACTURA IFA, 
     CONTROL_ZETA.ESTADIA E, 
     CONTROL_ZETA.RESERVA R,
     CONTROL_ZETA.HOTEL H
WHERE F.FACTURA_NRO = IFA.FACTURA_NRO
  AND F.EST_ID = E.EST_ID   
  AND IFA.ITEM_DESCRIPCION = 'CONSUMIBLES'
  AND E.EST_RESERVA_ID = R.RESERVA_ID
  AND R.RESERVA_ID_HOTEL = H.HOTEL_ID
  AND R.RESERVA_FECHA_INICIO BETWEEN @FECHA AND DATEADD(MONTH,3, @FECHA)
GROUP BY  ITEM_FACTURA_CANTIDAD, H.HOTEL_CALLE, H.HOTEL_NRO_CALLE, H.HOTEL_NOMBRE
ORDER BY COUNT(*) DESC
END


-- HOTEL CON MAYOR CANTIDAD DE DIAS FUERA DE SERVICIO
IF @CODIGO_LISTADO = 3
BEGIN
SELECT TOP 5 SUM(DATEDIFF (DAY, HC.HOTEL_C_FECHA_DESDE, HC.HOTEL_C_FECHA_HASTA)) CANT,
        H.HOTEL_NOMBRE, H.HOTEL_CALLE, H.HOTEL_NRO_CALLE
FROM CONTROL_ZETA.HOTEL_CIERRE HC, 
     CONTROL_ZETA.HOTEL H
WHERE HC.HOTEL_C_ID = H.HOTEL_ID   
  AND HC.HOTEL_C_FECHA_DESDE BETWEEN @FECHA AND DATEADD(MONTH,3, @FECHA)
GROUP BY H.HOTEL_NOMBRE, H.HOTEL_CALLE, H.HOTEL_NRO_CALLE
ORDER BY SUM(DATEDIFF (DAY, HC.HOTEL_C_FECHA_DESDE, HC.HOTEL_C_FECHA_HASTA)) DESC
END

IF @CODIGO_LISTADO = 4
BEGIN
SELECT TOP 5 COUNT(*) , H.HAB_ID, H.HAB_ID_HOTEL
FROM CONTROL_ZETA.HABITACION H,
     CONTROL_ZETA.RESERVA_HABITACION RH,
     CONTROL_ZETA.RESERVA R
WHERE H.HAB_ID = RH.HAB_ID
  AND RH.RESERVA_ID = R.RESERVA_ID
  AND R.RESERVA_ESTADO = 'RI'
  AND R.RESERVA_FECHA_INICIO BETWEEN @FECHA AND DATEADD(MONTH,3, @FECHA)
  GROUP BY  H.HAB_ID, H.HAB_ID_HOTEL
  ORDER BY COUNT(*) DESC, H.HAB_ID_HOTEL, H.HAB_ID
END

IF @CODIGO_LISTADO = 5
BEGIN
SELECT TOP 5 RES.CLIENTE_ID, SUM(RES.PUNTOS) TOTAL_DE_PUNTOS
FROM (
SELECT SUM(CASE WHEN ITEM_DESCRIPCION = 'COSTO POR ESTADIA' OR 
                 ITEM_DESCRIPCION = 'COSTO POR REGIMEN'
            THEN (ITEM_FACTURA_MONTO/10)
       ELSE 
            (ITEM_FACTURA_MONTO/5)
       END ) AS PUNTOS,
       ITF.ITEM_DESCRIPCION, ITF.ITEM_FACTURA_MONTO,  R.CLIENTE_ID
FROM CONTROL_ZETA.ITEM_FACTURA ITF,
     CONTROL_ZETA.FACTURA F,
     CONTROL_ZETA.ESTADIA E,
     CONTROL_ZETA.RESERVA R 
WHERE ITF.FACTURA_NRO = F.FACTURA_NRO
  AND F.EST_ID = E.EST_ID
  AND E.EST_RESERVA_ID = R.RESERVA_ID
  AND R.RESERVA_FECHA_INICIO BETWEEN @FECHA AND DATEADD(MONTH,3, @FECHA)
GROUP BY R.CLIENTE_ID, ITF.ITEM_DESCRIPCION, ITF.ITEM_FACTURA_MONTO) RES
GROUP BY RES.CLIENTE_ID
ORDER BY SUM(RES.PUNTOS) DESC, RES.CLIENTE_ID
END

END

--SELECT  DATEADD(MONTH,3, GETDATE())
