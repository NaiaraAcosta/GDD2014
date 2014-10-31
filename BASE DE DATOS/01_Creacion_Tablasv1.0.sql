USE [GD2C2014]
GO


----------------------
--TABLAS PARAMETRICAS
----------------------

---Tabla PAIS
CREATE TABLE CONTROL_ZETA.PAIS
(PAIS_ID TINYINT IDENTITY(1,1) not NULL,
PAIS_DETALLE VARCHAR(50) NOT NULL,
CONSTRAINT PK_PAIS_ID PRIMARY KEY(PAIS_ID)
)
GO

---Tabla NACIONALIDAD
CREATE TABLE CONTROL_ZETA.NACIONALIDAD
(NAC_ID TINYINT IDENTITY(1,1) not NULL,
NAC_DETALLE VARCHAR(50) NOT NULL,
CONSTRAINT PK_NAC_ID PRIMARY KEY(NAC_ID)
)
GO

---Tabla TIPO_DOC
CREATE TABLE CONTROL_ZETA.TIPO_DOC
(TIPO_DOC_ID TINYINT IDENTITY(1,1) not NULL,
TIPO_DOC_DETALLE VARCHAR(50) NOT NULL,
CONSTRAINT PK_TIPO_DOC_ID PRIMARY KEY(TIPO_DOC_ID)
)
GO

---Tabla FUNCIONALIDAD
CREATE TABLE CONTROL_ZETA.FUNCIONALIDAD
(FUNC_ID TINYINT IDENTITY(1,1) not NULL,
FUNC_DETALLE VARCHAR(70) NOT NULL,
CONSTRAINT PK_FUNC_ID PRIMARY KEY(FUNC_ID)
)
GO

--Tabla LOCALIDAD
CREATE TABLE CONTROL_ZETA.LOCALIDAD
(LOC_ID TINYINT IDENTITY(1,1) not NULL,
LOC_DETALLE VARCHAR(50) NOT NULL,
CONSTRAINT PK_LOC_ID PRIMARY KEY(LOC_ID)
)
GO

---Tabla ROL
CREATE TABLE CONTROL_ZETA.ROL
(ROL_ID TINYINT IDENTITY(1,1) not NULL,
ROL_NOMBRE VARCHAR(20) NOT NULL,
ROL_ESTADO VARCHAR(1) NOT NULL,
CONSTRAINT PK_ROL_ID PRIMARY KEY(ROL_ID)
)
GO

---Tabla TIPO_HAB
CREATE TABLE CONTROL_ZETA.TIPO_HAB
(TIPO_HAB_ID SMALLINT not NULL,
TIPO_HAB_DESCRIPCION VARCHAR(50) NOT NULL, 
TIPO_HAB_PORC DECIMAL,
CONSTRAINT PK_TIPO_HAB_ID PRIMARY KEY(TIPO_HAB_ID)
)
GO

---Tabla CONSUMIBLE
CREATE TABLE CONTROL_ZETA.CONSUMIBLE
(CON_ID SMALLINT  not NULL,
CON_DESCRIPCION VARCHAR(50) NOT NULL,
CON_PRECIO DECIMAL(10,2) NOT NULL,
CON_ES_MODERADO VARCHAR(1),
CONSTRAINT PK_CON_ID PRIMARY KEY(CON_ID)
)
GO

---Tabla REGIMEN
CREATE TABLE CONTROL_ZETA.REGIMEN
(REG_ID TINYINT IDENTITY(1,1) not NULL,
REG_DESCRIPCION VARCHAR(50) NOT NULL,
REG_PRECIO DECIMAL(10,2) NOT NULL,
CONSTRAINT PK_REG_ID PRIMARY KEY(REG_ID)
)
GO

---Tabla ESTADO_RESERVA

