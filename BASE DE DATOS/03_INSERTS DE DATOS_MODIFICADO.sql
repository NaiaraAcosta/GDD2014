USE [GD2C2014]
GO

----------------------
--TABLAS PARAMETRICAS
----------------------


---TABLA PAIS
INSERT INTO CONTROL_ZETA.PAIS VALUES('ARGENTINA');
GO

---TABLA NACIONALIDAD
INSERT INTO CONTROL_ZETA.NACIONALIDAD
SELECT TOP(1) CLIENTE_NACIONALIDAD FROM GD_ESQUEMA.MAESTRA;
GO
---TABLA TIPO_DOC
INSERT INTO CONTROL_ZETA.TIPO_DOC VALUES ('PASAPORTE');
GO

---TABLA FUNCIONALIDAD
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('ABM DE ROL');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('LOGIN Y SEGURIDAD');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('ABM DE USUARIO');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('ABM DE CLIENTE(HU�SPEDES)');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('ABM DE HOTEL');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('ABM DE HABITACION');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('ABM REGIMEN DE ESTADIA ');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('GENERAR O MODIFICAR UNA RESERVA');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('CANCELAR RESERVA');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('REGISTRAR ESTADIA (CHECK-IN/CHECK-OUT)');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('REGISTRAR CONSUMIBLES');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('FACTURAR ESTADIA');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('LISTADO ESTAD�STICO');
INSERT INTO CONTROL_ZETA.FUNCIONALIDAD VALUES ('RESERVAS INCONSISTENTES');
GO
--TABLA LOCALIDAD
INSERT INTO CONTROL_ZETA.LOCALIDAD 
SELECT HOTEL_CIUDAD FROM GD_ESQUEMA.MAESTRA group by HOTEL_CIUDAD;
GO
---TABLA ROL
INSERT INTO CONTROL_ZETA.ROL VALUES ('ADMIN', 'H');
INSERT INTO CONTROL_ZETA.ROL VALUES ('RECEPCIONISTA', 'H');
INSERT INTO CONTROL_ZETA.ROL VALUES ('GUEST', 'H');
GO
---TABLA ROL_FUNC
--Funcionalidades de ADMINISTRADOR
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,1);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,2);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,3);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,4);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,5);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,6);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,7);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,8);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,9);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,10);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,11);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,12);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,13);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(1,14);
GO
--Funcionalidades de RECEPCIONISTA
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,2);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,8);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,9);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,10);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,11);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,12);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(2,14);
GO
--Funcionalidades GUEST
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(3,8);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(3,9);
INSERT INTO CONTROL_ZETA.ROL_FUNC VALUES(3,2);
---TABLA TIPO_HAB
INSERT INTO CONTROL_ZETA.TIPO_HAB (TIPO_HAB_ID,TIPO_HAB_DESCRIPCION, TIPO_HAB_PORC, TIPO_HAB_CANT_PERSONAS)
SELECT HABITACION_TIPO_CODIGO, HABITACION_TIPO_DESCRIPCION, HABITACION_TIPO_PORCENTUAL, 
case HABITACION_TIPO_CODIGO when 1001 then 1  
							when 1002 then 2
							when 1003 then 3
							when 1004 then 4
							when 1005 then 5
							else 1 end cant_personas
FROM GD_ESQUEMA.MAESTRA
GROUP BY  HABITACION_TIPO_CODIGO, HABITACION_TIPO_DESCRIPCION, HABITACION_TIPO_PORCENTUAL;

GO


---TABLA CONSUMIBLE
INSERT INTO CONTROL_ZETA.CONSUMIBLE (CON_ID, CON_DESCRIPCION, CON_PRECIO,CON_ES_MODERADO) 
SELECT distinct CONSUMIBLE_CODIGO, CONSUMIBLE_DESCRIPCION, CONSUMIBLE_PRECIO,'S'
FROM GD_ESQUEMA.MAESTRA
WHERE CONSUMIBLE_CODIGO IS NOT NULL
--GROUP BY CONSUMIBLE_CODIGO, CONSUMIBLE_DESCRIPCION, CONSUMIBLE_PRECIO;
GO


