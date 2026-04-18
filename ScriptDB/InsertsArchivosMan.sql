/*

Base de Datos << ArchivosMan >>

 ********************************************
	Las siguientes instrucciones insertan los valores iniciales a las tablas .
	
	Para que la aplicación o al menos la administración de usuarios, se deben insertar valores a Roles.
 
 ********************************************

*/


-- Asignar el nombre correspondiente a la base de datos.
USE ArchivosMan
GO

/*
	Aquí podrán cambiar las descripciones de los roles correspondientes a los usuarios.
*/

-- Inserta Rol Super
DECLARE @rolDescripcion VARCHAR(30),
	@rolEsActivo BIT = 1,
	@rolFechaCrea DATETIME = GETDATE(),
	@rolUsuarioCrea INT = 0; -- Asignar Usuario Creador

SET @rolDescripcion = 'Super';
EXEC dbo.sp_RegistrarRol @descripcion = @rolDescripcion, @usuarioCrea = @rolUsuarioCrea

-- Rol Administrador
SET @rolDescripcion = 'Administrador';
EXEC dbo.sp_RegistrarRol @descripcion = @rolDescripcion, @usuarioCrea = @rolUsuarioCrea
GO	
-- Rol Supervisor
SET @rolDescripcion = 'Supervisor';
EXEC dbo.sp_RegistrarRol @descripcion = @rolDescripcion, @usuarioCrea = @rolUsuarioCrea
	
-- Rol Empleado
SET @rolDescripcion = 'Empleado';
EXEC dbo.sp_RegistrarRol @descripcion = @rolDescripcion, @usuarioCrea = @rolUsuarioCrea
GO	
--	Rol Contacto
SET @rolDescripcion = 'Contacto';
EXEC dbo.sp_RegistrarRol @descripcion = @rolDescripcion, @usuarioCrea = @rolUsuarioCrea
GO	

-- Inserta Usuario Super
DECLARE @usrNombre VARCHAR(50),
	@usrCorreo VARCHAR(50),
	@usrClave VARCHAR(100),
	@usrRolId INT = 0, -- Asignar Rol de Super Usuario
	@usrUrlFoto VARCHAR(500),
	@usrEsActivo BIT = 1,
	@usrFechaCrea DATETIME = GETDATE(),
	@usrUsuarioCrea INT = 0, -- Asignar Usuario Creador
	@usrNuevoUsuarioId INT, -- Variable para capturar el ID del nuevo usuario registrado
	@usrCodigoError VARCHAR(100); -- Variable para capturar el código de error en caso de fallo
-- Usuario: "Super"
SET @usrNombre = 'Super';
SET @usrCorreo = '<<Cambiar el valor correspondiente a Super ID=0';
SET @usrClave =  '1BdFD9Hgf2Vvre/zNx3Y+UQAJZiSLVfIT8ofIjBkxbs=';	// Debería ser '123'
SET @usrRolId = (SELECT idRol FROM dbo.Rol WHERE descripcion = 'Super' AND esActivo = 1);
SET @usrUrlFoto = '<< URL de imagen para perfil de usuario.';

INSERT INTO dbo.Usuario (nombre, correo, clave, rolId, urlFoto, esActivo, fechaCrea, usuarioCrea)
VALUES (@usrNombre, @usrCorreo, @usrClave, @usrRolId, @usrUrlFoto, @usrEsActivo, @usrFechaCrea, @usrUsuarioCrea)

-- Inserta valores para menus
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Herramientas', NULL, 'fa-solid fa-screwdriver-wrench', NULL, 'herramientas', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Administración', NULL, 'fas fa-fw fa-cog', NULL, 'administracion', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Ubicación', NULL, 'fa-solid fa-map-location-dot', NULL, 'ubicacion', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Archivos', 1, 'bi bi-file-image-fill-nav-menu', 'herramientas', 'archivos', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Proyectos', 1, 'bi bi-project-fill-nav-menu', 'herramientas', 'proyectos', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Usuarios', 2, 'bi bi-people-fill-nav-menu', 'administracion', 'usuarios', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Negocios', 2, 'bi bi-negocio-fill-nav-menu', 'administracion', 'negocios', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Países', 3, 'bi bi-globe-fill-nav-menu', 'ubicacion', 'paises', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Estados', 3, 'bi bi-map-fill-nav-menu', 'ubicacion', 'estados', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Login', NULL, NULL, NULL, 'login', 1, 0, );
INSERT INTO Menu (descripcion, menuPadreId, icono, controlador, paginaAccion, esActivo, usuarioCrea) 
VALUES ('Contactos', 2, 'bi bi-contact-fill-nav-menu', 'administracion', 'contactos', 1, 0, );

-- Inserta valores para RolMenu
-- idRol 0 = Super
INSERT INTO RolMenu(idRol, idMenu, esActivo, usuarioCrea)
SELECT 0 AS idRol, idMenu, 1, 1
FROM Menu;

-- idRol 1 = Administrador
INSERT INTO RolMenu(idRol, idMenu, esActivo, usuarioCrea)
SELECT 1 AS idRol, idMenu, 1, 1
FROM Menu;

-- idRol 2 = Supervisor
INSERT INTO RolMenu(idRol,idMenu,esActivo, usuarioCrea) values
(2,1,1,1),
(2,2,1,1),
(2,3,1,1),
(2,4,1,1),
(2,5,1,1),
(2,7,1,1),
(2,8,1,1),
(2,9,1,1)

-- idRol 3 = Empleado
INSERT INTO RolMenu(idRol,idMenu,esActivo, usuarioCrea) values
(3,1,1,1),
(3,2,1,1),
(3,3,1,1),
(3,4,1,1),
(3,5,1,1),
(3,8,1,1),
(3,9,1,1),
(3,11,1,1)

-- idRol 4 = Contacto
INSERT INTO RolMenu(idRol,idMenu,esActivo, usuarioCrea) values
(4,1,1,1),
(4,3,1,1),
(4,4,1,1),
(4,5,1,1),
(4,8,1,1),
(4,9,1,1)

