select *
from gd_esquema.Maestra;

select 
Hotel_Calle, Hotel_CantEstrella, Hotel_Ciudad, Hotel_Nro_Calle,
Hotel_Recarga_Estrella
from gd_esquema.Maestra;






INSERT INTO CONTROL_ZETA.NACIONALIDAD
select distinct Cliente_Nacionalidad
from gd_esquema.Maestra;


-- Ciudades EN LOCALIDAD
INSERT INTO CONTROL_ZETA.LOCALIDAD
select distinct hotel_ciudad
from gd_esquema.Maestra;

INSERT INTO CONTROL_ZETA.TIPO_HAB (TIPO_HAB_DESCRIPCION, TIPO_HAB_PORC)
select  distinct habitacion_tipo_descripcion, Habitacion_Tipo_Porcentual
from gd_esquema.Maestra;

insert into CONTROL_ZETA.REGIMEN (REG_DESCRIPCION, REG_PRECIO,REG_ESTADO)
select  
distinct Regimen_Descripcion, Regimen_Precio, 'H'
from gd_esquema.Maestra;

select  
distinct Cliente_Apellido, Cliente_Nombre, Cliente_Pasaporte_Nro
from gd_esquema.Maestra;