UPDATE CONTROL_ZETA.CONSUMIBLE
SET CON_ES_MODERADO = 'N'
WHERE CON_ID = 2326;
GO

---TABLA REGIMEN
INSERT INTO CONTROL_ZETA.REGIMEN (REG_DESCRIPCION, REG_PRECIO)
SELECT  
DISTINCT REGIMEN_DESCRIPCION, REGIMEN_PRECIO
FROM GD_ESQUEMA.MAESTRA;
GO

-- ESTADOS DE LA RESERVA
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA SIN FACTURA','RSF');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CORRECTA','RC');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA MODIFICADA','RM');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CANCELADA POR RECEPCION','RCR');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CANCELADA POR EL CLIENTE','RCC');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CANCELADA POR NO-SHOW','RCNS');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CON INGRESO','RI');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA INCONSISTENTE','RINC');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA EN CURSO','REC');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA FINALIZADA SIN FACTURA','RFSF');
GO

 
--------------------
--TABLA DE DATOS----
--------------------

INSERT INTO CONTROL_ZETA.CLIENTE
( CLIENTE_NOMBRE, 
  CLIENTE_APELLIDO,
  CLIENTE_ID_TIPO_DOC,
  CLIENTE_DOC,
  CLIENTE_MAIL, 
  CLIENTE_TEL, 
 CLIENTE_ID_LOC,
 CLIENTE_ID_PAIS_ORIGEN,
 CLIENTE_DOM_CALLE,
 CLIENTE_DOM_NRO,
 CLIENTE_DPTO,
 CLIENTE_DOM_PISO, 
 CLIENTE_NAC_ID,
 CLIENTE_ESTADO,
 CLIENTE_FECHA_NAC
 )
SELECT  UPPER(CLIENTE_NOMBRE), 
   UPPER(CLIENTE_APELLIDO),
  1 AS TIPO_DOC,
  CLIENTE_PASAPORTE_NRO,
  CLIENTE_MAIL,
  NULL AS TEL,
    NULL AS LOCALIDAD, 
  NULL AS PAIS_ORIGEN, 
  CLIENTE_DOM_CALLE,  
  CLIENTE_NRO_CALLE,  
  CLIENTE_DEPTO,
  CLIENTE_PISO,  
  1 AS NACIONALIDAD,
     'H',  
	 CLIENTE_FECHA_NAC
 FROM GD_ESQUEMA.MAESTRA M
 group by UPPER(CLIENTE_NOMBRE), 
   UPPER(CLIENTE_APELLIDO),
  M.Cliente_Pasaporte_Nro,
  M.Cliente_Mail,
  M.Cliente_Depto,
  M.Cliente_Dom_Calle,
  M.Cliente_Nacionalidad,
  M.Cliente_Nro_Calle,
  M.Cliente_Piso,
  M.Cliente_Fecha_Nac
 GO
 -- 100740

INSERT INTO CONTROL_ZETA.HOTEL
(HOTEL_CALLE,HOTEL_NRO_CALLE, HOTEL_ID_LOC, 
        HOTEL_CANT_ESTRELLA, HOTEL_PAIS, 
        HOTEL_RECARGA_ESTRELLA,HOTEL_FECHA_CREACION)
 SELECT DISTINCT HOTEL_CALLE, HOTEL_NRO_CALLE, 
  L.LOC_ID,
HOTEL_CANTESTRELLA, NULL AS PAIS, HOTEL_RECARGA_ESTRELLA,  GETDATE()AS FECHA
FROM GD_ESQUEMA.MAESTRA M, CONTROL_ZETA.LOCALIDAD L
WHERE M.Hotel_Ciudad=L.LOC_DETALLE -- 16
GO

--TABLA HOTEL-REGIMEN
INSERT INTO CONTROL_ZETA.HOTEL_REGIMEN(HOTEL_ID, REG_ID, REG_ESTADO)
SELECT DISTINCT  
     CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE) AS HOTEL_ID,
      R.REG_ID, 'H'
FROM GD_ESQUEMA.MAESTRA M,
     CONTROL_ZETA.REGIMEN R
WHERE M.REGIMEN_DESCRIPCION = R.REG_DESCRIPCION; -- 64
GO

--TABLA HABITACION

--SELECT * FROM CONTROL_ZETA.HABITACION

