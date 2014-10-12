USE [GD2C2014]
GO

----------------------
--TABLAS PARAMETRICAS
----------------------

---Tabla PAIS
INSERT INTO CONTROL_ZETA.PAIS VALUES('ARGENTINA');

---TABLA NACIONALIDAD
INSERT INTO CONTROL_ZETA.NACIONALIDAD
SELECT DISTINCT CLIENTE_NACIONALIDAD FROM GD_ESQUEMA.MAESTRA;

---Tabla TIPO_DOC
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

--Tabla LOCALIDAD
INSERT INTO CONTROL_ZETA.LOCALIDAD 
SELECT DISTINCT HOTEL_CIUDAD FROM GD_ESQUEMA.MAESTRA;

---TABLA ROL
INSERT INTO CONTROL_ZETA.ROL VALUES ('ADMINISTRADOR', 'A');
INSERT INTO CONTROL_ZETA.ROL VALUES ('RECEPCIONISTA', 'A');
INSERT INTO CONTROL_ZETA.ROL VALUES ('GUEST', 'A');

---TABLA TIPO_HAB
INSERT INTO CONTROL_ZETA.TIPO_HAB (TIPO_HAB_ID,TIPO_HAB_DESCRIPCION, TIPO_HAB_PORC)
SELECT DISTINCT Habitacion_Tipo_Codigo, Habitacion_Tipo_Descripcion, Habitacion_Tipo_Porcentual 
FROM GD_ESQUEMA.Maestra ;



---TABLA CONSUMIBLE
INSERT INTO CONTROL_ZETA.CONSUMIBLE (CON_ID, CON_DESCRIPCION, CON_PRECIO) 
SELECT DISTINCT CONSUMIBLE_CODIGO, CONSUMIBLE_DESCRIPCION, CONSUMIBLE_PRECIO
FROM GD_ESQUEMA.MAESTRA
WHERE CONSUMIBLE_CODIGO IS NOT NULL;


---TABLA REGIMEN
INSERT INTO CONTROL_ZETA.REGIMEN (REG_DESCRIPCION, REG_PRECIO,REG_ESTADO)
SELECT  
DISTINCT REGIMEN_DESCRIPCION, REGIMEN_PRECIO, 'H'
FROM GD_ESQUEMA.MAESTRA;


--------------------
--TABLA DE DATOS----
--------------------


--select * from CONTROL_ZETA.CLIENTE

---TABLA CLIENTE
INSERT INTO CONTROL_ZETA.CLIENTE
( CLIENTE_NOMBRE, CLIENTE_ID_TIPO_DOC,CLIENTE_DOC,CLIENTE_MAIL, CLIENTE_TEL, 
 CLIENTE_ID_LOC,CLIENTE_ID_PAIS_ORIGEN,CLIENTE_DOM_CALLE,CLIENTE_DOM_NRO,CLIENTE_DPTO,
 CLIENTE_DOM_PISO, CLIENTE_NAC_ID,CLIENTE_ESTADO,CLIENTE_FECHA_NAC)
SELECT DISTINCT UPPER(CLIENTE_APELLIDO + ', ' + CLIENTE_NOMBRE  ) AS NOMBRE,
  1 AS TIPO_DOC,
  CLIENTE_PASAPORTE_NRO,
  CLIENTE_MAIL,
  NULL AS TEL,
    NULL AS LOCALIDAD, 
  NULL AS PAIS_ORIGEN, 
  CLIENTE_DOM_CALLE,  CLIENTE_NRO_CALLE,   CLIENTE_DEPTO,
  CLIENTE_PISO,  
  (SELECT  NAC_ID
   FROM CONTROL_ZETA.NACIONALIDAD 
  WHERE NAC_DETALLE = M.CLIENTE_NACIONALIDAD) AS NACIONALIDAD,
     'H',  CLIENTE_FECHA_NAC
 FROM GD_ESQUEMA.MAESTRA M
 ; -- 100740

SELECT COUNT(*) FROM GD_ESQUEMA.MAESTRA; -- 548755

SELECT * FROM GD_ESQUEMA.MAESTRA;

-- 87316 
select cliente_mail, COUNT(*) from gd_esquema.Maestra group by cliente_mail;