CREATE TABLE CONTROL_ZETA.ESTADO_RESERVA
(ESTADO_ID TINYINT IDENTITY(1,1) not NULL,
 ESTADO_DESCRIPCION VARCHAR(50) NOT NULL,
 ESTADO_DESCRIP_CORTA VARCHAR(4) NOT NULL,
 CONSTRAINT PK_ESTADO_DESC_RESERVA PRIMARY KEY(ESTADO_DESCRIP_CORTA)
)
GO

--------------------
--TABLA DE DATOS----
--------------------

---Tabla CLIENTE


CREATE TABLE CONTROL_ZETA.CLIENTE
(CLIENTE_ID numeric IDENTITY(1,1) not NULL,-->Genera numero correlativo
CLIENTE_NOMBRE VARCHAR(50) not NULL,
CLIENTE_APELLIDO VARCHAR(50) not NULL,
CLIENTE_ID_TIPO_DOC TINYINT not null,
CLIENTE_DOC VARCHAR(15) ,
CLIENTE_MAIL VARCHAR(50) not NULL,
CLIENTE_TEL VARCHAR(10) ,
CLIENTE_ID_LOC TINYINT ,
CLIENTE_ID_PAIS_ORIGEN TINYINT ,
CLIENTE_DOM_CALLE VARCHAR(70),
CLIENTE_DOM_NRO INT,
CLIENTE_DPTO VARCHAR(2),
CLIENTE_DOM_PISO VARCHAR(10),
CLIENTE_NAC_ID TINYINT,
CLIENTE_ESTADO VARCHAR(1) NOT NULL,
CLIENTE_FECHA_NAC DATE NOT NULL,
CONSTRAINT PK_CLIENTE_ID PRIMARY KEY(CLIENTE_ID),
CONSTRAINT FK_TIPO_DOC FOREIGN KEY (CLIENTE_ID_TIPO_DOC) REFERENCES CONTROL_ZETA.TIPO_DOC(TIPO_DOC_ID),
--CONSTRAINT U_MAIL UNIQUE(CLIENTE_MAIL), --Se saca ya que hay clientes con mail repetido
CONSTRAINT FK_LOCALIDAD FOREIGN KEY (CLIENTE_ID_LOC) REFERENCES CONTROL_ZETA.LOCALIDAD(LOC_ID),
CONSTRAINT FK_PAIS FOREIGN KEY (CLIENTE_ID_PAIS_ORIGEN) REFERENCES CONTROL_ZETA.PAIS(PAIS_ID),
CONSTRAINT FK_NAC FOREIGN KEY (CLIENTE_NAC_ID)REFERENCES CONTROL_ZETA.NACIONALIDAD(NAC_ID)
)

GO

---Tabla EMPLEADO
CREATE TABLE CONTROL_ZETA.EMPLEADO
(USR_USERNAME VARCHAR(50) not NULL,
EMP_NOMBRE VARCHAR(50) not NULL,
EMP_APELLIDO VARCHAR(50) not NULL,
EMP_ID_TIPO_DOC TINYINT ,
EMP_DOC VARCHAR(15) ,
EMP_MAIL VARCHAR(50) not NULL,
EMP_TEL VARCHAR(10) ,
EMP_DOM VARCHAR(50),
EMP_FECHA_NAC DATE NOT NULL,
CONSTRAINT PK_USR_USERNAME_EMP PRIMARY KEY(USR_USERNAME),
CONSTRAINT FK_EMP_TIPO_DOC FOREIGN KEY (EMP_ID_TIPO_DOC) REFERENCES CONTROL_ZETA.TIPO_DOC(TIPO_DOC_ID)
)

GO
---Tabla USUARIO 
CREATE TABLE CONTROL_ZETA.USUARIO
(USR_USERNAME VARCHAR(50) not NULL,
USR_PASS VARCHAR (100) not NULL,---Cambie el tipo porque se va a poner el encriptado ahi 
USR_ESTADO VARCHAR(1) NOT NULL,
USR_INTENTOS TINYINT not null,
CONSTRAINT PK_USR_USERNAME PRIMARY KEY(USR_USERNAME),
CONSTRAINT FK_USR_USERNAME FOREIGN KEY (USR_USERNAME) REFERENCES CONTROL_ZETA.EMPLEADO(USR_USERNAME)
)



