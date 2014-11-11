----------------------------------
-------PROCEDURES/FUNCIONES-------
----------------------------------

-------------
------ROL----
-------------

CREATE PROCEDURE CONTROL_ZETA.SP_ABM_ROL (@accion SMALLINT,@id_rol TINYINT, @nombre varchar(20),@estado varchar(1), @id_rol_new TINYINT output,@error tinyint output)
AS
--@Desc:ABM de Rol
BEGIN
IF (@accion=1)
BEGIN
--Alta
	IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.ROL WHERE ROL_NOMBRE=@nombre)
	BEGIN
		INSERT INTO CONTROL_ZETA.ROL (ROL_NOMBRE, ROL_ESTADO)VALUES(@nombre,@estado)
		SET @id_rol=SCOPE_IDENTITY()
		SET @error=1
	END;
	ELSE
	SET @error=3
END;
ELSE IF @accion=2
BEGIN
--Modificacion
	IF EXISTS (SELECT * FROM CONTROL_ZETA.ROL WHERE ROL_ID=@id_rol)
	BEGIN
		IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.ROL WHERE ROL_NOMBRE=@nombre)
		BEGIN
			UPDATE CONTROL_ZETA.ROL SET ROL_NOMBRE=@nombre,ROL_ESTADO=@estado where ROL_ID=@id_rol
			DELETE CONTROL_ZETA.ROL_FUNC WHERE ROL_ID = @id_rol
			SET @error=1
		END;
		ELSE
		SET @error=3
	END
	ELSE
		SET @error=2
	
END
ELSE IF @accion=3
BEGIN
--Baja
IF EXISTS (SELECT * FROM CONTROL_ZETA.ROL WHERE ROL_ID=@id_rol)
	BEGIN
		UPDATE CONTROL_ZETA.ROL SET ROL_ESTADO='I' where ROL_ID=@id_rol
		SET @error=1
	END
	ELSE
		SET @error=2
END
END;

GO

CREATE PROCEDURE CONTROL_ZETA.SP_ROL_FUNC(@rol_id TINYINT, @func_id TINYINT, @error tinyint output)
AS
--@Desc:Crea relacion de Rol y funcionalidad
BEGIN
	
	BEGIN
		IF NOT EXISTS(SELECT * FROM CONTROL_ZETA.ROL_FUNC WHERE ROL_ID = @rol_id and FUNC_ID=@func_id)
		BEGIN
			INSERT INTO CONTROL_ZETA.ROL_FUNC(ROL_ID,FUNC_ID) VALUES (@rol_id,@func_id)
			set @error=1
		END
		ELSE
			set @error=2
	END
END

GO
----------------
----HOTEL-------
----------------


CREATE FUNCTION CONTROL_ZETA.get_id_pais(@pais varchar(50))
returns tinyint
AS
BEGIN
RETURN (SELECT P.PAIS_ID FROM CONTROL_ZETA.PAIS P WHERE P.PAIS_DETALLE=@pais)
END

GO
---

CREATE FUNCTION CONTROL_ZETA.hay_reservas_regimen(@id_regimen TINYINT, @fe_sist DATE, @id_hotel int)
returns numeric
AS
BEGIN
RETURN (SELECT COUNT(R.RESERVA_ID_REGIMEN) 
FROM CONTROL_ZETA.RESERVA R 
WHERE R.RESERVA_ID_REGIMEN=@id_regimen AND 
R.RESERVA_ESTADO IN ('RC','RM','RI') AND 
R.RESERVA_FECHA_HASTA >@fe_sist AND R.RESERVA_ID_HOTEL=@id_hotel )

END

GO
---

CREATE FUNCTION CONTROL_ZETA.hay_reservas_fechas(@fe_inicio_cierre date, @fe_fin_cierre date, @id_hotel int)
returns tinyint
AS
BEGIN
RETURN (SELECT COUNT (R.RESERVA_ID) 
FROM CONTROL_ZETA.RESERVA R 
WHERE ((R.RESERVA_FECHA_INICIO between @fe_inicio_cierre AND @fe_fin_cierre) 
OR (R.RESERVA_FECHA_HASTA between @fe_inicio_cierre AND @fe_fin_cierre))
AND R.RESERVA_ID_HOTEL=@id_hotel 
AND R.RESERVA_ESTADO IN ('RC','RM','REC'))
END

GO