-- SAM
---TABLA USUARIO 
CREATE TABLE CONTROL_ZETA.USUARIO
(USR_USERNAME VARCHAR(50) NOT NULL,
USR_PASS VARBINARY (32) NOT NULL,
USR_NOMBRE VARCHAR(50) NOT NULL,
USR_APELLIDO VARCHAR(50) NOT NULL,
USR_ID_TIPO_DOC TINYINT ,
USR_DOC VARCHAR(15) ,
USR_MAIL VARCHAR(50) NOT NULL,
USR_TEL VARCHAR(10) ,
USR_DOM INT,
USR_FECHA_NAC DATE NOT NULL,
USR_ESTADO VARCHAR(1) NOT NULL,
CONSTRAINT PK_USR_USERNAME PRIMARY KEY(USR_USERNAME),
CONSTRAINT FK_USR_TIPO_DOC FOREIGN KEY (USR_ID_TIPO_DOC) REFERENCES CONTROL_ZETA.TIPO_DOC(TIPO_DOC_ID)
)
GO

---TABLA HOTEL
CREATE TABLE CONTROL_ZETA.HOTEL
(HOTEL_ID INT identity(1,1) NOT NULL,
HOTEL_NOMBRE VARCHAR(100),
HOTEL_MAIL VARCHAR(50),
HOTEL_TEL  VARCHAR(10),
HOTEL_CALLE VARCHAR(50),
HOTEL_NRO_CALLE SMALLINT,
HOTEL_ID_CIUDAD  TINYINT,  -- va con localidad?
HOTEL_CANT_ESTRELLA TINYINT,
HOTEL_PAIS  TINYINT, -- 1 hard?
HOTEL_RECARGA_ESTRELLA INT,
HOTEL_FECHA_CREACION DATE
CONSTRAINT PK_HOTEL_ID PRIMARY KEY(HOTEL_ID),
CONSTRAINT FK_HOTEL_PAIS FOREIGN KEY (HOTEL_PAIS) REFERENCES CONTROL_ZETA.PAIS(PAIS_ID)
)

GO

select HOTEL_CALLE,HOTEL_NRO_CALLE, HOTEL_ID_CIUDAD, 
        HOTEL_CANT_ESTRELLA, 1 as pais, 
        HOTEL_RECARGA_ESTRELLA,HOTEL_FECHA_CREACION
 from CONTROL_ZETA.HOTEL;

INSERT INTO CONTROL_ZETA.HOTEL
(HOTEL_CALLE,HOTEL_NRO_CALLE, HOTEL_ID_CIUDAD, 
        HOTEL_CANT_ESTRELLA, HOTEL_PAIS, 
        HOTEL_RECARGA_ESTRELLA,HOTEL_FECHA_CREACION)
select distinct Hotel_Calle, Hotel_Nro_Calle, 
  (SELECT LOC_ID FROM CONTROL_ZETA.LOCALIDAD WHERE LOC_DETALLE = Hotel_Ciudad),
Hotel_CantEstrella, 1 AS PAIS, Hotel_Recarga_Estrella,  GETDATE()AS FECHA
 from gd_esquema.Maestra; -- 16




---TABLA HOTEL_CIERRE
CREATE TABLE CONTROL_ZETA.HOTEL_CIERRE
(HOTEL_C_ID SMALLINT IDENTITY(1,1) NOT NULL,
HOTEL_ID INT,
HOTEL_C_FECHA_DESDE DATE,
HOTEL_C_FECHA_HASTA DATE,
HOTEL_C_MOTIVO VARCHAR(100)
CONSTRAINT PK_HOTEL_C_ID PRIMARY KEY(HOTEL_C_ID),
CONSTRAINT FK_HOTEL_ID_CIERRE FOREIGN KEY (HOTEL_ID) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID)
)
GO



---TABLA HABITACION
CREATE TABLE CONTROL_ZETA.HABITACION
(HAB_ID ??
HAB_NRO SMALLINT NOT NULL,
HAB_ID_HOTEL INT NOT NULL,
HAB_PISO SMALLINT NOT NULL,
HAB_FRENTE VARCHAR(1) NOT NULL,
HAB_ID_TIPO TINYINT ,
CONSTRAINT PK_HAB_NRO PRIMARY KEY(HAB_NRO),
CONSTRAINT FK_HAB_ID_HOTEL FOREIGN KEY (HAB_ID_HOTEL) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID),
CONSTRAINT FK_HAB_ID_TIPO FOREIGN KEY (HAB_ID_TIPO) REFERENCES CONTROL_ZETA.TIPO_HAB(TIPO_HAB_ID)
)
GO