GO

---Tabla HOTEL
CREATE TABLE CONTROL_ZETA.HOTEL
(HOTEL_ID INT IDENTITY(1,1) not NULL,
HOTEL_NOMBRE VARCHAR(100),
HOTEL_MAIL VARCHAR(50),
HOTEL_TEL  VARCHAR(10),
HOTEL_CALLE VARCHAR(50),
HOTEL_NRO_CALLE SMALLINT,
HOTEL_ID_LOC TINYINT,
HOTEL_CANT_ESTRELLA TINYINT,
HOTEL_PAIS  TINYINT,
HOTEL_RECARGA_ESTRELLA INT,
HOTEL_FECHA_CREACION DATE
CONSTRAINT PK_HOTEL_ID PRIMARY KEY(HOTEL_ID),
CONSTRAINT FK_LOCALIDAD_HOTEL FOREIGN KEY (HOTEL_ID_LOC) REFERENCES CONTROL_ZETA.LOCALIDAD(LOC_ID),
CONSTRAINT FK_HOTEL_PAIS FOREIGN KEY (HOTEL_PAIS) REFERENCES CONTROL_ZETA.PAIS(PAIS_ID)
)

GO
---Tabla HOTEL_CIERRE
CREATE TABLE CONTROL_ZETA.HOTEL_CIERRE
(HOTEL_C_ID SMALLINT IDENTITY(1,1) not NULL,
HOTEL_ID INT,
HOTEL_C_FECHA_DESDE DATE,
HOTEL_C_FECHA_HASTA DATE,
HOTEL_C_MOTIVO VARCHAR(100)
CONSTRAINT PK_HOTEL_C_ID PRIMARY KEY(HOTEL_C_ID),
CONSTRAINT FK_HOTEL_ID_CIERRE FOREIGN KEY (HOTEL_ID) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID)
)
GO



---Tabla HABITACION
CREATE TABLE CONTROL_ZETA.HABITACION
( HAB_ID numeric IDENTITY(1,1) NOT NULL,
HAB_NRO SMALLINT not NULL,
HAB_ID_HOTEL INT not NULL,
HAB_PISO SMALLINT not NULL,
HAB_FRENTE VARCHAR(1) not NULL,
HAB_ID_TIPO SMALLINT ,
--HAB_OBSERVACION VARCHAR(100), SF CAMPO PROPUESTO
CONSTRAINT PK_HAB_ID PRIMARY KEY(HAB_ID),
CONSTRAINT FK_HAB_ID_HOTEL FOREIGN KEY (HAB_ID_HOTEL) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID),
CONSTRAINT FK_HAB_ID_TIPO FOREIGN KEY (HAB_ID_TIPO) REFERENCES CONTROL_ZETA.TIPO_HAB(TIPO_HAB_ID),
CONSTRAINT U_HAB_HOTEL UNIQUE(HAB_NRO,HAB_ID_HOTEL)
)
GO

---Tabla RESERVA