CREATE PROCEDURE CONTROL_ZETA.SP_AM_HOTEL(@accion tinyint,@id_hotel int,@nombre VARCHAR(100), @mail VARCHAR(50), @tel VARCHAR(10),@calle  VARCHAR(50), @nro_calle SMALLINT, @loc VARCHAR(50), @cant_est TINYINT, @pais VARCHAR(50), @rec_estrella int, @usr VARCHAR(50),@fe_sist date,@error tinyint OUTPUT,@id_hotel_new int OUTPUT)
AS
--@Desc: Alta y modificacion de hotel
BEGIN
DECLARE
@id_loc tinyint,
@id_pais tinyint

set @id_loc=CONTROL_ZETA.get_ciudad(@loc)
set @id_pais=CONTROL_ZETA.get_id_pais(@pais)
IF (@accion=1)
BEGIN
--Alta
	IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HOTEL H WHERE (H.HOTEL_NOMBRE=@nombre) OR (H.HOTEL_CALLE=@calle AND H.HOTEL_NRO_CALLE= @nro_calle AND H.HOTEL_ID_LOC=@id_loc AND H.HOTEL_PAIS=@id_pais))
	BEGIN
		INSERT INTO CONTROL_ZETA.HOTEL(HOTEL_NOMBRE,HOTEL_CALLE,HOTEL_NRO_CALLE,HOTEL_ID_LOC,HOTEL_PAIS,HOTEL_TEL,	 HOTEL_RECARGA_ESTRELLA,HOTEL_CANT_ESTRELLA,HOTEL_FECHA_CREACION,HOTEL_MAIL)
		VALUES(@nombre,@calle,@nro_calle,@id_loc,@id_pais, @tel, @rec_estrella,@cant_est,@fe_sist,@mail)
		set @id_hotel_new=SCOPE_IDENTITY()
		
		INSERT INTO CONTROL_ZETA.USR_ROL_HOTEL(USR_USERNAME,ROL_ID,HOTEL_ID) VALUES(@usr,1,@id_hotel_new)
		
		SET @error=1
	END
	ELSE
	SET @error=3
END
ELSE IF @accion=2
BEGIN
--Modificacion
	IF EXISTS(SELECT * FROM CONTROL_ZETA.HOTEL H WHERE H.HOTEL_ID=@id_hotel)
	BEGIN
		IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HOTEL H WHERE H.HOTEL_ID!=@id_hotel and((H.HOTEL_NOMBRE=@nombre) OR (H.HOTEL_CALLE=@calle AND H.HOTEL_NRO_CALLE= @nro_calle AND H.HOTEL_ID_LOC=@id_loc AND H.HOTEL_PAIS=@id_pais))	)
		BEGIN
			UPDATE CONTROL_ZETA.HOTEL 
			SET HOTEL_NOMBRE=@nombre, 
			HOTEL_CALLE=@calle, 
			HOTEL_NRO_CALLE=@nro_calle, 
			HOTEL_ID_LOC=@id_loc, 
			HOTEL_PAIS=@id_pais, 
			HOTEL_MAIL=@mail, 
			HOTEL_CANT_ESTRELLA=@cant_est, 
			HOTEL_RECARGA_ESTRELLA=@rec_estrella, 
			HOTEL_TEL=@tel
			WHERE HOTEL_ID=@id_hotel
			SET @error=1
		END
		ELSE
		SET @error=3
	END
	ELSE
	SET @error=2
	END
	
END


GO


CREATE PROCEDURE CONTROL_ZETA.SP_REGIMEN_HOTEL(@accion tinyint,@id_hotel int, @id_regimen TINYINT, @fe_sist date,@error TINYINT OUTPUT)
AS
--@Desc: Relacion hotel y regimen
BEGIN
IF @accion=1 --Alta
BEGIN
	IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HOTEL_REGIMEN HR WHERE HR.HOTEL_ID=@id_hotel AND HR.REG_ID=@id_regimen)
	BEGIN
		INSERT INTO CONTROL_ZETA.HOTEL_REGIMEN(HOTEL_ID,REG_ID,REG_ESTADO) VALUES(@id_hotel,@id_regimen,'H')
	SET @error=1
	END
	ELSE IF EXISTS (SELECT * FROM CONTROL_ZETA.HOTEL_REGIMEN HR WHERE HR.HOTEL_ID=@id_hotel AND HR.REG_ID=@id_regimen AND HR.REG_ESTADO='I')
	BEGIN
		UPDATE CONTROL_ZETA.HOTEL_REGIMEN SET REG_ESTADO='H' WHERE HOTEL_ID=@id_hotel AND REG_ID=@id_regimen
		SET @error=1
	END
	ELSE
	SET @error=3
