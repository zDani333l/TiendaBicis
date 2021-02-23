Create database dbTiendaOnlineBicicletas
go
use dbTiendaOnlineBicicletas
go


CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (150)  NOT NULL,
    [ContextKey]     NVARCHAR (300)  NOT NULL,
    [Model]          VARBINARY (MAX) NOT NULL,
    [ProductVersion] NVARCHAR (32)   NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC)
)

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [IsActive] bit null,
    [IsDelete] bit null,
    [FechaCreacion] datetime,
    [FechaModificacion] datetime
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)

create table Tbl_Categoria( 
	CategoriaId int identity primary key,
	NombreCategoria varchar(500) unique,
	DescripcionCategoria varchar(500) unique,
	IsActive bit null,
	IsDelete bit null
)

create table Tbl_SubCategoria(
	IdSubCategoria int identity primary key,
	NombreSubCategoria varchar(500) unique,
	DescripcionSubCategoria varchar(500) unique,
	CategoriaId int,
	foreign key (CategoriaId) references Tbl_Categoria(CategoriaId)
)

create table Tbl_Proveedor (
	ProveedorId int identity primary key,
	nombre varchar(200),
	direccion varchar (200)
)


create table Tbl_Producto (
	ProductoId int identity primary key,
	NombreProducto varchar(500) unique,
	IsActive bit null,
	IsDelete bit null,
	Descripcion varchar(500),
	Cantidad int,
	FechaCreacion datetime null,
	FechaModificacion datetime null,
	Precio decimal NULL,
	Talla varchar(10) NULL,
	Color varchar(50) NULL,
	ProveedorId int,
	IdSubCategoria int,
	IdCategoria int,
	foreign key (IdSubCategoria) references Tbl_SubCategoria(IdSubCategoria),
	foreign key (IdCategoria) references Tbl_Categoria(CategoriaId),
	foreign key (ProveedorId) references Tbl_Proveedor(ProveedorId)
	
)
 
create table Tbl_imagen(
	IdImagenes int identity primary key,
	NombreImagen varchar(200),
	DirImagen varchar(500),
	IdProducto int,
	foreign key (IdProducto) references Tbl_Producto(ProductoId),
)

create table Tbl_Carro(
	CarroId int identity primary key,
	IdUsuario NVARCHAR (128),
	foreign key (IdUsuario) references AspNetUsers(Id)
)

create table Tbl_OrdenStatus(
	OrdenStatusId  int identity primary key,
	OrdenStatus varchar(500)
)

create table Tbl_DetalleCompra (
	DetalleCompraId int identity primary key,
	Direccion varchar(500),
	Ciudad varchar(500),
	Estado varchar(500),
	Pais varchar(50),
	CodigoPostal varchar(15),
	Monto decimal,
	TipoPago varchar(50),
	IdUsuario NVARCHAR (128),
	IdCarrito int,
	IdOrdenStatus int,
	foreign key (IdUsuario) references AspNetUsers(Id),
	foreign key (IdCarrito) references Tbl_Carro(CarroId),
	foreign key (IdOrdenStatus) references Tbl_OrdenStatus(OrdenStatusId)
)


CREATE TABLE [dbo].[Tbl_CarroProducto] (
    IdList INT NOT NULL IDENTITY(1,1) primary key,
    IdCarro INT NOT NULL,
    ProductoId INT NOT NULL,
    foreign key (IdCarro) references Tbl_Carro(CarroId),
    foreign key (ProductoId) references Tbl_Producto(ProductoId),
)



USE [dbTiendaOnlineBicicletas]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetBySearch]
	@search nvarchar(max)=null
AS
BEGIN
	SELECT * from [dbo].[Tbl_Producto] P 
	left join [dbo].[Tbl_SubCategoria] SC on p.IdSubCategoria=SC.CategoriaId left join [dbo].[Tbl_Categoria] C on SC.IdSubCategoria=C.CategoriaId
	where 
	P.NombreProducto LIKE CASE WHEN @search is not null then  '%'+@search+'%' else P.NombreProducto end
	OR 
	SC.NombreSubCategoria LIKE CASE WHEN @search is not null then  '%'+@search+'%' else SC.NombreSubCategoria end
	OR
	C.NombreCategoria LIKE CASE WHEN @search is not null then '%'+@search+'%' else C.NombreCategoria end
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetIdRol]
	@NombreUser nvarchar(max)=null
AS
BEGIN
	SELECT R.Name,R.Id FROM AspNetUserRoles UR,AspNetRoles R, AspNetUsers U where U.UserName = @NombreUser AND 

UR.UserId = U.Id AND R.Id=UR.RoleId
END