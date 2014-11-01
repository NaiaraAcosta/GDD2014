---Referencia Accion
--1:Alta-Insert
--2:Modificacion-Update
--3:Borrado-Delete

----ABM ROL-3 (Deberia cargar la tabla ROL, ROL_FUNC)
CREATE PROCEDURE CONTROL_ZETA.SP_ABM_ROL (@accion SMALLINT,@nombre varchar(20),@estado varchar(1))
as
begin
end;
---IMPORTANTE:Se tendria que hacer otro SP para asociar el ROL con la Funcionalidad y que desde la app se ejecute varias veces


---ASIGNACION DE FUNCIONES A ROL-3 (Carga tabla ROL_FUNC)
CREATE PROCEDURE CONTROL_ZETA.SP_ROL_FUNC(@accion SMALLINT,@rol_id TINYINT, @func_id TINYINT)
AS
BEGIN
END; 

---ABM HOTEL(Tabla Hotel)-2
CREATE PROCEDURE CONTROL_ZETA.SP_ABM_HOTEL()
AS
BEGIN
END;
---ABM HABITACION(Tabla habitaciones)-2
CREATE PROCEDURE CONTROL_ZETA.SP_ABM_RESERVA()
AS
BEGIN
END;

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


CREATE PROCEDURE CONTROL_ZETA.SP_CANC_RCNS(@fe_sistema date)
AS
--@Desc: Realiza limpieza por no show
BEGIN
	UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RCNS' WHERE RESERVA_FECHA_INICIO<@fe_sistema
END; 

GO


CREATE PROCEDURE CONTROL_ZETA.SP_VERIFICA_DISPONIBILIDAD(@hotel_id int,@fe_desde date,@fe_hasta date,@cant_hab tinyint,@fe_sist date,@tipo_hab SMALLINT, @disp smallint output)
AS
----@Desc: Verifica disponibilidad
BEGIN

	EXEC CONTROL_ZETA.SP_CANC_RCNS @fe_sistema=@fe_sist
	
	IF (CONTROL_ZETA.fe_res_consistente(@fe_desde,@fe_hasta,@fe_sist)=0)
	BEGIN
	SET @disp = (SELECT COUNT (H.HAB_ID) 
				FROM CONTROL_ZETA.HABITACION H 
				WHERE H.HAB_ID_HOTEL=@hotel_id AND
				H.HAB_ID_TIPO=@tipo_hab AND
				(H.HAB_ID NOT IN (SELECT RH.HAB_ID 
								  FROM CONTROL_ZETA.RESERVA_HABITACION RH 
								  WHERE RH.RESERVA_ID IN (SELECT R.RESERVA_ID 
								             			  FROM CONTROL_ZETA.RESERVA R 
														  WHERE R.RESERVA_FECHA_INICIO=@fe_desde AND 
														  R.RESERVA_FECHA_HASTA=@fe_hasta AND
														  R.RESERVA_ESTADO IN('RI','RC','RM'))															 )OR 
				H.HAB_ID IN (SELECT DISTINCT(EHC.HAB_ID) 
				FROM CONTROL_ZETA.ESTADIA_HAB_CON EHC, CONTROL_ZETA.ESTADIA E 
				WHERE EHC.EST_ID=E.EST_ID AND
				E.EST_FECHA_HASTA IS NOT NULL)))
	END
	ELSE 
	SET @disp=(-1)
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