END
ELSE IF @accion=3
BEGIN
	IF (CONTROL_ZETA.hay_reservas_regimen(@id_regimen,@fe_sist,@id_hotel)=0)
	BEGIN
		UPDATE CONTROL_ZETA.HOTEL_REGIMEN SET REG_ESTADO='I' WHERE HOTEL_ID=@id_hotel AND REG_ID=@id_regimen
		SET @error=1
	END
	ELSE
	SET @error=5
END
END;

GO


CREATE PROCEDURE CONTROL_ZETA.SP_CIERRE_HOTEL(@fe_inicio_cierre date, @fe_fin_cierre date, @id_hotel int, @motivo varchar(100), @error tinyint OUTPUT)
AS
--@Desc: Cierre de hotel
BEGIN
	IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HOTEL_CIERRE WHERE HOTEL_ID=@id_hotel AND ((@fe_inicio_cierre>=HOTEL_C_FECHA_DESDE AND @fe_inicio_cierre<=HOTEL_C_FECHA_HASTA) OR (@fe_fin_cierre>=HOTEL_C_FECHA_DESDE AND @fe_fin_cierre<=HOTEL_C_FECHA_HASTA)))
	BEGIN
		IF CONTROL_ZETA.hay_reservas_fechas(@fe_inicio_cierre, @fe_fin_cierre ,@id_hotel )=0
		BEGIN
			INSERT INTO CONTROL_ZETA.HOTEL_CIERRE(HOTEL_ID,HOTEL_C_MOTIVO,HOTEL_C_FECHA_DESDE,HOTEL_C_FECHA_HASTA) 
			VALUES (@id_hotel,@motivo,@fe_inicio_cierre,@fe_fin_cierre)
			SET @error=1
		END
		ELSE 
			SET @error=6
END
ELSE
	SET @error=3		
END

GO

------------------
----HABITACION----
------------------

CREATE PROCEDURE CONTROL_ZETA.SP_ABM_HABITACION(@accion tinyint,@nro_hab smallint,@id_hab numeric, @hab_piso SMALLINT,@ubi_hab varchar(70),@obs varchar(150),@id_hotel int, @id_tipo_hab smallint, @error tinyint output,@id_hab_new numeric output)
AS
--@Desc: Realiza ABM de habitacion
BEGIN
IF @accion=1
BEGIN
--Alta
	IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HABITACION H WHERE H.HAB_NRO=@nro_hab AND H.HAB_ID_HOTEL=@id_hotel)
	BEGIN
		INSERT INTO CONTROL_ZETA.HABITACION(HAB_NRO,HAB_ID_HOTEL,HAB_PISO,HAB_ID_TIPO,HAB_UBI_HOTEL,HAB_OBSERVACION,HAB_ESTADO)
		VALUES(@nro_hab,@id_hotel,@hab_piso,@id_tipo_hab,@ubi_hab,@obs,'H')
		set @id_hab_new=scope_identity()
		set @error=1
	END
	ELSE
		set @error=3
END		
ELSE IF @accion=2
BEGIN
--Modificacion
	IF EXISTS (SELECT * FROM CONTROL_ZETA.HABITACION H WHERE H.HAB_ID=@id_hab)
	BEGIN
		IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HABITACION H WHERE H.HAB_NRO=@nro_hab AND H.HAB_ID_HOTEL=@id_hotel)
		BEGIN
			UPDATE CONTROL_ZETA.HABITACION 
			SET HAB_ID_HOTEL=@id_hotel,
			HAB_NRO=@nro_hab,
			HAB_OBSERVACION=@obs,
			HAB_PISO=@hab_piso,
			HAB_UBI_HOTEL=@ubi_hab 
			WHERE HAB_ID=@id_hab
			set @error=1
		END;
		ELSE
		set @error=3
	END
	ELSE
	set @error=-2
	
END
ELSE IF @accion=2
BEGIN
--Baja
IF EXISTS (SELECT * FROM CONTROL_ZETA.HABITACION H WHERE H.HAB_ID=@id_hab)
	BEGIN
		UPDATE CONTROL_ZETA.HABITACION 
		SET HAB_ESTADO='I'	WHERE HAB_ID=@id_hab
		
		set @error=1
	END
	ELSE
	set @error=2
END;
END;

GO
--------------
---RESERVAS---
--------------


