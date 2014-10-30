USE [GD2C2014]
GO

----------------------
--TABLAS PARAMETRICAS
----------------------


---TABLA PAIS
INSERT INTO CONTROL_ZETA.PAIS VALUES('ARGENTINA');

---TABLA NACIONALIDAD
INSERT INTO CONTROL_ZETA.NACIONALIDAD
SELECT DISTINCT CLIENTE_NACIONALIDAD FROM GD_ESQUEMA.MAESTRA;

---TABLA TIPO_DOC
INSERT INTO CONTROL_ZETA.TIPO_DOC VALUES ('PASAPORTE');


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

--TABLA LOCALIDAD
INSERT INTO CONTROL_ZETA.LOCALIDAD 
SELECT DISTINCT HOTEL_CIUDAD FROM GD_ESQUEMA.MAESTRA;

---TABLA ROL
INSERT INTO CONTROL_ZETA.ROL VALUES ('ADMINISTRADOR', 'H');
INSERT INTO CONTROL_ZETA.ROL VALUES ('RECEPCIONISTA', 'H');
INSERT INTO CONTROL_ZETA.ROL VALUES ('GUEST', 'H');

---TABLA TIPO_HAB
INSERT INTO CONTROL_ZETA.TIPO_HAB (TIPO_HAB_ID,TIPO_HAB_DESCRIPCION, TIPO_HAB_PORC)
SELECT DISTINCT HABITACION_TIPO_CODIGO, HABITACION_TIPO_DESCRIPCION, HABITACION_TIPO_PORCENTUAL 
FROM GD_ESQUEMA.MAESTRA ;



---TABLA CONSUMIBLE
INSERT INTO CONTROL_ZETA.CONSUMIBLE (CON_ID, CON_DESCRIPCION, CON_PRECIO) 
SELECT DISTINCT CONSUMIBLE_CODIGO, CONSUMIBLE_DESCRIPCION, CONSUMIBLE_PRECIO
FROM GD_ESQUEMA.MAESTRA
WHERE CONSUMIBLE_CODIGO IS NOT NULL;

UPDATE CONTROL_ZETA.CONSUMIBLE
SET CON_ES_MODERADO = 'S'
WHERE CON_ID IN (2324,2325,2327);

UPDATE CONTROL_ZETA.CONSUMIBLE
SET CON_ES_MODERADO = 'N'
WHERE CON_ID = 2326;

---TABLA REGIMEN
INSERT INTO CONTROL_ZETA.REGIMEN (REG_DESCRIPCION, REG_PRECIO)
SELECT  
DISTINCT REGIMEN_DESCRIPCION, REGIMEN_PRECIO
FROM GD_ESQUEMA.MAESTRA;

-- ESTADOS DE LA RESERVA
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA SIN FACTURA','RSF');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CORRECTA','RC');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA MODIFICADA','RM');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CANCELADA POR RECEPCION','RCR');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CANCELADA POR EL CLIENTE','RCC');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CANCELADA POR NO-SHOW','RCNS');
INSERT INTO CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIPCION, ESTADO_DESCRIP_CORTA) VALUES ('RESERVA CON INGRESO','RI');


 
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
SELECT DISTINCT UPPER(CLIENTE_NOMBRE), 
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
  (SELECT  NAC_ID --OBTIENE EL ID DE NACIONALIDAD
   FROM CONTROL_ZETA.NACIONALIDAD 
  WHERE NAC_DETALLE = M.CLIENTE_NACIONALIDAD) AS NACIONALIDAD,
     'H',  
	 CLIENTE_FECHA_NAC
 FROM GD_ESQUEMA.MAESTRA M;
 -- 100740

SELECT * FROM CONTROL_ZETA.HOTEL
INSERT INTO CONTROL_ZETA.HOTEL
(HOTEL_CALLE,HOTEL_NRO_CALLE, HOTEL_ID_LOC, 
        HOTEL_CANT_ESTRELLA, HOTEL_PAIS, 
        HOTEL_RECARGA_ESTRELLA,HOTEL_FECHA_CREACION)
SELECT DISTINCT HOTEL_CALLE, HOTEL_NRO_CALLE, 
  CONTROL_ZETA.GET_CIUDAD(HOTEL_CIUDAD ) AS CIUDAD,
HOTEL_CANTESTRELLA, NULL AS PAIS, HOTEL_RECARGA_ESTRELLA,  GETDATE()AS FECHA
 FROM GD_ESQUEMA.MAESTRA; -- 16