INSERT INTO CONTROL_ZETA.HABITACION (HAB_NRO, HAB_ID_HOTEL, HAB_PISO,HAB_UBI_HOTEL,HAB_ID_TIPO)
SELECT DISTINCT HABITACION_NUMERO,
       CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE) AS HOTEL_ID,
       HABITACION_PISO,
       CASE WHEN HABITACION_FRENTE = 'N' THEN 'COMUN'
       WHEN HABITACION_FRENTE = 'S' THEN 'CON FRENTE'
       ELSE ' ' END,
       CONTROL_ZETA.GET_ID_TIPO_HABITACION(M.HABITACION_TIPO_DESCRIPCION)AS TIPO_HAB_ID
 FROM GD_ESQUEMA.MAESTRA M
--ORDER BY HOTEL_ID, HABITACION_NUMERO, HABITACION_PISO ; -- 345
GO

--TABLA RESERVA


INSERT INTO CONTROL_ZETA.RESERVA (RESERVA_ID,  
            RESERVA_FECHA_INICIO, RESERVA_FECHA_HASTA, 
            RESERVA_ID_REGIMEN, RESERVA_ID_HOTEL, RESERVA_ESTADO, CLIENTE_ID)
SELECT  M.RESERVA_CODIGO,
        M.RESERVA_FECHA_INICIO,
        M.RESERVA_FECHA_INICIO + M.RESERVA_CANT_NOCHES AS FECHA_HASTA,
        R.REG_ID,
        H.HOTEL_ID,
		'RINC' AS ESTADO,
        C.CLIENTE_ID
FROM GD_ESQUEMA.MAESTRA M , CONTROL_ZETA.REGIMEN R, CONTROL_ZETA.CLIENTE C, CONTROL_ZETA.HOTEL H
WHERE M.REGIMEN_DESCRIPCION = R.REG_DESCRIPCION
AND M.CLIENTE_APELLIDO = C.CLIENTE_APELLIDO
AND M.CLIENTE_NOMBRE = C.CLIENTE_NOMBRE
AND M.CLIENTE_PASAPORTE_NRO = C.CLIENTE_DOC
AND H.HOTEL_CALLE=M.Hotel_Calle
AND H.HOTEL_NRO_CALLE=M.Hotel_Nro_Calle
group by M.RESERVA_CODIGO,
        M.RESERVA_FECHA_INICIO,
        M.RESERVA_FECHA_INICIO + M.RESERVA_CANT_NOCHES ,
        R.REG_ID,
        H.HOTEL_ID,
		--'RINC' ,
        C.CLIENTE_ID; -- 100740
GO

---UPDATES RSF
UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RSF' WHERE RESERVA_ID IN 
(select m.Reserva_Codigo from gd_esquema.Maestra m  where  m.Factura_Nro is null group by m.Reserva_Codigo,m.Factura_Nro
except
select  m.Reserva_Codigo from gd_esquema.Maestra m where  m.Factura_Nro is not null group by m.Reserva_Codigo,m.Factura_Nro 
) and RESERVA_FECHA_HASTA<GETDATE()

GO
---UPDATE REC para Reservas en curso
UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='REC' WHERE RESERVA_ID IN 
(select m.Reserva_Codigo from gd_esquema.Maestra m  where  m.Factura_Nro is null group by m.Reserva_Codigo,m.Factura_Nro
except
select  m.Reserva_Codigo from gd_esquema.Maestra m where  m.Factura_Nro is not null group by m.Reserva_Codigo,m.Factura_Nro 
)and RESERVA_FECHA_INICIO<GETDATE()
and RESERVA_FECHA_HASTA>=GETDATE()

GO
---UPDATE RC
UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RC' WHERE RESERVA_ID IN
(select m.Reserva_Codigo from gd_esquema.Maestra m  where  m.Factura_Nro is null group by m.Reserva_Codigo,m.Factura_Nro
except
select  m.Reserva_Codigo from gd_esquema.Maestra m where  m.Factura_Nro is not null group by m.Reserva_Codigo,m.Factura_Nro 
)
and RESERVA_FECHA_HASTA>GETDATE()
and RESERVA_ESTADO='RINC'

GO
---UPDATES RI