CREATE TABLE CONTROL_ZETA.RESERVA
(RESERVA_ID numeric not NULL,
RESERVA_FECHA DATE ,
RESERVA_FECHA_INICIO DATE,
RESERVA_FECHA_HASTA DATE,
RESERVA_ID_REGIMEN TINYINT,
RESERVA_ID_HOTEL INT,
RESERVA_ESTADO VARCHAR(4) NOT NULL,
CLIENTE_ID NUMERIC NOT NULL,
CONSTRAINT PK_RESERVA_ID PRIMARY KEY(RESERVA_ID),
CONSTRAINT FK_RESERVA_ID_REGIMEN FOREIGN KEY (RESERVA_ID_REGIMEN) REFERENCES CONTROL_ZETA.REGIMEN(REG_ID),
CONSTRAINT FK_RESERVA_ID_HOTEL FOREIGN KEY (RESERVA_ID_HOTEL) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID),
CONSTRAINT FK_RES_CLIENTE_ID FOREIGN KEY (CLIENTE_ID) REFERENCES CONTROL_ZETA.CLIENTE(CLIENTE_ID),
CONSTRAINT PK_ESTADO_RESERVA FOREIGN KEY (RESERVA_ESTADO) REFERENCES CONTROL_ZETA.ESTADO_RESERVA (ESTADO_DESCRIP_CORTA),
)
GO
---Tabla ESTADIA
CREATE TABLE CONTROL_ZETA.ESTADIA
(EST_ID numeric IDENTITY(1,1) not NULL,
EST_RESERVA_ID numeric not NULL,
EST_FECHA_DESDE DATE,
EST_FECHA_HASTA DATE,
CONSTRAINT PK_EST_ID PRIMARY KEY(EST_ID),
CONSTRAINT FK_ESTA_RESERVA_ID FOREIGN KEY (EST_RESERVA_ID) REFERENCES CONTROL_ZETA.RESERVA(RESERVA_ID)
)

GO
---Tabla FACTURA

CREATE TABLE CONTROL_ZETA.FACTURA
(FACTURA_NRO numeric not NULL,
FACTURA_FECHA DATE not NULL,
FACTURA_TOTAL INT,
FACTURA_FORMA_PAGO varchar(2) not NULL,
FACTURA_NRO_TARJETA INT ,
FACTURA_COD_VERIFICADOR INT,
EST_ID NUMERIC NOT NULL,
FACTURA_TARJ_NRO VARCHAR(25),
FACTURA_TARJ_COD_SEG VARCHAR(4),
FACTURA_TARJ_FECHA_VENCIMIENTO DATE,
FACTURA_TARJ_NRO_CUOTAS INT,
CONSTRAINT PK_FACTURA_NRO PRIMARY KEY(FACTURA_NRO),
CONSTRAINT FK_FACT_EST_ID FOREIGN KEY (EST_ID) REFERENCES CONTROL_ZETA.ESTADIA(EST_ID)
)
GO
---Tabla ITEM_FACTURA

CREATE TABLE CONTROL_ZETA.ITEM_FACTURA
(ITEM_FACTURA_NRO INT identity(1,1) not NULL,
FACTURA_NRO numeric not NULL,
ITEM_FACTURA_CANTIDAD SMALLINT,
ITEM_FACTURA_MONTO int,
ITEM_DESCRIPCION VARCHAR(50),
CONSTRAINT PK_ITEM_FACTURA_NRO PRIMARY KEY(ITEM_FACTURA_NRO, FACTURA_NRO),
CONSTRAINT FK_ITEM_FACTURA_NRO FOREIGN KEY (FACTURA_NRO) REFERENCES CONTROL_ZETA.FACTURA(FACTURA_NRO)
)
GO


-------------------
--TABLAS RELACION--
-------------------
---Tabla ROL_FUNC
CREATE TABLE CONTROL_ZETA.ROL_FUNC
(ROL_ID TINYINT NOT NULL,
FUNC_ID TINYINT NOT NULL,
CONSTRAINT PK_ROL_FUNC_ID PRIMARY KEY(ROL_ID, FUNC_ID),
CONSTRAINT FK_ROL_ID FOREIGN KEY (ROL_ID) REFERENCES CONTROL_ZETA.ROL(ROL_ID),
CONSTRAINT FK_FUNC_ID FOREIGN KEY (FUNC_ID) REFERENCES CONTROL_ZETA.FUNCIONALIDAD(FUNC_ID)
)
GO

