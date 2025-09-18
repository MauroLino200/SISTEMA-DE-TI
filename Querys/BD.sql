USE master;
GO


IF DB_ID('SISTEMA_DE_TI') IS NOT NULL
BEGIN
    ALTER DATABASE SISTEMA_DE_TI SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SISTEMA_DE_TI;
END
GO

create database SISTEMA_DE_TI

use SISTEMA_DE_TI;
go


-- configuramos el formato de fecha
SET DATEFORMAT DMY;
GO


-- Tabla de Cargos 
CREATE TABLE TblCargo (
    IdCargo INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(50) NOT NULL
);
GO


-- insercion 
INSERT INTO TblCargo (Titulo) VALUES
('Asistente de Reclutamiento y Selección'),
('Asistente de Clima y Cultura'),
('Asistente de Capacitación'),
('Desarrollador Laravel Junior (Backend)'),
('Desarrollador Laravel Junior (Frontend)'),
('Desarrollador Rápido de Aplicaciones'),
('Analista de Documentación'),
('Administrador de Base de Datos'),
('Diseñador Web'),
('Asistente de Marketing Digital'),
('Diseñador Gráfico'),
('Practicante Comercial'),
('Contabilidad');
GO



-- Tabla de Departamentos
CREATE TABLE TblDepartamento (
    IdDepartamento INT IDENTITY(1,1) PRIMARY KEY,
    NombreDepartamento VARCHAR(100) NOT NULL,
    Ubicacion VARCHAR(100)
);
GO


-- insercion 
INSERT INTO TblDepartamento (NombreDepartamento, Ubicacion)
VALUES 
('Tecnología', 'Sede Central'),
('Recursos Humanos', 'Sede Central'),
('Contabilidad', 'Edificio B'),
('Marketing', 'Edificio A'),
('Comercial', 'Edificio C');
GO

CREATE TABLE TblTipoDocumento (
    IdTipoDocumento  INT IDENTITY(1,1) PRIMARY KEY,
	TipoDocumento VARCHAR (20) not null unique
);
GO


INSERT INTO TblTipoDocumento (TipoDocumento)
VALUES ('DNI'), ('Pasaporte'), ('Carnet Extranjería');


-- Tabla de Empleados
CREATE TABLE TblEmpleado (
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    IdTipoDocumento int,
	NumeroDocumento VARCHAR(50) NOT NULL UNIQUE,
    NombreCompleto VARCHAR(100) NOT NULL,
    FechaIngreso DATE NOT NULL,
    Turno CHAR(1)  NOT NULL CHECK(Turno IN ('M','T','N')), -- M=mañana, T=tarde, N=noche
    Correo VARCHAR(100) NOT NULL UNIQUE,
    IdCargo INT REFERENCES TblCargo,
	IdDepartamento INT REFERENCES TblDepartamento,
	IdTipoDocumento INT REFERENCES TblTipoDocumento	
);
GO


select * from dbo.TblEmpleado


-- Supongamos que tienes estos registros en TblTipoDocumento:
-- 1 = DNI
-- 2 = Pasaporte
-- 3 = Carnet de Extranjería

INSERT INTO TblEmpleado 
(IdTipoDocumento, NumeroDocumento, NombreCompleto, FechaIngreso, Turno, Correo, IdCargo, IdDepartamento)
VALUES
(1, '12345678', 'Andrea Torres Vega', '2022-03-01', 'M', 'andrea.torres@empresa.com', 1, 2),
(1, '23456789', 'Luis Méndez Paredes', '2021-12-10', 'T', 'luis.mendez@empresa.com', 2, 4),
(1, '34567890', 'María García Salas', '2022-07-15', 'M', 'maria.garcia@empresa.com', 3, 2),
(1, '45678901', 'Carlos Ruiz Campos', '2023-01-20', 'N', 'carlos.ruiz@empresa.com', 4, 1),
(1, '56789012', 'Elena López Díaz', '2022-10-05', 'M', 'elena.lopez@empresa.com', 1, 2),
(1, '67890123', 'Jorge Fernández Soto', '2022-09-01', 'T', 'jorge.fernandez@empresa.com', 5, 1),
(1, '78901234', 'Valeria Díaz Moreno', '2023-02-14', 'M', 'valeria.diaz@empresa.com', 2, 4),
(1, '89012345', 'Renzo Carrillo Núñez', '2022-05-12', 'N', 'renzo.carrillo@empresa.com', 3, 2),
(1, '90123456', 'Lucía Herrera León', '2023-04-18', 'T', 'lucia.herrera@empresa.com', 4, 1),
(1, '01234567', 'Diego Silva Romero', '2022-08-25', 'M', 'diego.silva@empresa.com', 1, 2);
GO