INSERT INTO CONTROL_ZETA.HABITACION
SELECT DISTINCT Habitacion_Numero,
  (SELECT HOTEL_ID 
     FROM CONTROL_ZETA.HOTEL H, 
          CONTROL_ZETA.LOCALIDAD L
      WHERE HOTEL_CALLE = M.Hotel_Calle
        AND HOTEL_NRO_CALLE = M.Hotel_Nro_Calle
        AND HOTEL_ID_CIUDAD = L.LOC_ID
        AND L.LOC_DETALLE = M.Hotel_Ciudad) AS HOTEL_ID,
       Habitacion_Piso,
       Habitacion_Frente,
   --  HABITACION_TIPO_CODIGO,
       (SELECT TIPO_HAB_ID
       FROM CONTROL_ZETA.TIPO_HAB TH
       WHERE TH.TIPO_HAB_DESCRIPCION = M.Habitacion_Tipo_Descripcion) AS TIPO_HAB_ID
 FROM gd_esquema.Maestra M
 
 SELECT * FROM CONTROL_ZETA.TIPO_HAB
 
 
 SELECT * FROM gd_esquema.Maestra M


---TABLA RESERVA
CREATE TABLE CONTROL_ZETA.RESERVA
(RESERVA_ID INT NOT NULL,
RESERVA_FECHA DATE NOT NULL,
RESERVA_FECHA_INICIO DATE,
RESERVA_FECHA_HASTA DATE,
RESERVA_ID_REGIMEN TINYINT,
RESERVA_ID_HOTEL INT,
RESERVA_ESTADO VARCHAR(1),
CLIENTE_ID INT NOT NULL,
CONSTRAINT PK_RESERVA_ID PRIMARY KEY(RESERVA_ID),
CONSTRAINT FK_RESERVA_ID_REGIMEN FOREIGN KEY (RESERVA_ID_REGIMEN) REFERENCES CONTROL_ZETA.REGIMEN(REG_ID),
CONSTRAINT FK_RESERVA_ID_HOTEL FOREIGN KEY (RESERVA_ID_HOTEL) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID),
CONSTRAINT FK_RES_CLIENTE_ID FOREIGN KEY (CLIENTE_ID) REFERENCES CONTROL_ZETA.CLIENTE(CLIENTE_ID)
)
GO
---TABLA ESTADIA
CREATE TABLE CONTROL_ZETA.ESTADIA
(EST_ID INT NOT NULL,
EST_RESERVA_ID INT NOT NULL,
EST_FECHA_DESDE DATE,
EST_FECHA_HASTA DATE,
CONSTRAINT PK_EST_ID PRIMARY KEY(EST_ID),
CONSTRAINT FK_ESTA_RESERVA_ID FOREIGN KEY (EST_RESERVA_ID) REFERENCES CONTROL_ZETA.RESERVA(RESERVA_ID)
)

GO
---TABLA FACTURA
CREATE TABLE CONTROL_ZETA.FACTURA
(FACTURA_NRO INT NOT NULL,
FACTURA_FECHA DATE NOT NULL,
FACTURA_TOTAL DECIMAL(10,8) NOT NULL,
FACTURA_FORMA_PAGO INT NOT NULL,
FACTURA_NRO_TARJETA INT ,
FACTURA_COD_VERIFICADOR INT,
EST_ID INT NOT NULL,
FACTURA_TARJ_NRO VARCHAR,
FACTURA_TARJ_COD_SEG VARCHAR,
FACTURA_TARJ_FECHA_VENCIMIENTO DATE,
FACTURA_TARJ_NRO_CUOTAS INT,
CONSTRAINT PK_FACTURA_NRO PRIMARY KEY(FACTURA_NRO),
CONSTRAINT FK_FACT_EST_ID FOREIGN KEY (EST_ID) REFERENCES CONTROL_ZETA.ESTADIA(EST_ID)
)
GO
---TABLA ITEM_FACTURA
CREATE TABLE CONTROL_ZETA.ITEM_FACTURA
(ITEM_FACTURA_NRO INT NOT NULL,
FACTURA_NRO INT NOT NULL,
ITEM_FACTURA_CANTIDAD SMALLINT,
ITEM_FACTURA_MONTO DECIMAL(10,8)
CONSTRAINT PK_ITEM_FACTURA_NRO PRIMARY KEY(ITEM_FACTURA_NRO, FACTURA_NRO),
CONSTRAINT FK_ITEM_FACTURA_NRO FOREIGN KEY (FACTURA_NRO) REFERENCES CONTROL_ZETA.FACTURA(FACTURA_NRO)
)
GO