CREATE PROCEDURE CONTROL_ZETA.SP_ALTA_RESERVA(@hotel_id int,@fe_desde date,@fe_hasta date,@tipo_reg_id TINYINT,@cliente_id numeric,@id_usr varchar(50),@fe_sist date,@id_reserva numeric OUTPUT,@error tinyint OUTPUT)
AS
--@Desc: Realiza el alta de reserva
BEGIN
	IF (CONTROL_ZETA.fe_res_consistente(@fe_desde,@fe_hasta,@fe_sist)=0)
	BEGIN
		SET @id_reserva=CONTROL_ZETA.get_id_reserva_new()
		INSERT INTO CONTROL_ZETA.RESERVA (RESERVA_ID,RESERVA_FECHA,RESERVA_FECHA_INICIO, RESERVA_FECHA_HASTA,RESERVA_ID_REGIMEN, RESERVA_ID_HOTEL, RESERVA_ESTADO, CLIENTE_ID, USR_USERNAME)
		VALUES(@id_reserva,@fe_sist,@fe_desde,@fe_hasta,@tipo_reg_id,@hotel_id,'RINC',@cliente_id,@id_usr);
		set @error=0
	END
	ELSE
	set @error=1
	
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
	
	UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RC'
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
	WHERE R.RESERVA_ESTADO ='RINC';
							
	DELETE CONTROL_ZETA.RESERVA 
	WHERE RESERVA_ESTADO ='RINC';
												
						
END;

GO

CREATE PROCEDURE CONTROL_ZETA.SP_MODIF_RESERVA(@hotel_id int,@fe_desde date,@fe_hasta date,@tipo_reg_id TINYINT,@cliente_id numeric,@id_usr varchar(50),@id_reserva numeric,@fe_sist date,@error tinyint OUTPUT)
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
			RESERVA_ESTADO='RINC', 
			CLIENTE_ID=@cliente_id,
			USR_USERNAME=@id_usr
			WHERE RESERVA_ID=@id_reserva
			
			set @error=0
		END
		ELSE

		set @error=1
		END
	ELSE
		set @error=2

END;

GO

CREATE PROCEDURE CONTROL_ZETA.SP_MODIF_HAB_RES(@id_res numeric,@cant_h tinyint,@tipo_h SMALLINT,@hotel int,@error tinyint OUTPUT)
AS
--@Desc: Se realiza la modificacion de las habitaciones y reserva
BEGIN
	IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_res)
	BEGIN
		DELETE CONTROL_ZETA.RESERVA_HABITACION WHERE RESERVA_ID=@id_res 
		EXEC CONTROL_ZETA.SP_AGREGAR_HAB_RES @id_reserva=@id_res ,@cant_hab=@cant_h ,@tipo_hab=@tipo_h ,@hotel_id=@hotel
		UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RM'
		set @error=0
	END;
	ELSE
		set @error=1
END;

GO

CREATE PROCEDURE CONTROL_ZETA.SP_CANCELAR_RESERVA(@id_reserva numeric, @motivo varchar(150), @fecha_canc date, @id_usr varchar(50), @id_est varchar(4),@error tinyint OUTPUT)
AS
--@Desc:Cancela la reserva
BEGIN
IF EXISTS(SELECT *FROM CONTROL_ZETA.RESERVA R WHERE R.RESERVA_ID=@id_reserva AND R.RESERVA_FECHA_INICIO>=@fecha_canc)
	BEGIN
		
		UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO=@id_est, RESERVA_MOTIVO_CANC=@motivo, RESERVA_FECHA_CANC=@fecha_canc,USR_USERNAME_CANC=@id_usr
		WHERE RESERVA_ID=@id_reserva
		set @error=0
	END;
else
	set @error=1
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
@i tinyint =1
@id_hab numeric =CONTROL_ZETA.get_id_habitacion(@nro_hab,@id_hotel)
@id_est numeric = CONTROL_ZETA.get_id_estadia(@hab_id)

IF (@id_est>0)
	BEGIN
	IF @id_hab>0
	BEGIN
		WHILE @i<@cant
		BEGIN
			INSERT INTO CONTROL_ZETA.ESTADIA_HAB_CON (HAB_ID,CON_ID,EST_ID)
			VALUES (@id_hab,@id_con,@id_est)
		END;
		set @error=0	
	END;	
	ELSE
		set @error=2		
	END;
ELSE
set @error=1
END;