CREATE FUNCTION CONTROL_ZETA.fe_res_consistente(@fe_desde date, @fe_hasta date, @fe_sist date)
returns tinyint
AS
--@Desc:Verifica consistencia en las fechas de reserva
BEGIN
DECLARE
@error tinyint=0
	
	IF (@fe_desde<@fe_sist)
	SET @error=1;
	
	IF (@fe_desde>=@fe_hasta)
	SET @error=1;
RETURN(@error)

END;

GO


CREATE FUNCTION CONTROL_ZETA.hotel_cerrado(@fe_desde date, @fe_hasta date, @id_hotel int)
returns tinyint
AS
BEGIN
RETURN(SELECT COUNT (*) 
		FROM CONTROL_ZETA.HOTEL_CIERRE HC 
		WHERE HC.HOTEL_ID=@id_hotel 
		AND ((@fe_desde between HC.HOTEL_C_FECHA_DESDE AND HC.HOTEL_C_FECHA_HASTA )
			OR
			(@fe_hasta between HC.HOTEL_C_FECHA_DESDE AND HC.HOTEL_C_FECHA_HASTA)))
END

GO


CREATE PROCEDURE CONTROL_ZETA.SP_CANC_RCNS(@fe_sistema date)
AS
--@Desc: Realiza limpieza por no show
BEGIN
	update CONTROL_ZETA.RESERVA set RESERVA_ESTADO ='RCNS' 
	WHERE RESERVA_FECHA_INICIO<@fe_sistema 
	AND RESERVA_ESTADO IN('RC','RM')
END; 

GO


CREATE FUNCTION CONTROL_ZETA.get_id_reserva_new()
returns numeric
AS
--@Desc: Se obtiene el numero de id siguiente de reserva
BEGIN

RETURN(SELECT (MAX(RESERVA_ID)+1) FROM CONTROL_ZETA.RESERVA)

END;

GO


CREATE FUNCTION CONTROL_ZETA.OBTENER_PRECIO_HAB(@id_regimen TINYINT,@id_hotel int,@id_tipo_hab SMALLINT)
RETURNS NUMERIC
AS
BEGIN
	return(SELECT top (1)(RG.REG_PRECIO *  TH.TIPO_HAB_PORC *TH.TIPO_HAB_CANT_PERSONAS) 
			FROM CONTROL_ZETA.RESERVA_HABITACION RH,
			CONTROL_ZETA.TIPO_HAB TH,
			CONTROL_ZETA.HOTEL HT,
			CONTROL_ZETA.REGIMEN RG
			WHERE RG.REG_ID=@id_regimen AND
			HT.HOTEL_ID=@id_hotel AND
			TH.TIPO_HAB_ID=@id_tipo_hab)	 
    
END     

GO



CREATE FUNCTION CONTROL_ZETA.SP_PRECIO_TOTAL(@criterio tinyint,@fe_desde date,@fe_hasta date,@id_res numeric,@id_hotel int)
RETURNS numeric
AS
BEGIN
DECLARE
@PRECIO NUMERIC,
@REC_EST int,
@CANT_EST tinyint,
@P_FINAL NUMERIC

	SET @CANT_EST  = (SELECT TOP(1) HT.HOTEL_CANT_ESTRELLA 
					  FROM HOTEL HT 
					  WHERE HT.HOTEL_ID=@id_hotel)
	SET @REC_EST = (SELECT HT.HOTEL_RECARGA_ESTRELLA 
					FROM HOTEL HT 
					WHERE HT.HOTEL_ID=@id_hotel)
	
SET @PRECIO=((@REC_EST * @CANT_EST ) + (SELECT SUM(TRP.PRECIO_TEMP) 
									   FROM CONTROL_ZETA.TEMP_RESERVA_PEDIDO TRP
									   WHERE TRP.RESERVA_ID=@id_res))

IF @criterio=0 --Por noche 
	SET @P_FINAL=@PRECIO
else if @criterio=1 --Por estadia
	SET @P_FINAL=(@PRECIO*DATEDIFF (DAY,@fe_desde,@fe_hasta))
	
RETURN(@P_FINAL)

END;

GO


