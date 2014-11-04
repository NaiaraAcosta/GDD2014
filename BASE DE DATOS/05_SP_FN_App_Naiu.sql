---Referencia Accion
--1:Alta-Insert
--2:Modificacion-Update
--3:Borrado-Delete

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
	
END;
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
END;
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
returns tinyint
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
WHERE R.RESERVA_FECHA_INICIO>@fe_inicio_cierre AND 
R.RESERVA_FECHA_HASTA<@fe_fin_cierre AND 
R.RESERVA_ID_HOTEL=@id_hotel AND
R.RESERVA_ESTADO IN ('RC','RM','RI'))
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
		IF NOT EXISTS (SELECT * FROM CONTROL_ZETA.HOTEL H WHERE (H.HOTEL_NOMBRE=@nombre) OR (H.HOTEL_CALLE=@calle AND H.HOTEL_NRO_CALLE= @nro_calle AND H.HOTEL_ID_LOC=@id_loc AND H.HOTEL_PAIS=@id_pais))	
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
		END;
		ELSE
		SET @error=3
	END;
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
	END;
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
		IF CONTROL_ZETA.hay_reservas_fechas(@fe_inicio_cierre, @fe_fin_cierre ,@id_hotel )>0
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
WHERE HC.HOTEL_ID=@id_hotel AND 
@fe_desde> HC.HOTEL_C_FECHA_DESDE AND
@fe_desde< HC.HOTEL_C_FECHA_HASTA)

END

GO

CREATE PROCEDURE CONTROL_ZETA.SP_CANC_RCNS(@fe_sistema date)
AS
--@Desc: Realiza limpieza por no show
BEGIN
	UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RCNS' WHERE RESERVA_FECHA_INICIO<@fe_sistema
END; 

GO


CREATE PROCEDURE CONTROL_ZETA.SP_VERIFICA_DISPONIBILIDAD(@id_res NUMERIC,@hotel_id int,@fe_desde date,@fe_hasta date,@cant_hab tinyint,@fe_sist date,@id_tipo_hab SMALLINT, @disp smallint output)
AS
----@Desc: Verifica disponibilidad
BEGIN

	EXEC CONTROL_ZETA.SP_CANC_RCNS @fe_sistema=@fe_sist
	
	IF (CONTROL_ZETA.fe_res_consistente(@fe_desde,@fe_hasta,@fe_sist)=0)
	BEGIN
		IF (CONTROL_ZETA.hotel_cerrado(@fe_desde,@fe_hasta,@hotel_id)=0)
		BEGIN
			IF @id_res IS NOT NULL
			BEGIN
			--Es Modif: Ya existe reserva
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
		SET @disp=(-2)
	END
	ELSE 
		SET @disp=(-1)
END


GO

CREATE FUNCTION CONTROL_ZETA.get_id_reserva_new()
returns numeric
AS
--@Desc: Se obtiene el numero de id siguiente de reserva
BEGIN

RETURN(SELECT (MAX(RESERVA_ID)+1) FROM CONTROL_ZETA.RESERVA)

END;

GO


CREATE PROCEDURE CONTROL_ZETA.SP_ALTA_RESERVA(@hotel_id int,@fe_desde date,@fe_hasta date,@tipo_reg_id TINYINT,@cliente_id numeric,@id_usr varchar(50),@fe_sist date,@cant_hab tinyint,@id_reserva numeric OUTPUT,@error tinyint OUTPUT)
AS
--@Desc: Realiza el alta de reserva
BEGIN
	IF (CONTROL_ZETA.fe_res_consistente(@fe_desde,@fe_hasta,@fe_sist)=0)
	BEGIN
		SET @id_reserva=CONTROL_ZETA.get_id_reserva_new()
		INSERT INTO CONTROL_ZETA.RESERVA (RESERVA_ID,RESERVA_FECHA,RESERVA_FECHA_INICIO, RESERVA_FECHA_HASTA,RESERVA_ID_REGIMEN, RESERVA_ID_HOTEL, RESERVA_ESTADO, CLIENTE_ID, USR_USERNAME,RES_CANT_HAB)
		VALUES(@id_reserva,@fe_sist,@fe_desde,@fe_hasta,@tipo_reg_id,@hotel_id,'RC',@cliente_id,@id_usr,@cant_hab);
		set @error=1
	END
	ELSE
	set @error=5
	
END;


GO

CREATE PROCEDURE CONTROL_ZETA.SP_AGREGAR_HAB_RES(@id_reserva numeric,@cant_hab tinyint,@tipo_hab SMALLINT,@hotel_id int)
AS
--@Desc: Agrega la relacion de reserva y habitacion
BEGIN

	INSERT INTO CONTROL_ZETA.RESERVA_HABITACION(RESERVA_ID,HAB_ID)
	SELECT TOP (@cant_hab) @id_reserva,H.HAB_ID 
	FROM CONTROL_ZETA.HABITACION H 
	WHERE H.HAB_ID_TIPO=@tipo_hab AND 
	H.HAB_ID_HOTEL=@hotel_id;
	
	
END;