-- equipos  --


-- Tabla de modelos generales
CREATE TABLE TblEquipoModelo (
    IdModelo INT IDENTITY(1,1) PRIMARY KEY,
    NombreEquipo VARCHAR(100) NOT NULL,
    TipoEquipo VARCHAR(50) NOT NULL,
    Marca VARCHAR(100),
    Garantia VARCHAR(15)
);


-- insercion 
INSERT INTO TblEquipoModelo (NombreEquipo, TipoEquipo, Marca, Garantia) VALUES
('Laptop HP ProBook 440', 'Laptop', 'HP', '12 meses'),
('Monitor LG UltraFine', 'Monitor', 'LG', '24 meses'),
('Mouse Logitech M330', 'Periférico', 'Logitech', '6 meses'),
('Teclado Mecánico Redragon', 'Periférico', 'Redragon', '12 meses'),
('Smartphone Samsung A34', 'Celular', 'Samsung', '12 meses');


-- Tabla de equipos físicos (unidad por unidad)
CREATE TABLE TblEquipoTrabajo (
    IdEquipo INT IDENTITY(1,1) PRIMARY KEY,
    IdModelo INT REFERENCES TblEquipoModelo,
    NombreEquipo VARCHAR(100) NOT NULL,
    TipoEquipo VARCHAR(50) NOT NULL,
    Marca VARCHAR(100),
    FechaAsignacion DATE NOT NULL,
    Estado CHAR(1) CHECK (Estado IN ('N', 'S', 'A')) -- N = Nuevo, S = Segunda mano, A = Alquilado/Reservado, R = Reparado
);


-- insercion
INSERT INTO TblEquipoTrabajo (IdModelo, NombreEquipo, TipoEquipo, Marca, FechaAsignacion, Estado) VALUES
(1, 'HP ProBook 440 - SN123', 'Laptop', 'HP', '2023-01-01', 'N'),
(1, 'HP ProBook 440 - SN124', 'Laptop', 'HP', '2023-01-10', 'S'),
(2, 'Monitor LG UltraFine - SN555', 'Monitor', 'LG', '2023-03-15', 'N'),
(3, 'Mouse Logitech M330 - SN321', 'Periférico', 'Logitech', '2023-02-20', 'S'),
(4, 'Teclado Redragon K552 - SN982', 'Periférico', 'Redragon', '2023-04-12', 'A'),
(5, 'Samsung A34 - SN777', 'Celular', 'Samsung', '2023-05-05', 'N');


-- stock equipos --
CREATE TABLE TblStockEquipoModelo (
    IdModelo INT REFERENCES TblEquipoModelo,
    Estado CHAR(1) CHECK (Estado IN ('N', 'S', 'A')), -- Nuevo, Segunda mano, Alquilado, Reparado
    Cantidad INT NOT NULL,
    PRIMARY KEY (IdModelo, Estado)
);


-- insercion
INSERT INTO TblStockEquipoModelo (IdModelo, Estado, Cantidad) VALUES
(1, 'N', 3),  -- HP ProBook nuevas
(1, 'S', 1),  -- HP ProBook usadas
(2, 'N', 2),  -- Monitores nuevos
(3, 'S', 1),  -- Mouse usado
(4, 'A', 1),  -- Teclado alquilado
(5, 'N', 5);  -- Celulares nuevos


-- equipos  servicios --
CREATE TABLE TblReservasEquipos (
    IdReserva INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT REFERENCES TblEmpleado,
    IdEquipo INT REFERENCES TblEquipoTrabajo,
    FechaReserva DATETIME DEFAULT GETDATE()
);