CREATE PROCEDURE CONTROL_ZETA.SP_VERIFICA_DISPONIBILIDAD(@id_res NUMERIC,@hotel_id int,@fe_desde date,@fe_hasta date,@cant_hab tinyint,@fe_sist date,@id_tipo_hab SMALLINT,@id_regimen TINYINT, @res smallint output, @id_res_new_temp numeric output)
AS
----@Desc: Verifica disponibilidad
BEGIN
DECLARE
@DISP INT,
@PRECIO NUMERIC,
@RESERVA NUMERIC

	EXEC CONTROL_ZETA.SP_CANC_RCNS @fe_sistema=@fe_sist
	
	IF (CONTROL_ZETA.fe_res_consistente(@fe_desde,@fe_hasta,@fe_sist)=0)
	BEGIN
		IF (CONTROL_ZETA.hotel_cerrado(@fe_desde,@fe_hasta,@hotel_id)=0)
		BEGIN
			IF @id_res IS NOT NULL
			BEGIN
			--Es Modif: Ya existe reserva
				SET @RESERVA=@id_res
				SET @disp =(SELECT COUNT(DISTINCT H.HAB_ID)
						FROM CONTROL_ZETA.HABITACION H 
						WHERE H.HAB_ID_HOTEL=@hotel_id AND
						H.HAB_ID_TIPO=@id_tipo_hab AND
						(H.HAB_ID NOT IN (SELECT RH.HAB_ID 
										  FROM CONTROL_ZETA.RESERVA_HABITACION RH 
										  WHERE RH.RESERVA_ID IN (SELECT R.RESERVA_ID 
																  FROM CONTROL_ZETA.RESERVA R 
																  WHERE 
																  R.RESERVA_ID!=@id_res AND
																  R.RESERVA_FECHA_INICIO=@fe_desde AND 
																  R.RESERVA_FECHA_HASTA=@fe_hasta AND
																  R.RESERVA_ESTADO IN('RI','RC','RM')
																  ))OR													
						H.HAB_ID IN (SELECT DISTINCT(EHC.HAB_ID) 
						FROM CONTROL_ZETA.ESTADIA_HAB_CON EHC, CONTROL_ZETA.ESTADIA E 
						WHERE EHC.EST_ID=E.EST_ID AND
						E.EST_FECHA_HASTA IS NOT NULL
						AND E.EST_FECHA_HASTA<@fe_desde)))
			
			END
			ELSE
			BEGIN
			--Es Alta
				SET @RESERVA=CONTROL_ZETA.get_id_reserva_new()
				SET @disp =(SELECT COUNT(DISTINCT H.HAB_ID)
						FROM CONTROL_ZETA.HABITACION H 
						WHERE H.HAB_ID_HOTEL=@hotel_id AND
						H.HAB_ID_TIPO=@id_tipo_hab AND
						(H.HAB_ID NOT IN (SELECT RH.HAB_ID 
										  FROM CONTROL_ZETA.RESERVA_HABITACION RH 
										  WHERE RH.RESERVA_ID IN (SELECT R.RESERVA_ID 
																  FROM CONTROL_ZETA.RESERVA R 
																  WHERE 
																  --R.RESERVA_ID!=@id_res AND
																  R.RESERVA_FECHA_INICIO=@fe_desde AND 
																  R.RESERVA_FECHA_HASTA=@fe_hasta AND
																  R.RESERVA_ESTADO IN('RI','RC','RM')
																  ))OR													
						H.HAB_ID IN (SELECT DISTINCT(EHC.HAB_ID) 
						FROM CONTROL_ZETA.ESTADIA_HAB_CON EHC, CONTROL_ZETA.ESTADIA E 
						WHERE EHC.EST_ID=E.EST_ID AND
						E.EST_FECHA_HASTA IS NOT NULL
						AND E.EST_FECHA_HASTA<@fe_desde)))
			END
		END
		ELSE
		SET @RES=(-2)
	END
	ELSE 
		SET @RES=(-1)
	IF @DISP>=@cant_hab 
	BEGIN
		SET @RES=1
			
			set @PRECIO=CONTROL_ZETA.OBTENER_PRECIO_HAB(@id_regimen,@hotel_id ,@id_tipo_hab )
			INSERT INTO CONTROL_ZETA.TEMP_RESERVA_PEDIDO(RESERVA_ID,HAB_ID,PRECIO_TEMP)
			SELECT TOP (@cant_hab) @RESERVA,H.HAB_ID, @PRECIO
			FROM CONTROL_ZETA.HABITACION H 
			WHERE H.HAB_ID_TIPO=@id_tipo_hab AND 
			H.HAB_ID_HOTEL=@hotel_id;
			set @id_res_new_temp=@RESERVA
	END
	else SET @RES=0
