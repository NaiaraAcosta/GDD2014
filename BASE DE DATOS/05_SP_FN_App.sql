---Referencia Accion
--1:Alta-Insert
--2:Modificacion-Update
--3:Borrado-Delete

----ABM ROL (Deberia cargar la tabla ROL, ROL_FUNC)
CREATE PROCEDURE CONTROL_ZETA.SP_ABM_ROL (@accion SMALLINT,@nombre varchar(20),@estado varchar(1))
as
begin
end;
---IMPORTANTE:Se tendria que hacer otro SP para asociar el ROL con la Funcionalidad y que desde la app se ejecute varias veces


---ASIGNACION DE FUNCIONES A ROL (Carga tabla ROL_FUNC)
CREATE PROCEDURE CONTROL_ZETA.SP_ROL_FUNC(@accion SMALLINT,@rol_id TINYINT, @func_id TINYINT)
AS
BEGIN
END;


---LOGIN (Retorna 1 si loggea ok, 0 si loggea nok)
CREATE FUNCTION CONTROL_ZETA.SP_LOGIN (@usuario varchar (50),@pass varchar(70))
returns tinyint
as
begin
return;
end;

---ABM USUARIO (Carga tabla USUARIO y EMPLEADO) FALTA: poner tipo de dato a dom
CREATE PROCEDURE CONTROL_ZETA.SP_ABM_USUARIO
(@accion SMALLINT,@usuario varchar (50),@pass varchar(70),@nombre VARCHAR(50),@apellido VARCHAR(50),@tipo_doc TINYINT,@doc VARCHAR(15),@mail VARCHAR(50),@tel VARCHAR(10),@dom,@fecha_nac date,@estado VARCHAR(1))
as
begin
end;

---ASIGNACION DE USUARIO_ROL_HOTEL (Carga tabla USR_ROL_HOTEL)
CREATE PROCEDURE CONTROL_ZETA.SP_USR_ROL_HOTEL (@accion SMALLINT,@usuario varchar (50),@rol_id TINYINT,@hotel_id INT)
as
begin
end;

---ABM CLIENTE (Carga tabla clientes)
CREATE PROCEDURE CONTROL_ZETA.SP_CLIENTE(@accion SMALLINT,@nombre VARCHAR(50), @apellido VARCHAR(50), @id_tipo_doc TINYINT, @idpais TINYINT, @dom_calle VARCHAR(70),
@dom_nro  INT, @dom_dpto VARCHAR(2), @dom_piso VARCHAR(10), @nac_id TINYINT, @estado VARCHAR(1) )
as
begin
end;



---ABM HOTEL

---ABM HABITACION

---ALTA RESERVA

---MODIFICACION RESERVA

---CANCELAR RESERVA

---CHECK-IN

---CHECK OUT

---REGISTRAR CONSUMIBLES

---FACTURAR ESTADIA

---ESTADISTICA