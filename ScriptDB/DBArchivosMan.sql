/*
 Base de Datos ArchivosMan 
 
 ********************************************
	Estás tablas son las básicas para que el proyecto, como quiera que le llamen, pueda funcionar.
	Con ellas podrán administrar los usuarios de acceso a la base de datos.
	
	Sistema de manejo de archivo con control de usuarios.
	
															MemoRodz, 2020409
 ********************************************
*/

-- Base de datos, podrán cambiar el nombre, sólo no olviden hacer el cambio correspondiente en la cadena de conexión de la aplicación correspondiente.
CREATE DATABASE ArchivosMan
GO

USE ArchivosMan
GO

-- Tablas de Sistema
CREATE TABLE Rol(
 idRol int primary key identity(0,1),
 descripcion varchar(30),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

CREATE TABLE Usuario(
 idUsuario int primary key identity(0,1),
 nombre varchar(50) not null,
 correo varchar(50) not null,
 clave varchar(100) not null,
 rolId int references Rol(idRol),
 urlFoto varchar(500),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

CREATE TABLE Negocio(
 idNegocio int primary key identity(0,1),
 nombre varchar(50) not null,
 correo varchar(50) not null,
 urlLogo varchar(100),
 nombreLogo varchar(100),
 direccion varchar(50),
 codpos varchar(5) not null,
 pais varchar(50) not null,
 telefono varchar(50),
 porcentajeImpuesto decimal(10,2),
 simboloMoneda varchar(5),
 numeroDocumento varchar(50),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

CREATE TABLE Configuracion(
 idConfiguracion int primary key identity(1,1),
 recurso varchar(50),
 propiedad varchar(50),
 valor varchar(50),
 negocioId int references Negocio(idNegocio),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

create table Menu(
 idMenu int primary key identity(1,1),
 descripcion varchar(30),
 menuPadreId int references Menu(idMenu),
 icono varchar(30),
 controlador varchar(30),
 paginaAccion varchar(30),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO
 
create table RolMenu(
 idRolMenu int primary key identity(1,1),
 idRol int references Rol(idRol),
 idMenu int references Menu(idMenu),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO
create table Pais(
 idPais int primary key identity(1,1),
 nombre varchar(50) not null,
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO
create table Estado(
 idEstado int primary key identity(1,1),
 nombre varchar(50) not null,
 paisId int references Pais(idPais),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

CREATE TABLE Categoria(
 idCategoria int primary key identity(1,1),
 nombre varchar(50) not null,
 descripcion varchar(100),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

-- Tablas de Proyecto

CREATE TABLE Archivo(
 idArchivo int primary key identity(1,1),
 nombre varchar(100) not null,
 url varchar(200) not null,
 categoriaId int References Categoria(idCategoria),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

CREATE TABLE Proyecto(
 idProyecto int primary key identity(1,1),
 nombre varchar(50) not null,
 iconoId int References Archivo(idArchivo), 
 descripcion varchar(100),
 fechaInicio datetime,
 fechaFin datetime,
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO

CREATE TABLE Contacto(
 idContacto int primary key identity(1,1),
 nombre varchar(50) not null,
 correo varchar(50) not null,
 telefono varchar(20),
 esActivo bit not null default 1,
 fechaCrea datetime default getdate(),
 usuarioCrea int not null,
 fechaActualiza datetime,
 usuarioActualiza int
)
GO