END
GO


CREATE PROCEDURE CONTROL_ZETA.SP_ALTA_RESERVA(@hotel_id int,@fe_desde date,@fe_hasta date,@tipo_reg_id TINYINT,@cliente_id numeric,@id_usr varchar(50),@fe_sist date,@id_reserva numeric OUTPUT,@error tinyint OUTPUT)
AS
--@Desc: Realiza el alta de reserva
BEGIN
DECLARE
@PRECIO_TOTAL NUMERIC
BEGIN TRANSACTION
	IF (CONTROL_ZETA.fe_res_consistente(@fe_desde,@fe_hasta,@fe_sist)=0)
	BEGIN
		SET @id_reserva=CONTROL_ZETA.get_id_reserva_new()
		SET @PRECIO_TOTAL = CONTROL_ZETA.SP_PRECIO_TOTAL(0,@fe_desde,@fe_hasta,@id_reserva,@hotel_id)
		
		INSERT INTO CONTROL_ZETA.RESERVA (RESERVA_ID,RESERVA_FECHA,RESERVA_FECHA_INICIO, RESERVA_FECHA_HASTA,RESERVA_ID_REGIMEN, RESERVA_ID_HOTEL, RESERVA_ESTADO, CLIENTE_ID, USR_USERNAME,RES_PRECIO_TOTAL)
		VALUES(@id_reserva,@fe_sist,@fe_desde,@fe_hasta,@tipo_reg_id,@hotel_id,'RC',@cliente_id,@id_usr,@PRECIO_TOTAL);
		
		IF ((SELECT COUNT(*) FROM CONTROL_ZETA.TEMP_RESERVA_PEDIDO) > 0)
		BEGIN
			INSERT INTO CONTROL_ZETA.RESERVA_HABITACION(RESERVA_ID,HAB_ID)
			SELECT TRP.RESERVA_ID,TRP.HAB_ID 
			FROM CONTROL_ZETA.TEMP_RESERVA_PEDIDO TRP
			WHERE TRP.RESERVA_ID=@id_reserva
		
			DELETE CONTROL_ZETA.TEMP_RESERVA_PEDIDO
		
			set @error=1
			COMMIT
		END
		ELSE
		BEGIN
			set @error=6
			ROLLBACK
		END
	END
	ELSE
	BEGIN
		set @error=5
		ROLLBACK
	END;
	
END;

GO


CREATE PROCEDURE CONTROL_ZETA.SP_MODIF_RESERVA(@hotel_id int,@fe_desde date,@fe_hasta date,@tipo_reg_id TINYINT,@cliente_id numeric,@id_usr varchar(50),@id_reserva numeric,@fe_sist date,@error tinyint OUTPUT)
AS
--@Desc: Modifica la reserva
BEGIN
DECLARE
@PRECIO_TOTAL NUMERIC

BEGIN TRANSACTION
	IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva)
	BEGIN
		IF ((SELECT R.RESERVA_FECHA_INICIO FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva)<@fe_desde)
		BEGIN
			SET @PRECIO_TOTAL = CONTROL_ZETA.SP_PRECIO_TOTAL(0,@fe_desde,@fe_hasta,@id_reserva,@hotel_id)
			
			UPDATE CONTROL_ZETA.RESERVA 
			SET RESERVA_FECHA=@fe_sist,
			RESERVA_FECHA_INICIO=@fe_desde, 
			RESERVA_FECHA_HASTA=@fe_hasta,
			RESERVA_ID_REGIMEN=@tipo_reg_id, 
			RESERVA_ID_HOTEL=@hotel_id, 
			RESERVA_ESTADO='RM', 
			CLIENTE_ID=@cliente_id,
			USR_USERNAME=@id_usr,
			RES_PRECIO_TOTAL=@PRECIO_TOTAL
			WHERE RESERVA_ID=@id_reserva
			
			DELETE CONTROL_ZETA.RESERVA_HABITACION WHERE RESERVA_ID=@id_reserva
			
			IF (SELECT COUNT(*) FROM CONTROL_ZETA.TEMP_RESERVA_PEDIDO) > 0
			BEGIN
				INSERT INTO CONTROL_ZETA.RESERVA_HABITACION(RESERVA_ID,HAB_ID)
				SELECT TRP.RESERVA_ID,TRP.HAB_ID 
				FROM CONTROL_ZETA.TEMP_RESERVA_PEDIDO TRP
				WHERE TRP.RESERVA_ID=@id_reserva
			
				DELETE CONTROL_ZETA.TEMP_RESERVA_PEDIDO
				
				set @error=1
			
				COMMIT
			END
			ELSE 
			BEGIN
				set @error=6
				ROLLBACK
			END
		END
		ELSE
		BEGIN
			set @error=5
			ROLLBACK
		END
	END	
	ELSE
	BEGIN
		set @error=2
		ROLLBACK
	END