---Tabla USR_ROL_HOTEL
CREATE TABLE CONTROL_ZETA.USR_ROL_HOTEL
(USR_USERNAME VARCHAR(50) NOT NULL,
HOTEL_ID INT NOT NULL,
ROL_ID TINYINT NOT NULL,
CONSTRAINT PK_USR_ROL_USERNAME PRIMARY KEY(USR_USERNAME,ROL_ID,HOTEL_ID ),
CONSTRAINT FK_USR_ROL_USERNAME FOREIGN KEY (USR_USERNAME) REFERENCES CONTROL_ZETA.USUARIO(USR_USERNAME),
CONSTRAINT FK_USR_ROL_ID FOREIGN KEY (ROL_ID) REFERENCES CONTROL_ZETA.ROL(ROL_ID),
CONSTRAINT FK_HOTEL_ID FOREIGN KEY (HOTEL_ID) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID)
)
GO

---Tabla ESTADIA_CLIENTE

CREATE TABLE CONTROL_ZETA.ESTADIA_CLIENTE
(EST_ID numeric  not NULL,
 CLIENTE_ID numeric NOT NULL,
CONSTRAINT PK_EST_CLIENTE PRIMARY KEY(EST_ID,CLIENTE_ID),
CONSTRAINT FK_CLIENTE_ID FOREIGN KEY (CLIENTE_ID) REFERENCES CONTROL_ZETA.CLIENTE(CLIENTE_ID),
CONSTRAINT FK_EST_ID FOREIGN KEY (EST_ID) REFERENCES CONTROL_ZETA.ESTADIA(EST_ID)
)
GO

-----   Tabla ESTADIA_HAB_CON --> TIENE   REPETIDOS
CREATE TABLE CONTROL_ZETA.ESTADIA_HAB_CON
(EST_ID numeric  not NULL,
 HAB_ID numeric NOT NULL,
 CON_ID SMALLINT NOT NULL,
CONSTRAINT FK_EST_HAB_CON_HABITACION_ID FOREIGN KEY (HAB_ID) REFERENCES CONTROL_ZETA.HABITACION(HAB_ID),
CONSTRAINT FK_EST_HAB_CON_ESTADIA_ID FOREIGN KEY (EST_ID) REFERENCES CONTROL_ZETA.ESTADIA(EST_ID),
CONSTRAINT FK_EST_HAB_CON_CONSUMO_ID FOREIGN KEY (CON_ID) REFERENCES CONTROL_ZETA.CONSUMIBLE(CON_ID)
)
GO

---Tabla RESERVA_HABITACION
CREATE TABLE CONTROL_ZETA.RESERVA_HABITACION
(RESERVA_ID numeric  not NULL,
 HAB_ID numeric NOT NULL,
CONSTRAINT PK_RESERVA_HAB PRIMARY KEY(RESERVA_ID, HAB_ID),
CONSTRAINT FK_RESERVA_ID FOREIGN KEY (RESERVA_ID) REFERENCES CONTROL_ZETA.RESERVA(RESERVA_ID),
CONSTRAINT FK_RESERVA_HABITACION_HAB_NRO FOREIGN KEY (HAB_ID) REFERENCES CONTROL_ZETA.HABITACION(HAB_ID)
)
GO

---Tabla HOTEL_REGIMEN
CREATE TABLE CONTROL_ZETA.HOTEL_REGIMEN
(HOTEL_ID INT  not NULL,
 REG_ID TINYINT NOT NULL,
 REG_ESTADO VARCHAR(1) NOT NULL,
CONSTRAINT PK_HOTEL_REG PRIMARY KEY(HOTEL_ID,REG_ID),
CONSTRAINT FK_HOTEL_REGIMEN_HOTEL_ID FOREIGN KEY (HOTEL_ID) REFERENCES CONTROL_ZETA.HOTEL(HOTEL_ID),
CONSTRAINT FK_HOTEL_REGIMEN_REG_ID FOREIGN KEY (REG_ID) REFERENCES CONTROL_ZETA.REGIMEN(REG_ID)
)