UPDATE CONTROL_ZETA.RESERVA SET RESERVA_ESTADO='RI' 
WHERE RESERVA_ESTADO='RINC' 
AND RESERVA_FECHA_HASTA<GETDATE()

--Los casos RINC son reservas con factura que no finalizaron, o reservas a futuro q tienen factura

--TABLA ESTADIA
INSERT INTO CONTROL_ZETA.ESTADIA (EST_RESERVA_ID, EST_FECHA_DESDE, EST_FECHA_HASTA)
  SELECT DISTINCT RESERVA_ID, 
                M.ESTADIA_FECHA_INICIO, 
                M.ESTADIA_FECHA_INICIO + M.ESTADIA_CANT_NOCHES
FROM GD_ESQUEMA.MAESTRA M,
     CONTROL_ZETA.RESERVA R,
     CONTROL_ZETA.CLIENTE C,
     CONTROL_ZETA.REGIMEN REG
WHERE  M.RESERVA_CODIGO = R.RESERVA_ID 
   AND M.CLIENTE_NOMBRE = C.CLIENTE_NOMBRE
   AND  M.CLIENTE_APELLIDO = C.CLIENTE_APELLIDO
   AND M.CLIENTE_MAIL = C.CLIENTE_MAIL
   AND  C.CLIENTE_ID = R.CLIENTE_ID        
   AND  CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE ) = R.RESERVA_ID_HOTEL
   AND M.REGIMEN_DESCRIPCION = REG.REG_DESCRIPCION 
   AND R.RESERVA_ID_REGIMEN = REG.REG_ID
   AND M.RESERVA_FECHA_INICIO = R.RESERVA_FECHA_INICIO
   AND M.RESERVA_FECHA_INICIO + M.RESERVA_CANT_NOCHES = R.RESERVA_FECHA_HASTA
   AND M.ESTADIA_FECHA_INICIO IS NOT NULL
   AND M.Factura_Nro IS NOT NULL ; --89603
GO  
  
--TABLA ESTADIA_HAB_CON  
/*INSERT INTO CONTROL_ZETA.ESTADIA_HAB_CON (EST_ID, HAB_ID,CON_ID)
SELECT E.EST_ID, HA.HAB_ID, M.CONSUMIBLE_CODIGO
FROM GD_ESQUEMA.MAESTRA M,
     CONTROL_ZETA.RESERVA R,
     CONTROL_ZETA.ESTADIA E,
     CONTROL_ZETA.HABITACION HA
WHERE M.RESERVA_CODIGO = R.RESERVA_ID
  AND R.RESERVA_ID = E.EST_RESERVA_ID
  AND  CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE ) = R.RESERVA_ID_HOTEL
  AND HA.HAB_ID_HOTEL = CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE ) 
  AND HA.HAB_NRO = M.HABITACION_NUMERO     
 --AND M.ESTADIA_FECHA_INICIO >= E.EST_FECHA_DESDE --2 240 075
  AND M.CONSUMIBLE_CODIGO IS NOT NULL ; -- 268809*/

INSERT INTO CONTROL_ZETA.ESTADIA_HAB_CON (EST_ID, HAB_ID,CON_ID, CANTIDAD)
SELECT E.EST_ID, HA.HAB_ID, M.CONSUMIBLE_CODIGO, COUNT(M.CONSUMIBLE_CODIGO) AS CANTIDAD
FROM GD_ESQUEMA.MAESTRA M,
     CONTROL_ZETA.RESERVA R,
     CONTROL_ZETA.ESTADIA E,
     CONTROL_ZETA.HABITACION HA
WHERE M.RESERVA_CODIGO = R.RESERVA_ID
  AND R.RESERVA_ID = E.EST_RESERVA_ID
  AND  CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE ) = R.RESERVA_ID_HOTEL
  AND HA.HAB_ID_HOTEL = CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE ) 
  AND HA.HAB_NRO = M.HABITACION_NUMERO     
  AND M.CONSUMIBLE_CODIGO IS NOT NULL 
  GROUP BY E.EST_ID, HA.HAB_ID, M.CONSUMIBLE_CODIGO; -- 268809  
GO
  
