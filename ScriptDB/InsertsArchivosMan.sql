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