END;


GO


CREATE PROCEDURE CONTROL_ZETA.SP_CANCELAR_RESERVA(@id_reserva numeric, @motivo varchar(150), @fecha_canc date, @id_usr varchar(50), @id_est varchar(4),@error tinyint OUTPUT)
AS
--@Desc:Cancela la reserva
BEGIN

BEGIN TRANSACTION
IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva AND R.RESERVA_FECHA_INICIO>=@fecha_canc)
	BEGIN
		IF EXISTS(SELECT * FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva AND RESERVA_ESTADO IN('RCC','RCR','RCNS'))
		BEGIN
			set @error=6 --Reserva ya cancela
			ROLLBACK TRANSACTION
		END
		ELSE
		BEGIN
			UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO=@id_est, RESERVA_MOTIVO_CANC=@motivo, RESERVA_FECHA_CANC=@fecha_canc,USR_USERNAME_CANC=@id_usr
			WHERE RESERVA_ID=@id_reserva
			set @error=1
			COMMIT
		END
	END
else
	BEGIN
	set @error=5
	ROLLBACK TRANSACTION
	END
END;

GO

CREATE PROCEDURE CONTROL_ZETA.LIMPIAR_PEDIDO
AS
BEGIN
DELETE CONTROL_ZETA.TEMP_RESERVA_PEDIDO
END;

GO


CREATE FUNCTION CONTROL_ZETA.get_id_habitacion(@nro_hab SMALLINT,@id_hotel int)
returns numeric
AS
--@Desc: Se obtiene el id de habitacion segun nro de habitacion y hotel
BEGIN

RETURN(SELECT H.HAB_ID 
FROM CONTROL_ZETA.HABITACION H 
WHERE H.HAB_NRO=@nro_hab AND H.HAB_ID_HOTEL=@id_hotel)
END;

GO


CREATE FUNCTION CONTROL_ZETA.get_id_estadia(@hab_id numeric)
returns numeric
AS
--@Desc: Se obtiene el id de estadia segun el id de habitacion
BEGIN

RETURN(SELECT E.EST_ID 
FROM CONTROL_ZETA.ESTADIA E, CONTROL_ZETA.RESERVA R,CONTROL_ZETA.RESERVA_HABITACION RH 
WHERE E.EST_RESERVA_ID=R.RESERVA_ID AND RH.RESERVA_ID=R.RESERVA_ID AND RH.HAB_ID=@hab_id)
END;
GO

--------------------------
--REGISTRAR CONSUMIBLE----
--------------------------


CREATE PROCEDURE CONTROL_ZETA.SP_REGISTRAR_CONSUMIBLE(@id_hotel int, @nro_hab SMALLINT, @id_con smallint, @id_est numeric,@cant tinyint, @error tinyint OUTPUT )
AS
--@Desc:Se registran los consumibles
BEGIN
DECLARE 
@i tinyint =1,
@id_hab numeric =CONTROL_ZETA.get_id_habitacion(@nro_hab,@id_hotel)
--@id_est numeric = 0

--set @id_est=CONTROL_ZETA.get_estadia(@id_hab)


	IF @id_hab>0
	BEGIN
		WHILE @i<=@cant
		BEGIN
			INSERT INTO CONTROL_ZETA.ESTADIA_HAB_CON (HAB_ID,CON_ID,EST_ID)
			VALUES (@id_hab,@id_con,@id_est)
			set @i=@i+1
		END
		set @error=1	
	END	
	ELSE set @error=5		
	
END
GO
---------------
----LOGIN------
---------------

CREATE PROCEDURE CONTROL_ZETA.LOGIN_USR(@usr varchar(50),@pass VARCHAR (100),@error tinyint output)
as
BEGIN

 
DECLARE 
@V_USR varchar(50),
@V_PASS VARCHAR (100),
@V_INTENTOS TINYINT,
@V_ESTADO VARCHAR(1)

DECLARE C_USR CURSOR FOR  
SELECT  U.USR_USERNAME,U.USR_PASS,U.USR_ESTADO,U.USR_INTENTOS 
FROM CONTROL_ZETA.USUARIO U 
WHERE U.USR_USERNAME=@usr