--TABLA RESERVA_HABITACION
INSERT INTO CONTROL_ZETA.RESERVA_HABITACION (RESERVA_ID, HAB_ID)
   select distinct Ma.Reserva_Codigo,
        H.HAB_ID 
  from gd_esquema.Maestra Ma,
       CONTROL_ZETA.HABITACION H
 where CONTROL_ZETA.GET_ID_HOTEL(Ma.HOTEL_CIUDAD,Ma.HOTEL_NRO_CALLE,Ma.HOTEL_CALLE ) = H.HAB_ID_HOTEL
 and Ma.Habitacion_Numero = H.HAB_NRO ;
GO
 
 
INSERT INTO CONTROL_ZETA.ESTADIA_CLIENTE (EST_ID, CLIENTE_ID) 
 SELECT DISTINCT E.EST_ID, R.CLIENTE_ID 
 FROM GD_ESQUEMA.MAESTRA M,
      CONTROL_ZETA.RESERVA R,
      CONTROL_ZETA.ESTADIA E
WHERE M.RESERVA_CODIGO = R.RESERVA_ID
 AND R.RESERVA_ID = E.EST_RESERVA_ID;
GO
 
 --TABLA FACTURA 
 
 INSERT INTO CONTROL_ZETA.FACTURA 
 (FACTURA_NRO,FACTURA_FECHA,FACTURA_FORMA_PAGO ,EST_ID, FACTURA_ID_CLIENTE)
 SELECT DISTINCT M.FACTURA_NRO, M.FACTURA_FECHA,  'E',E.EST_ID, EC.CLIENTE_ID
 FROM GD_ESQUEMA.MAESTRA M,
      CONTROL_ZETA.ESTADIA E,
	  CONTROL_ZETA.ESTADIA_CLIENTE EC
WHERE M.RESERVA_CODIGO = E.EST_RESERVA_ID
AND FACTURA_NRO IS NOT NULL
AND EC.EST_ID=E.EST_ID;  --89603
GO 
  
--TABLA ITEM_FACTURA 


INSERT INTO CONTROL_ZETA.ITEM_FACTURA 
 (FACTURA_NRO, ITEM_FACTURA_CANTIDAD, ITEM_FACTURA_MONTO, ITEM_DESCRIPCION, ITEM_ID_EST_CON)
 SELECT  M.FACTURA_NRO, M.ITEM_FACTURA_CANTIDAD ITEM_FACTURA_CANTIDAD, 
          M.ITEM_FACTURA_MONTO * M.Reserva_Cant_Noches, 'COSTO POR ESTADIA', NULL AS ID_EST_CON -- VER ESTE CAMPO
 FROM GD_ESQUEMA.MAESTRA M
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL 
 AND Consumible_Codigo IS  NULL --and factura_nro = 2396782
  UNION all
  SELECT  M.FACTURA_NRO,  COUNT(Consumible_Codigo) ITEM_FACTURA_CANTIDAD, 
         sum(M.Consumible_Precio) as ITEM_FACTURA_MONTO, 
         M.Consumible_Descripcion + ' - Habitacion: ' + 
         CAST(M.Habitacion_Numero AS CHAR(5) ) 'CONSUMIBLES',
         ehc.EST_HAB_CON_ID
 FROM GD_ESQUEMA.MAESTRA M, CONTROL_ZETA.ESTADIA e, CONTROL_ZETA.ESTADIA_HAB_CON ehc
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL 
   AND  Consumible_Codigo IS NOT NULL
   AND E.EST_RESERVA_ID = M.Reserva_Codigo
   AND E.EST_ID = ehc.EST_ID
   AND M.Consumible_Codigo = ehc.CON_ID
 --AND factura_nro = 2396782
  GROUP BY M.FACTURA_NRO, M.Consumible_Codigo,M.Consumible_Precio, 
  M.Consumible_Descripcion,ehc.EST_HAB_CON_ID, M.Habitacion_Numero
  UNION all
  SELECT  M.FACTURA_NRO,  
         COUNT(*) ITEM_FACTURA_CANTIDAD,
        -SUM(M.Consumible_Precio) as ITEM_FACTURA_MONTO, 
        'DESCUENTO ALL INCLUSIVE', NULL
 FROM GD_ESQUEMA.MAESTRA M, CONTROL_ZETA.CONSUMIBLE C
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL
 and M.Regimen_Descripcion = 'All inclusive' --AND factura_nro = 2396782
 AND M.Consumible_Descripcion = C.CON_DESCRIPCION
 group by M.FACTURA_NRO, C.CON_ES_MODERADO
 UNION ALL
  SELECT  M.FACTURA_NRO, 
       case  C.CON_ES_MODERADO
       when 'S'
       then COUNT(1)
       else COUNT(0) end ITEM_FACTURA_CANTIDAD,
        -SUM(M.Consumible_Precio) as ITEM_FACTURA_MONTO, 
        'DESCUENTO ALL INCLUSIVE MODERADO', NULL 
 FROM GD_ESQUEMA.MAESTRA M, CONTROL_ZETA.CONSUMIBLE C
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL
 and M.Regimen_Descripcion = 'All inclusive moderado' --AND factura_nro = 2396750
 AND M.Consumible_Descripcion = C.CON_DESCRIPCION
 AND C.CON_ES_MODERADO = 'S' 
 group by M.FACTURA_NRO,  C.CON_ES_MODERADO