/***************************************************************************/
/***************************************************************************/
/***************************************************************************/
-- ESTADO ACTIVO POR AHORA
INSERT INTO CONTROL_ZETA.HOTEL_REGIMEN(HOTEL_ID, REG_ID, REG_ESTADO)
SELECT DISTINCT  
     CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE) AS HOTEL_ID,
      R.REG_ID, 'A'
FROM GD_ESQUEMA.MAESTRA M,
     CONTROL_ZETA.REGIMEN R
WHERE M.REGIMEN_DESCRIPCION = R.REG_DESCRIPCION; -- 64


--TABLA HABITACION

INSERT INTO CONTROL_ZETA.HABITACION
SELECT DISTINCT HABITACION_NUMERO,
       CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,M.HOTEL_NRO_CALLE,M.HOTEL_CALLE) AS HOTEL_ID,
       HABITACION_PISO,
       HABITACION_FRENTE,
       CONTROL_ZETA.GET_ID_TIPO_HABITACION(M.HABITACION_TIPO_DESCRIPCION)AS TIPO_HAB_ID
 FROM GD_ESQUEMA.MAESTRA M
ORDER BY HOTEL_ID, HABITACION_NUMERO, HABITACION_PISO ; -- 345


-- CONSULTA DE VERIFICACI�N, EXISTE UNA RESERVA POR PERSONA, 
-- LOS CODIGOS DE RESERVA SE REPITEN PERO LO QUE CAMBIA SON LOS CONSUMIBLES EN CADA REGISTRO

INSERT INTO CONTROL_ZETA.RESERVA (RESERVA_ID,  
            RESERVA_FECHA_INICIO, RESERVA_FECHA_HASTA, 
            RESERVA_ID_REGIMEN, RESERVA_ID_HOTEL, RESERVA_ESTADO, CLIENTE_ID)
SELECT  DISTINCT M.RESERVA_CODIGO,
        M.RESERVA_FECHA_INICIO,
        M.RESERVA_FECHA_INICIO + M.RESERVA_CANT_NOCHES AS FECHA_HASTA,
        R.REG_ID,
        CONTROL_ZETA.GET_ID_HOTEL(M.HOTEL_CIUDAD,
                                  M.HOTEL_NRO_CALLE,
                                  M.HOTEL_CALLE) ID_HOTEL,
        
        case  
        when RESERVA_FECHA_INICIO < GETDATE()
        then 'RSF'
        when RESERVA_FECHA_INICIO >= GETDATE()
        then 
        'RC'
        ELSE 
        'RI'END ESTADO,
        C.CLIENTE_ID
FROM GD_ESQUEMA.MAESTRA M , CONTROL_ZETA.REGIMEN R, CONTROL_ZETA.CLIENTE C
WHERE M.REGIMEN_DESCRIPCION = R.REG_DESCRIPCION
AND M.CLIENTE_APELLIDO = C.CLIENTE_APELLIDO
AND M.CLIENTE_NOMBRE = C.CLIENTE_NOMBRE
AND M.CLIENTE_PASAPORTE_NRO = C.CLIENTE_DOC; -- 100740


--Reserva con Factura: RI (Reserva con ingreso)
--Reserva sin factura pero con fecha de reserva posterior a la actual: RC (Reserva Correcta)
--Reserva sin Factura pero con fecha anterior a la actual: RSF (Reserva sin factura)


/*
INSERT INTO CONTROL_ZETA.ESTADIA (EST_RESERVA_ID, EST_FECHA_DESDE, EST_FECHA_HASTA)
SELECT DISTINCT R.RESERVA_ID,
       M.ESTADIA_FECHA_INICIO, 
       M.ESTADIA_FECHA_INICIO + M.ESTADIA_CANT_NOCHES AS FECHA_HASTA
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
   AND R.RESERVA_ID_REGIMEN = REG.REG_ID -- 100740
  */


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
   AND M.Factura_Nro IS NOT NULL  --89603
  
   

/*************************************************************************/
/**                    VER!!!!!!!!!!!                                   **/
/*************************************************************************/

--SELECT * FROM CONTROL_ZETA.ESTADIA_HAB_CON 

INSERT INTO CONTROL_ZETA.ESTADIA_HAB_CON (EST_ID, HAB_ID,CON_ID)
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
  AND M.CONSUMIBLE_CODIGO IS NOT NULL  -- 268809
  
