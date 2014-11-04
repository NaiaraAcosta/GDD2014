GO
drop function control_zeta.get_ciudad;
GO
create function control_zeta.get_ciudad(@p_Hotel_Ciudad varchar(50))
returns tinyint
as
begin
return (SELECT LOC_ID FROM CONTROL_ZETA.LOCALIDAD WHERE LOC_DETALLE = @p_Hotel_Ciudad);
end;

GO
drop function control_zeta.get_id_hotel;
GO
create function control_zeta.get_id_hotel(@p_ciudad VARCHAR(50),
                                          @p_hotel_nro_calle SMALLINT,
                                          @p_hotel_calle   VARCHAR(50) )
returns tinyint
as
begin
return   (SELECT HOTEL_ID 
     FROM CONTROL_ZETA.HOTEL H, 
          CONTROL_ZETA.LOCALIDAD L
      WHERE HOTEL_CALLE = @p_hotel_calle
        AND HOTEL_NRO_CALLE = @p_hotel_nro_calle
        AND HOTEL_ID_LOC = L.LOC_ID
        AND L.LOC_DETALLE = @p_ciudad);
end;

GO
drop function control_zeta.get_id_tipo_habitacion;
GO
create function control_zeta.get_id_tipo_habitacion(@p_descripcion VARCHAR(50))
returns smallint
as
begin
return  (SELECT TIPO_HAB_ID 
           FROM CONTROL_ZETA.TIPO_HAB TH  
           WHERE TH.TIPO_HAB_DESCRIPCION = @p_descripcion)
end;