-------------------
--TABLAS RELACION--
-------------------
---TABLA ROL_FUNC
CREATE TABLE CONTROL_ZETA.ROL_FUNC
(ROL_ID TINYINT NOT NULL,
FUNC_ID TINYINT NOT NULL,
CONSTRAINT PK_ROL_FUNC_ID PRIMARY KEY(ROL_ID, FUNC_ID),
CONSTRAINT FK_ROL_ID FOREIGN KEY (ROL_ID) REFERENCES CONTROL_ZETA.ROL(ROL_ID),
CONSTRAINT FK_FUNC_ID FOREIGN KEY (FUNC_ID) REFERENCES CONTROL_ZETA.FUNCIONALIDAD(FUNC_ID)
)
GO
---TABLA USR_HOTEL
CREATE TABLE CONTROL_ZETA.USR_HOTEL
(USR_USERNAME VARCHAR(50) NOT NULL,
HOTEL_ID INT NOT NULL,
CONSTRAINT PK_USR_HOTEL_USERNAME PRIMARY KEY(USR_USERNAME,HOTEL_ID ),
CONSTRAINT FK_USR_USERNAME FOREIGN KEY (USR_USERNAME) REFERENCES CONTROL_ZETA.USUARIO(USR_USERNAME),
CONSTRAINT FK_HOTEL_ID FOREIGN KEY (HOTEL_ID) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID)
)

GO
---TABLA USR_ROL
CREATE TABLE CONTROL_ZETA.USR_ROL
(USR_USERNAME VARCHAR(50) NOT NULL,
ROL_ID TINYINT NOT NULL,
CONSTRAINT PK_USR_ROL_USERNAME PRIMARY KEY(USR_USERNAME,ROL_ID ),
CONSTRAINT FK_USR_ROL_USERNAME FOREIGN KEY (USR_USERNAME) REFERENCES CONTROL_ZETA.USUARIO(USR_USERNAME),
CONSTRAINT FK_USR_ROL_ID FOREIGN KEY (ROL_ID) REFERENCES CONTROL_ZETA.ROL(ROL_ID)
)

GO



---TABLA ESTADIA_CLIENTE
CREATE TABLE CONTROL_ZETA.ESTADIA_CLIENTE
(EST_ID INT IDENTITY(1,1) NOT NULL,
 CLIENTE_ID INT NOT NULL,
CONSTRAINT PK_EST_CLIENTE PRIMARY KEY(EST_ID,CLIENTE_ID),
CONSTRAINT FK_CLIENTE_ID FOREIGN KEY (CLIENTE_ID) REFERENCES CONTROL_ZETA.CLIENTE(CLIENTE_ID),
CONSTRAINT FK_EST_ID FOREIGN KEY (EST_ID) REFERENCES CONTROL_ZETA.ESTADIA(EST_ID)
)
GO
---TABLA ESTADIA_HAB_CON
CREATE TABLE CONTROL_ZETA.ESTADIA_HAB_CON
(EST_ID INT IDENTITY(1,1) NOT NULL,
 HAB_NRO SMALLINT NOT NULL,
 CON_ID SMALLINT NOT NULL,
CONSTRAINT PK_EST_HAB_CON PRIMARY KEY(EST_ID, HAB_NRO),
CONSTRAINT FK_EST_HAB_CON_HABITACION_ID FOREIGN KEY (HAB_NRO) REFERENCES CONTROL_ZETA.HABITACION(HAB_NRO),
CONSTRAINT FK_EST_HAB_CON_ESTADIA_ID FOREIGN KEY (EST_ID) REFERENCES CONTROL_ZETA.ESTADIA(EST_ID),
CONSTRAINT FK_EST_HAB_CON_CONSUMO_ID FOREIGN KEY (CON_ID) REFERENCES CONTROL_ZETA.CONSUMIBLE(CON_ID)
)
GO
---TABLA RESERVA_HABITACION
CREATE TABLE CONTROL_ZETA.RESERVA_HABITACION
(RESERVA_ID INT IDENTITY(1,1) NOT NULL,
 HAB_NRO SMALLINT NOT NULL,
CONSTRAINT PK_RESERVA_HAB PRIMARY KEY(RESERVA_ID, HAB_NRO),
CONSTRAINT FK_RESERVA_ID FOREIGN KEY (RESERVA_ID) REFERENCES CONTROL_ZETA.RESERVA(RESERVA_ID),
CONSTRAINT FK_RESERVA_HABITACION_HAB_NRO FOREIGN KEY (HAB_NRO) REFERENCES CONTROL_ZETA.HABITACION(HAB_NRO)
)
GO
---TABLA HOTEL_REGIMEN
CREATE TABLE CONTROL_ZETA.HOTEL_REGIMEN
(HOTEL_ID INT IDENTITY(1,1) NOT NULL,
 REG_ID TINYINT NOT NULL,
CONSTRAINT PK_HOTEL_REG PRIMARY KEY(HOTEL_ID,REG_ID),
CONSTRAINT FK_HOTEL_REGIMEN_HOTEL_ID FOREIGN KEY (HOTEL_ID) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID),
CONSTRAINT FK_HOTEL_REGIMEN_REG_ID FOREIGN KEY (REG_ID) REFERENCES CONTROL_ZETA.REGIMEN(REG_ID)
)