--SELECT * FROM GD_ESQUEMA.MAESTRA M WHERE  M.CONSUMIBLE_CODIGO IS NOT NULL -- 268809
  
/*
VERIFICO 
SELECT * FROM GD_ESQUEMA.MAESTRA M
WHERE M.RESERVA_CODIGO = 100001
AND M.CONSUMIBLE_CODIGO IS NOT NULL -- 2 COCAS Y UN AGUA MINER
*/

INSERT INTO CONTROL_ZETA.RESERVA_HABITACION (RESERVA_ID, HAB_ID)
 
 select Ma.Reserva_Codigo,
        H.HAB_ID 
  from gd_esquema.Maestra Ma,
       CONTROL_ZETA.HABITACION H
 where CONTROL_ZETA.GET_ID_HOTEL(Ma.HOTEL_CIUDAD,Ma.HOTEL_NRO_CALLE,Ma.HOTEL_CALLE ) = H.HAB_ID_HOTEL
 and Ma.Habitacion_Numero = H.HAB_NRO  --100740


/*SELECT DISTINCT E.EST_RESERVA_ID ,EH.HAB_ID 
 FROM GD_ESQUEMA.MAESTRA M,
      CONTROL_ZETA.ESTADIA_HAB_CON EH,
      CONTROL_ZETA.ESTADIA E
WHERE M.RESERVA_CODIGO = E.EST_RESERVA_ID
AND E.EST_ID = EH.EST_ID      -- 89603*/
 
      
 
 
INSERT INTO CONTROL_ZETA.ESTADIA_CLIENTE (EST_ID, CLIENTE_ID) 
 SELECT DISTINCT E.EST_ID, R.CLIENTE_ID 
 FROM GD_ESQUEMA.MAESTRA M,
      CONTROL_ZETA.RESERVA R,
      CONTROL_ZETA.ESTADIA E
WHERE M.RESERVA_CODIGO = R.RESERVA_ID
 AND R.RESERVA_ID = E.EST_RESERVA_ID
 
  
 /******************FACTURAS*******************/
 
 
 INSERT INTO CONTROL_ZETA.FACTURA (FACTURA_NRO,FACTURA_FECHA,FACTURA_FORMA_PAGO ,EST_ID )
 SELECT DISTINCT M.FACTURA_NRO, M.FACTURA_FECHA,  'E',E.EST_ID
 FROM GD_ESQUEMA.MAESTRA M,
      CONTROL_ZETA.ESTADIA E
WHERE M.RESERVA_CODIGO = E.EST_RESERVA_ID
AND FACTURA_NRO IS NOT NULL
  --89603
 
 
 
 --select * from gd_esquema.Maestra where factura_nro = 2396745 
  