GO
CREATE PROCEDURE CONTROL_ZETA.SP_CONSISTENCIA_RESERVAS
AS
--@Desc:Identifica las inconsistencias y las soluciona
BEGIN

	INSERT INTO CONTROL_ZETA.RESERVA_BKP (RESERVA_ID,  
				RESERVA_FECHA_INICIO, RESERVA_FECHA_HASTA, 
				RESERVA_ID_REGIMEN, RESERVA_ID_HOTEL, RESERVA_ESTADO, CLIENTE_ID)
	SELECT R.RESERVA_ID, R.RESERVA_FECHA_INICIO, R.RESERVA_FECHA_HASTA,R.RESERVA_ID_REGIMEN,
	R.RESERVA_ID_HOTEL,'RINC',R.CLIENTE_ID 
	FROM CONTROL_ZETA.RESERVA R
	WHERE R.RES_CANT_HAB!=(SELECT COUNT(1) 
						   FROM CONTROL_ZETA.RESERVA_HABITACION RH
						    WHERE RH.RESERVA_ID=R.RESERVA_ID)
							
	DELETE CONTROL_ZETA.RESERVA
	WHERE RES_CANT_HAB!=(SELECT COUNT(1) 
						   FROM CONTROL_ZETA.RESERVA_HABITACION RH
						    WHERE RH.RESERVA_ID=RESERVA_ID)
												
						
END;

GO

CREATE PROCEDURE CONTROL_ZETA.SP_MODIF_RESERVA(@hotel_id int,@fe_desde date,@fe_hasta date,@tipo_reg_id TINYINT,@cliente_id numeric,@id_usr varchar(50),@id_reserva numeric,@fe_sist date,@cant_hab tinyint,@error tinyint OUTPUT)
AS
--@Desc: Modifica la reserva
BEGIN
	IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva)
	BEGIN
		IF ((SELECT R.RESERVA_FECHA_INICIO FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva)<@fe_desde)
		BEGIN
			UPDATE CONTROL_ZETA.RESERVA 
			SET RESERVA_FECHA=@fe_sist,
			RESERVA_FECHA_INICIO=@fe_desde, 
			RESERVA_FECHA_HASTA=@fe_hasta,
			RESERVA_ID_REGIMEN=@tipo_reg_id, 
			RESERVA_ID_HOTEL=@hotel_id, 
			RESERVA_ESTADO='RM', 
			CLIENTE_ID=@cliente_id,
			USR_USERNAME=@id_usr,
			RES_CANT_HAB=@cant_hab
			WHERE RESERVA_ID=@id_reserva
			DELETE CONTROL_ZETA.RESERVA_HABITACION WHERE RESERVA_ID=@id_reserva
			set @error=1
		END
		ELSE

		set @error=5
		END
	ELSE
		set @error=2

END;

/*GO

CREATE PROCEDURE CONTROL_ZETA.SP_MODIF_HAB_RES(@id_res numeric,@cant_h tinyint,@tipo_h SMALLINT,@hotel int,@error tinyint OUTPUT)
AS
--@Desc: Se realiza la modificacion de las habitaciones y reserva
BEGIN
	IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_res)
	BEGIN
		 
		EXEC CONTROL_ZETA.SP_AGREGAR_HAB_RES @id_reserva=@id_res ,@cant_hab=@cant_h ,@tipo_hab=@tipo_h ,@hotel_id=@hotel
		set @error=1
	END;
	ELSE
		set @error=2
END;*/

GO

CREATE PROCEDURE CONTROL_ZETA.SP_CANCELAR_RESERVA(@id_reserva numeric, @motivo varchar(150), @fecha_canc date, @id_usr varchar(50), @id_est varchar(4),@error tinyint OUTPUT)
AS
--@Desc:Cancela la reserva
BEGIN
IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva AND R.RESERVA_FECHA_INICIO>=@fecha_canc)
	BEGIN
		
		UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO=@id_est, RESERVA_MOTIVO_CANC=@motivo, RESERVA_FECHA_CANC=@fecha_canc,USR_USERNAME_CANC=@id_usr
		WHERE RESERVA_ID=@id_reserva
		set @error=1
	END;
else
	set @error=5
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


CREATE PROCEDURE CONTROL_ZETA.SP_REGISTRAR_CONSUMIBLE(@id_hotel int, @nro_hab SMALLINT, @id_con smallint, @cant tinyint, @error tinyint OUTPUT )
AS
--@Desc:Se registran los consumibles
BEGIN
DECLARE 
@i tinyint =1,
@id_hab numeric =CONTROL_ZETA.get_id_habitacion(@nro_hab,@id_hotel),
@id_est numeric = 0

set @id_est=CONTROL_ZETA.get_estadia(@id_hab)

IF (@id_est>0)
	BEGIN
	IF @id_hab>0
	BEGIN
		WHILE @i<@cant
		BEGIN
			INSERT INTO CONTROL_ZETA.ESTADIA_HAB_CON (HAB_ID,CON_ID,EST_ID)
			VALUES (@id_hab,@id_con,@id_est)
		END;
		set @error=1	
	END;	
	ELSE
	set @error=5		
	END;
ELSE
set @error=6
END;