OPEN C_USR

FETCH NEXT FROM C_USR 
INTO @V_USR ,@V_PASS , @V_ESTADO ,@V_INTENTOS 

IF (@@CURSOR_ROWS!=0) 
BEGIN
	IF (@V_ESTADO ='H') 
	BEGIN
		IF (@V_INTENTOS<4)
		BEGIN
			IF(@V_PASS=@pass)
			begin
				SET @error=1 --PUDO INGRESAR!
				UPDATE CONTROL_ZETA.USUARIO SET USR_INTENTOS=0 WHERE USR_USERNAME=@usr
			end
			ELSE
			BEGIN
			    
				UPDATE CONTROL_ZETA.USUARIO SET USR_INTENTOS=@V_INTENTOS+1 WHERE USR_USERNAME=@usr
				SET @error=6 --Pass incorrecta
			END
		END
		ELSE set @error=5 --Ya hizo 3 intentos fallidos
	END
	ELSE SET @error=4 --Esta inhabilitado
END
ELSE SET @error=2 --No esta el usr

 CLOSE C_USR
 DEALLOCATE C_USR


END;

GO
CREATE PROCEDURE CONTROL_ZETA.SP_ACT_PRECIO_RES(@id_reserva numeric)
	AS
	BEGIN
		UPDATE CONTROL_ZETA.RESERVA SET RES_PRECIO_TOTAL=(SELECT (RG.REG_PRECIO *  TH.TIPO_HAB_PORC *TH.TIPO_HAB_CANT_PERSONAS) + (HT.HOTEL_RECARGA_ESTRELLA * HT.HOTEL_CANT_ESTRELLA )
			FROM CONTROL_ZETA.RESERVA_HABITACION RH,
			CONTROL_ZETA.TIPO_HAB TH,
			CONTROL_ZETA.HOTEL HT,
			CONTROL_ZETA.REGIMEN RG,
			CONTROL_ZETA.RESERVA R,
			CONTROL_ZETA.HABITACION H
			WHERE 
			R.RESERVA_ID=@id_reserva AND
			RG.REG_ID=R.RESERVA_ID_REGIMEN AND
			HT.HOTEL_ID=R.RESERVA_ID_HOTEL AND
			RH.RESERVA_ID=@id_reserva AND
			RH.HAB_ID=H.HAB_ID AND
			TH.TIPO_HAB_ID=H.HAB_ID_TIPO AND
			R.RESERVA_ESTADO IN ('RC','RM','REC')
			AND R.RES_PRECIO_TOTAL IS NULL) 
			WHERE RESERVA_ID=@id_reserva
			AND RES_PRECIO_TOTAL IS NULL
			
	END
GO
-------
-------
-------
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

GO

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

GO


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

GO
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

go
--------------------------
------REGISTRAR ESTADIA---
--------------------------

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


GO
--------------------------
------FACTURA---
--------------------------
---FACTURAR ESTADIA
CREATE FUNCTION CONTROL_ZETA.FN_BUSCA_FACTURA()
returns NUMERIC
AS
BEGIN
RETURN (SELECT MAX(FACTURA_NRO)+1 FROM CONTROL_ZETA.FACTURA)
END
GO
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

SET @CODIGO = 0


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
  AND R.RESERVA_FECHA_CANC IS NULL
  AND R.RESERVA_ESTADO IN ('RFSF')
  AND R.RESERVA_ID = E.EST_RESERVA_ID
  AND R.RESERVA_ID_REGIMEN = RG.REG_ID

SET @NRO_FACT = CONTROL_ZETA.FN_BUSCA_FACTURA(); 
SET @FACTURA_NRO=@NRO_FACT
BEGIN TRANSACTION
IF EXISTS (SELECT 1 FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID = @RESERVA_ID AND R.RESERVA_ESTADO ='RFSF')
BEGIN

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


INSERT INTO CONTROL_ZETA.ITEM_FACTURA (FACTURA_NRO,ITEM_FACTURA_CANTIDAD,ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
SELECT @NRO_FACT, 1,  C.CON_PRECIO, 'CONSUMIBLES'
FROM CONTROL_ZETA.ESTADIA_HAB_CON EH, CONTROL_ZETA.CONSUMIBLE C
WHERE EH.EST_ID = @ESTADIA_ID
AND EH.CON_ID = C.CON_ID


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

GO

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
	