-- SELECT * FROM CONTROL_ZETA.ITEM_FACTURA
-- delete CONTROL_ZETA.ITEM_FACTURA
 --select * from CONTROL_ZETA.ITEM_FACTURA
 
 INSERT INTO CONTROL_ZETA.ITEM_FACTURA 
 (FACTURA_NRO, ITEM_FACTURA_CANTIDAD, ITEM_FACTURA_MONTO, ITEM_DESCRIPCION)
 (SELECT  M.FACTURA_NRO, M.ITEM_FACTURA_CANTIDAD ITEM_FACTURA_CANTIDAD, 
          M.ITEM_FACTURA_MONTO * M.Estadia_Cant_Noches, 'COSTO POR ESTADIA' 
 FROM GD_ESQUEMA.MAESTRA M
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL 
 AND Consumible_Codigo IS  NULL --and factura_nro = 2396782
  UNION all
 SELECT  M.FACTURA_NRO,  1 ITEM_FACTURA_CANTIDAD, 
         M.Consumible_Precio as ITEM_FACTURA_MONTO, 'CONSUMIBLES'
 FROM GD_ESQUEMA.MAESTRA M
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL 
   AND  Consumible_Codigo IS NOT NULL
 GROUP BY M.FACTURA_NRO, M.Factura_Total,M.Consumible_Precio)
 --AND factura_nro = 2396782
 UNION all
 SELECT  M.FACTURA_NRO,  
       case  C.CON_ES_MODERADO
       when 'S'
       then COUNT(1)
       else COUNT(0) end ITEM_FACTURA_CANTIDAD,
        -SUM(M.Consumible_Precio) as ITEM_FACTURA_MONTO, 
        'DESCUENTO ALL INCLUSIVE'
 FROM GD_ESQUEMA.MAESTRA M, CONTROL_ZETA.CONSUMIBLE C
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL
 and M.Regimen_Descripcion = 'All inclusive' --AND factura_nro = 2396782
 AND M.Consumible_Descripcion = C.CON_DESCRIPCION
 AND C.CON_ES_MODERADO = 'S' 
 group by M.FACTURA_NRO, M.Factura_Total , C.CON_ES_MODERADO, M.Consumible_Precio
 UNION ALL
  SELECT  M.FACTURA_NRO,  
       case  C.CON_ES_MODERADO
       when 'N'
       then COUNT(1)
       else COUNT(0) end ITEM_FACTURA_CANTIDAD,
        -SUM(M.Consumible_Precio) as ITEM_FACTURA_MONTO, 
        'DESCUENTO ALL INCLUSIVE MODERADO'
 FROM GD_ESQUEMA.MAESTRA M, CONTROL_ZETA.CONSUMIBLE C
 WHERE M.FACTURA_NRO IS NOT NULL AND ITEM_FACTURA_CANTIDAD IS NOT NULL
 and M.Regimen_Descripcion = 'All inclusive moderado' --AND factura_nro = 2396782
 AND M.Consumible_Descripcion = C.CON_DESCRIPCION
 AND C.CON_ES_MODERADO = 'N' 
 group by M.FACTURA_NRO, M.Factura_Total , C.CON_ES_MODERADO, M.Consumible_Precio
 ORDER BY M.FACTURA_NRO  -- 380972
 
 
 
   /*select factura_nro, SUM(item_factura_monto) 
 from CONTROL_ZETA.ITEM_FACTURA
 --where factura_nro = F.factura_nro
 group by factura_nro
 having SUM(item_factura_monto) = 1040
 
 
 
 SELECT * FROM CONTROL_ZETA.FACTURA
 */
 
 --UPDATE CONTROL_ZETA.FACTURA
  --SET FACTURA_TOTAL = NULL --89603
 
-- SELECT * FROM CONTROL_ZETA.FACTURA
 
 UPDATE CONTROL_ZETA.FACTURA 
  SET CONTROL_ZETA.FACTURA.FACTURA_TOTAL =
 (select SUM(IFA.item_factura_monto) AS MONTO
 from CONTROL_ZETA.ITEM_FACTURA IFA
 where IFA.factura_nro = CONTROL_ZETA.FACTURA.factura_nro) -- 89603
 
 
 -- sumar un item por factura con el campo facturo_monto (suma de consumibles)
 -- y ponerle la leyenda "descuento por regimen de estadia" --> all inclusive
 -- otro items con el total de los consumibles
 
 --SELECT * FROM CONTROL_ZETA.RESERVA -- 100740
 
 UPDATE CONTROL_ZETA.RESERVA 
 SET RESERVA_ESTADO = 'RI'
 WHERE EXISTS
 (SELECT DISTINCT R1.RESERVA_ID 
 FROM CONTROL_ZETA.ESTADIA E , CONTROL_ZETA.FACTURA F, CONTROL_ZETA.RESERVA R1
 WHERE E.EST_RESERVA_ID = R1.RESERVA_ID
  AND F.EST_ID = E.EST_ID ) -- 100740








  
 -- HAY 358412  FACTURAS
 /*SELECT M.Factura_Nro, R.REG_ID 
 FROM CONTROL_ZETA.FACTURA F,
      gd_esquema.Maestra M,
      CONTROL_ZETA.REGIMEN R
 WHERE F.FACTURA_NRO = M.Factura_Nro
 AND F.FACTURA_NRO IS NOT NULL
 AND M.Regimen_Descripcion = R.REG_DESCRIPCION*/



/* SELECT factura_nro, (Habitacion_Tipo_Porcentual * Regimen_Precio) + (Hotel_Recarga_Estrella *Hotel_CantEstrella ) as cuenta, 
 Item_Factura_Monto,
 Regimen_Descripcion, Factura_Total, 
 (Item_Factura_Monto - ((Habitacion_Tipo_Porcentual * Regimen_Precio) + (Hotel_Recarga_Estrella *Hotel_CantEstrella ))) as diferencia
  FROM gd_esquema.Maestra  
  WHERE Consumible_Descripcion is null and Item_Factura_Monto is not null
 -- and Regimen_Descripcion = 'Media Pensi�n'
 
*/ 