CREATE TABLE TblDevolucionesEquipos (
    IdDevolucion INT IDENTITY(1,1) PRIMARY KEY,
    IdReserva INT REFERENCES TblReservasEquipos(IdReserva), -- Relación directa a la reserva
    FechaDevolucion DATETIME DEFAULT GETDATE()
);



-- Tabla de Contratos
CREATE TABLE TblContrato (
    IdContrato INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT REFERENCES TblEmpleado,
    TipoContrato VARCHAR(50) CHECK (TipoContrato IN ('Indeterminado', 'Temporal', 'Practicante''Freelance','Por proyecto','Outsourcing',
        'Interino',
        'Honorarios',
        'Consultoría')),
    FechaInicio DATE NOT NULL,
    FechaFin DATE,
    SueldoBase DECIMAL(10,2) NOT NULL
);
GO


CREATE TABLE TblAsistencia (
    IdAsistencia INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT REFERENCES TblEmpleado,
    HoraEntrada DATETIME NOT NULL,  -- fecha y hora de entrada
    HoraSalida DATETIME NULL        -- fecha y hora de salida
);



CREATE TABLE TblEstadoPermiso (
    IdEstadoPermiso INT IDENTITY(1,1) PRIMARY KEY,
    Estado VARCHAR(50) UNIQUE NOT NULL
);
GO

INSERT INTO TblEstadoPermiso ( Estado) VALUES
('Pendiente'),
('Aprobado'),
('Rechazado');
GO



-- Tabla de Permisos o Ausencias
CREATE TABLE TblPermiso (
    IdPermiso INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT REFERENCES TblEmpleado,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Motivo VARCHAR(255),
    IdEstadoPermiso INT REFERENCES TblEstadoPermiso DEFAULT 1 -- 'Pendiente'
);
GO


create table TblCapacitaciones(
	IdCapacitacion INT IDENTITY(1,1) PRIMARY KEY,
	Nombre varchar(100)
);
go


-- Tabla de Evaluaciones
CREATE TABLE TblEvaluacion (
    IdEvaluacion INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT REFERENCES TblEmpleado,
	IdCapacitacion INT REFERENCES TblCapacitaciones,
    FechaEvaluacion DATE NOT NULL,
	FechaFinalizacion DATE NOT NULL,
	Estado bit,
    Calificacion INT CHECK (Calificacion BETWEEN 0 AND 20),
    Comentarios TEXT
);
GO


ALTER TABLE TblEvaluacion
ALTER COLUMN FechaFinalizacion DATETIME NOT NULL;



create table TblCapacitaciones(
	IdCapacitacion INT IDENTITY(1,1) PRIMARY KEY,
	Nombre varchar(100)
);
go

insert into TblCapacitaciones (Nombre) values
('Algoritmos y Estructura de Datos'),
('Principios Básicos en Python'),
('La teoria del Marketing'),
('Curso de Excel Financiero'),
('Oratoria: "Cómo llegar a lo alto"'),
('Fundamentos de Redes'),
('Administración de Servidores Linux'),
('Desarrollo Web con JavaScript y Vue.js'),
('Patrones de Diseño de Software'),
('Seguridad Informática y Ciberseguridad'),
('Taller de Scrum y Metodologías Ágiles'),
('Análisis y Diseño de Sistemas'),
('Gestión de Proyectos con Microsoft Project'),
('Curso Intensivo de SQL Server'),
('Optimización de Consultas SQL'),
('Desarrollo de APIs REST con Laravel'),
('Testing y QA para Aplicaciones Web'),
('Power BI: Análisis y Visualización de Datos'),
('Machine Learning con Python y Scikit-learn'),
('Administración de Base de Datos Oracle'),
('Curso de Docker y Contenedores'),
('Git y GitHub: Control de Versiones Profesional'),
('Inglés Técnico para Profesionales TI'),
('Automatización de Procesos con Python'),
('Técnicas de Comunicación Asertiva en Equipos');


CREATE TABLE TblUsuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario VARCHAR(50) UNIQUE NOT NULL,
    Contrasena VARBINARY(64) NOT NULL, -- Para SHA-256
    Rol VARCHAR(50) CHECK (Rol IN ('admin', 'rrhh', 'accountant')),
    IdEmpleado INT REFERENCES TblEmpleado
);
GO