-- ORDER BY M.FACTURA_NRO;  -- 354083
GO

 
----------- 
---UPDATES
-----------

 ---TABLA FACTURA (Actualizacion de Monto)
 UPDATE CONTROL_ZETA.FACTURA 
  SET CONTROL_ZETA.FACTURA.FACTURA_TOTAL =
 (select SUM(IFA.item_factura_monto) AS MONTO
 from CONTROL_ZETA.ITEM_FACTURA IFA
 where IFA.factura_nro = CONTROL_ZETA.FACTURA.factura_nro); -- 89603
GO
 
GO
  
  -----------------------------------
  ---Carga de usuario administrador
  -----------------------------------
INSERT INTO CONTROL_ZETA.EMPLEADO(EMP_APELLIDO,EMP_NOMBRE,EMP_ID_TIPO_DOC,EMP_DOC,EMP_FECHA_NAC,EMP_MAIL,EMP_DOM,EMP_TEL,USR_USERNAME) 
VALUES(
'GDD',
'GESTION',
1,
'37098607',
'06/08/2014',
'GDD@gmail.com',
null,
null,
'ADMIN');
GO 
 
INSERT INTO CONTROL_ZETA.USUARIO(USR_USERNAME,USR_PASS,USR_ESTADO,USR_INTENTOS) 
VALUES
('ADMIN','e6b87050bfcb8143fcb8db0170a4dc9ed00d904ddd3e2a4ad1b1e8dc0fdc9be7','H',0);
--Pass encriptada:w23e
GO

INSERT INTO CONTROL_ZETA.USR_ROL_HOTEL
(USR_USERNAME,
ROL_ID,
HOTEL_ID)
SELECT 'ADMIN',1, H.HOTEL_ID FROM CONTROL_ZETA.HOTEL H;

GO

 -----------------------------------
 ---Carga de usuario Recepcionista
 ----------------------------------
 
 INSERT INTO CONTROL_ZETA.EMPLEADO(EMP_APELLIDO,EMP_NOMBRE,EMP_ID_TIPO_DOC,EMP_DOC,EMP_FECHA_NAC,EMP_MAIL,EMP_DOM,EMP_TEL,USR_USERNAME) 
VALUES(
'GDD-RECEPC',
'GESTION',
1,
'37098609',
'06/08/2000',
'GDD-RECEP@gmail.com',
null,
null,
'RECEPCIONISTA');
GO 
 
INSERT INTO CONTROL_ZETA.USUARIO(USR_USERNAME,USR_PASS,USR_ESTADO,USR_INTENTOS) 
VALUES
('RECEPCIONISTA','abbce88bd1788accc2ec66a738003efbe2342dea1c3e60c1459583289cbb3fed','H',0);
--Pass encriptada:w23e
GO

INSERT INTO CONTROL_ZETA.USR_ROL_HOTEL
(USR_USERNAME,
ROL_ID,
HOTEL_ID)
SELECT 'RECEPCIONISTA',2, H.HOTEL_ID FROM CONTROL_ZETA.HOTEL H;
go