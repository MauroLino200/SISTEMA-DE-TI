use SISTEMA_DE_TI;
go


-- procedure y triggers

-- Equipos ---
CREATE OR ALTER PROC USP_GET_ALL_EQUIPOS
AS
BEGIN
		
		SELECT
		IdEquipo        = EQT.IdEquipo,
        NombreEquipo    = EQT.NombreEquipo,
        IdModelo        = EQM.IdModelo, 
        TipoEquipo      = EQM.TipoEquipo,
        Marca           = EQM.Marca,
        Cantidad        = SEQM.Cantidad,
        FechaAsignacion = EQT.FechaAsignacion,
		ESTADO =	 CASE EQT.Estado 
					 WHEN 'N' THEN 'Nuevo'
								WHEN 'S' THEN 'Segunda Mano'
								WHEN 'A' THEN 'Alquilado'
								END
		FROM dbo.TblEquipoTrabajo EQT
		inner join TblEquipoModelo EQM ON EQT.IdModelo = EQM.IdModelo
		inner join TblStockEquipoModelo SEQM ON EQM.IdModelo = SEQM.IdModelo and EQT.Estado = SEQM.Estado 
END
GO

exec dbo.USP_GET_ALL_EQUIPOS


CREATE OR ALTER PROCEDURE USP_INSERT_EQUIPOTRABAJO
    @IdModelo INT,
    @NombreEquipo VARCHAR(100),
    @TipoEquipo VARCHAR(50),
    @Marca VARCHAR(100),
    @FechaAsignacion DATE,
    @Estado CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validación 1: Que el IdModelo exista
    IF NOT EXISTS (SELECT 1 FROM TblEquipoModelo WHERE IdModelo = @IdModelo)
    BEGIN
        RAISERROR('El modelo especificado no existe.', 16, 1);
        RETURN;
    END

    -- Validación 2: Que el Estado sea válido
    IF @Estado NOT IN ('N', 'S', 'A')
    BEGIN
        RAISERROR('Estado no válido. Debe ser N, S o A.', 16, 1);
        RETURN;
    END

    -- Inserción si todo es válido
    INSERT INTO TblEquipoTrabajo (IdModelo, NombreEquipo, TipoEquipo, Marca, FechaAsignacion, Estado)
    VALUES (@IdModelo, @NombreEquipo, @TipoEquipo, @Marca, @FechaAsignacion, @Estado);
END
GO


-- 


CREATE OR ALTER PROCEDURE USP_UPDATE_EQUIPOTRABAJO
    @IdEquipo INT,
    @IdModelo INT,
    @NombreEquipo VARCHAR(100),
    @TipoEquipo VARCHAR(50),
    @Marca VARCHAR(100),
    @FechaAsignacion DATE,
    @Estado CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar existencia del equipo
    IF NOT EXISTS (SELECT 1 FROM TblEquipoTrabajo WHERE IdEquipo = @IdEquipo)
    BEGIN
        RAISERROR('El equipo especificado no existe.', 16, 1);
        RETURN;
    END

    -- Validar existencia del modelo
    IF NOT EXISTS (SELECT 1 FROM TblEquipoModelo WHERE IdModelo = @IdModelo)
    BEGIN
        RAISERROR('El modelo especificado no existe.', 16, 1);
        RETURN;
    END

    -- Validar estado permitido
    IF @Estado NOT IN ('N', 'S', 'A')
    BEGIN
        RAISERROR('Estado no válido. Debe ser N, S o A.', 16, 1);
        RETURN;
    END

    -- Actualizar los datos
    UPDATE TblEquipoTrabajo
    SET
        IdModelo = @IdModelo,
        NombreEquipo = @NombreEquipo,
        TipoEquipo = @TipoEquipo,
        Marca = @Marca,
        FechaAsignacion = @FechaAsignacion,
        Estado = @Estado
    WHERE IdEquipo = @IdEquipo;
END
GO


---

select * from dbo.TblEquipoModelo

CREATE OR ALTER PROCEDURE USP_ELIMINAR_EQUIPO
    @IdEquipo INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM TblEquipoTrabajo
    WHERE IdEquipo = @IdEquipo;
END
GO


CREATE OR ALTER TRIGGER TRG_ACTUALIZAR_STOCK_EQUIPO
ON TblEquipoTrabajo
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- 🔻 Disminuir stock por los registros eliminados o actualizados (estado anterior)
    IF EXISTS (SELECT * FROM DELETED)
    BEGIN
        UPDATE S
        SET S.Cantidad = S.Cantidad - D.CantidadEliminada
        FROM TblStockEquipoModelo S
        INNER JOIN (
            SELECT IdModelo, Estado, COUNT(*) AS CantidadEliminada
            FROM DELETED
            GROUP BY IdModelo, Estado
        ) D ON S.IdModelo = D.IdModelo AND S.Estado = D.Estado;
    END

    -- 🔺 Aumentar stock por los registros insertados o actualizados (estado nuevo)
    IF EXISTS (SELECT * FROM INSERTED)
    BEGIN
        MERGE TblStockEquipoModelo AS target
        USING (
            SELECT IdModelo, Estado, COUNT(*) AS Cantidad
            FROM INSERTED
            GROUP BY IdModelo, Estado
        ) AS src
        ON target.IdModelo = src.IdModelo AND target.Estado = src.Estado
        WHEN MATCHED THEN
            UPDATE SET Cantidad = target.Cantidad + src.Cantidad
        WHEN NOT MATCHED THEN
            INSERT (IdModelo, Estado, Cantidad)
            VALUES (src.IdModelo, src.Estado, src.Cantidad);
    END
END
GO



-- equipos reserva --

CREATE OR ALTER PROC USP_GET_ALL_RESERVAS
AS
BEGIN
		
		SELECT
		IdReserva   = RE.IdReserva,
        IdEquipo    = EQT.IdEquipo,
		NombreEquipo = EQT.NombreEquipo,
        IdModelo     = EQT.IdModelo, 
        TipoEquipo   = EQT.TipoEquipo,
        Marca        = EQT.Marca,
		IdEmpleado = EMP.IdEmpleado,
		NombreEmpleado = EMP.NombreCompleto,
		IdCargo = EMP.IdCargo,
		IdDepartamento = EMP.IdDepartamento,
        FechaReserva = RE.FechaReserva
		FROM dbo.TblReservasEquipos RE
		inner join TblEquipoTrabajo EQT ON RE.IdEquipo = EQT.IdEquipo  
		inner join TblEmpleado EMP ON RE.IdEmpleado = EMP.IdEmpleado 
END
GO

exec dbo.USP_GET_ALL_RESERVAS


-- Filtrado Reservas 

CREATE OR ALTER PROC USP_GET_ONE_RESERVA_BY_ITS_ID
 @IdReserva int

AS
BEGIN
		
		SELECT
		IdReserva   = RE.IdReserva,
        IdEquipo    = EQT.IdEquipo,
		NombreEquipo = EQT.NombreEquipo,
        IdModelo     = EQT.IdModelo, 
        TipoEquipo   = EQT.TipoEquipo,
        Marca        = EQT.Marca,
		IdEmpleado = EMP.IdEmpleado,
		NombreEmpleado = EMP.NombreCompleto,
		IdCargo = EMP.IdCargo,
		IdDepartamento = EMP.IdDepartamento,
        FechaReserva = RE.FechaReserva
		FROM dbo.TblReservasEquipos RE
		inner join TblEquipoTrabajo EQT ON RE.IdEquipo = EQT.IdEquipo  
		inner join TblEmpleado EMP ON RE.IdEmpleado = EMP.IdEmpleado 
		where RE.IdReserva = @IdReserva
END
GO

exec dbo.USP_GET_ONE_RESERVA_BY_ITS_ID 2


CREATE OR ALTER PROC USP_GET_ONE_RESERVA_BY_ITS_NAME
 @NombreEquipo VARCHAR(100)

AS
BEGIN
		
		SELECT
		IdReserva   = RE.IdReserva,
        IdEquipo    = EQT.IdEquipo,
		NombreEquipo = EQT.NombreEquipo,
        IdModelo     = EQT.IdModelo, 
        TipoEquipo   = EQT.TipoEquipo,
        Marca        = EQT.Marca,
		IdEmpleado = EMP.IdEmpleado,
		NombreEmpleado = EMP.NombreCompleto,
		IdCargo = EMP.IdCargo,
		IdDepartamento = EMP.IdDepartamento,
        FechaReserva = RE.FechaReserva
		FROM dbo.TblReservasEquipos RE
		inner join TblEquipoTrabajo EQT ON RE.IdEquipo = EQT.IdEquipo  
		inner join TblEmpleado EMP ON RE.IdEmpleado = EMP.IdEmpleado 
		where EQT.NombreEquipo = @NombreEquipo
END
GO

exec dbo.USP_GET_ONE_RESERVA_BY_ITS_NAME ''  


CREATE OR ALTER PROC USP_GET_ONE_RESERVA_BY_ITS_TYPE_OF_THING
 @IdModelo int

AS
BEGIN
		
		SELECT
		IdReserva   = RE.IdReserva,
        IdEquipo    = EQT.IdEquipo,
		NombreEquipo = EQT.NombreEquipo,
        IdModelo     = EQT.IdModelo, 
        TipoEquipo   = EQT.TipoEquipo,
        Marca        = EQT.Marca,
		IdEmpleado = EMP.IdEmpleado,
		NombreEmpleado = EMP.NombreCompleto,
		IdCargo = EMP.IdCargo,
		IdDepartamento = EMP.IdDepartamento,
        FechaReserva = RE.FechaReserva
		FROM dbo.TblReservasEquipos RE
		inner join TblEquipoTrabajo EQT ON RE.IdEquipo = EQT.IdEquipo  
		inner join TblEmpleado EMP ON RE.IdEmpleado = EMP.IdEmpleado 
		where EQT.IdModelo = @IdModelo
END
GO

exec dbo.USP_GET_ONE_RESERVA_BY_ITS_ID 2


CREATE OR ALTER PROC USP_GET_ONE_RESERVA_BY_ID_OF_EMPLOYEE
 @IdEmpleado int

AS
BEGIN
		
		SELECT
		IdReserva   = RE.IdReserva,
        IdEquipo    = EQT.IdEquipo,
		NombreEquipo = EQT.NombreEquipo,
        IdModelo     = EQT.IdModelo, 
        TipoEquipo   = EQT.TipoEquipo,
        Marca        = EQT.Marca,
		IdEmpleado = EMP.IdEmpleado,
		NombreEmpleado = EMP.NombreCompleto,
		IdCargo = EMP.IdCargo,
		IdDepartamento = EMP.IdDepartamento,
        FechaReserva = RE.FechaReserva
		FROM dbo.TblReservasEquipos RE
		inner join TblEquipoTrabajo EQT ON RE.IdEquipo = EQT.IdEquipo  
		inner join TblEmpleado EMP ON RE.IdEmpleado = EMP.IdEmpleado 
		where EMP.IdEmpleado = @IdEmpleado
END
GO

exec dbo.USP_GET_ONE_RESERVA_BY_ID_OF_EMPLOYEE 1


CREATE OR ALTER PROC USP_GET_ONE_RESERVA_BY_ITS_DATE
 @FechaReserva DATETIME

AS
BEGIN
		
		SELECT
		IdReserva   = RE.IdReserva,
        IdEquipo    = EQT.IdEquipo,
		NombreEquipo = EQT.NombreEquipo,
        IdModelo     = EQT.IdModelo, 
        TipoEquipo   = EQT.TipoEquipo,
        Marca        = EQT.Marca,
		IdEmpleado = EMP.IdEmpleado,
		NombreEmpleado = EMP.NombreCompleto,
		IdCargo = EMP.IdCargo,
		IdDepartamento = EMP.IdDepartamento,
        FechaReserva = RE.FechaReserva
		FROM dbo.TblReservasEquipos RE
		inner join TblEquipoTrabajo EQT ON RE.IdEquipo = EQT.IdEquipo  
		inner join TblEmpleado EMP ON RE.IdEmpleado = EMP.IdEmpleado 
		where RE.FechaReserva = @FechaReserva
END
GO

select * from dbo.TblReservasEquipos


exec USP_GET_ONE_RESERVA_BY_ITS_DATE '2025-08-18 21:01:36.413'



CREATE OR ALTER PROCEDURE USP_InsertReservaEquipo
    @IdEmpleado INT,
    @IdEquipo INT
AS
BEGIN
    DECLARE @Estado CHAR(1);

    -- Verificar el estado actual del equipo
    SELECT @Estado = Estado
    FROM TblEquipoTrabajo
    WHERE IdEquipo = @IdEquipo;

    -- Validar si existe
    IF @Estado IS NULL
    BEGIN
        PRINT 'Error: El equipo no existe o no tiene estado definido.';
        RETURN;
    END

    -- Validar si el equipo está disponible
    IF @Estado IN ('N','S') -- disponibles
    BEGIN
        -- Registrar la reserva
        INSERT INTO TblReservasEquipos (IdEmpleado, IdEquipo)
        VALUES (@IdEmpleado, @IdEquipo);

        -- Cambiar estado del equipo a Alquilado
        UPDATE TblEquipoTrabajo
        SET Estado = 'A'
        WHERE IdEquipo = @IdEquipo;

        PRINT 'Reserva realizada correctamente y equipo marcado como Alquilado.';
    END
    ELSE
    BEGIN
        PRINT 'Error: El equipo no está disponible. Estado actual: ' + @Estado;
    END
END
GO



CREATE OR ALTER PROCEDURE USP_ActualizarReservaEquipo
    @IdReserva INT,
    @NuevoIdEmpleado INT,
    @NuevoIdEquipo INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar existencia de la reserva
    IF NOT EXISTS (SELECT 1 FROM TblReservasEquipos WHERE IdReserva = @IdReserva)
    BEGIN
        RAISERROR('La reserva especificada no existe.', 16, 1);
        RETURN;
    END

    -- Validar existencia del nuevo equipo
    IF NOT EXISTS (SELECT 1 FROM TblEquipoTrabajo WHERE IdEquipo = @NuevoIdEquipo)
    BEGIN
        RAISERROR('El nuevo equipo especificado no existe.', 16, 1);
        RETURN;
    END

    -- Validar existencia del nuevo empleado
    IF NOT EXISTS (SELECT 1 FROM TblEmpleado WHERE IdEmpleado = @NuevoIdEmpleado)
    BEGIN
        RAISERROR('El empleado especificado no existe.', 16, 1);
        RETURN;
    END

    UPDATE TblReservasEquipos
    SET IdEmpleado = @NuevoIdEmpleado,
        IdEquipo = @NuevoIdEquipo
    WHERE IdReserva = @IdReserva;

    PRINT 'Reserva actualizada correctamente.';
END
GO



CREATE OR ALTER PROCEDURE USP_EliminarReservaEquipo
    @IdReserva INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar existencia de la reserva
    IF NOT EXISTS (SELECT 1 FROM TblReservasEquipos WHERE IdReserva = @IdReserva)
    BEGIN
        RAISERROR('La reserva especificada no existe.', 16, 1);
        RETURN;
    END

    DELETE FROM TblReservasEquipos
    WHERE IdReserva = @IdReserva;

    PRINT 'Reserva eliminada correctamente.';
END
GO




-- equipos devolucion

CREATE OR ALTER PROC USP_GET_ALL_DEVOLUCIONES
AS
BEGIN
    SELECT
        DE.IdDevolucion,
        EQT.IdEquipo,
        EQT.NombreEquipo,
        EQT.IdModelo,
        EQT.TipoEquipo,
        EQT.Marca,
        RE.IdEmpleado,
        DE.FechaDevolucion
    FROM dbo.TblDevolucionesEquipos DE
    INNER JOIN TblReservasEquipos RE 
        ON DE.IdReserva = RE.IdReserva
    INNER JOIN TblEquipoTrabajo EQT 
        ON RE.IdEquipo = EQT.IdEquipo;
END
GO


exec dbo.USP_GET_ALL_DEVOLUCIONES


-- filtrados devoluciones

CREATE OR ALTER PROC USP_GET_ONE_DEVOLUCION_BY_ITS_ID
@IdDevolucion int
AS
BEGIN
    SELECT
         DE.IdDevolucion,
       EQT.IdEquipo,
       EQT.NombreEquipo,
    EQT.IdModelo,
        EQT.TipoEquipo,
      EQT.Marca,
		RE.IdEmpleado,
       DE.FechaDevolucion
    FROM dbo.TblDevolucionesEquipos DE
    INNER JOIN TblReservasEquipos RE 
        ON DE.IdReserva = RE.IdReserva
    INNER JOIN TblEquipoTrabajo EQT 
        ON RE.IdEquipo = EQT.IdEquipo
		WHERE DE.IdDevolucion = @IdDevolucion
END
GO

exec dbo.USP_GET_ONE_DEVOLUCION_BY_ITS_ID 1


CREATE OR ALTER PROC USP_GET_ONE_DEVOLUCION_BY_ITS_NAME
@NombreEquipo VARCHAR(100)
AS
BEGIN
    SELECT
         DE.IdDevolucion,
       EQT.IdEquipo,
       EQT.NombreEquipo,
    EQT.IdModelo,
        EQT.TipoEquipo,
      EQT.Marca,
		RE.IdEmpleado,
       DE.FechaDevolucion
    FROM dbo.TblDevolucionesEquipos DE
    INNER JOIN TblReservasEquipos RE 
        ON DE.IdReserva = RE.IdReserva
    INNER JOIN TblEquipoTrabajo EQT 
        ON RE.IdEquipo = EQT.IdEquipo
		WHERE EQT.NombreEquipo = @NombreEquipo
END
GO

exec dbo.USP_GET_ONE_DEVOLUCION_BY_ITS_NAME 'Mouse Logitech M330 - SN321'


CREATE OR ALTER PROC USP_GET_ONE_DEVOLUCION_BY_ITS_TYPE
@IdModelo int
AS
BEGIN
    SELECT
         DE.IdDevolucion,
       EQT.IdEquipo,
       EQT.NombreEquipo,
    EQT.IdModelo,
        EQT.TipoEquipo,
      EQT.Marca,
		RE.IdEmpleado,
       DE.FechaDevolucion
    FROM dbo.TblDevolucionesEquipos DE
    INNER JOIN TblReservasEquipos RE 
        ON DE.IdReserva = RE.IdReserva
    INNER JOIN TblEquipoTrabajo EQT 
        ON RE.IdEquipo = EQT.IdEquipo
		WHERE EQT.IdModelo = @IdModelo
END
GO

exec dbo.USP_GET_ONE_DEVOLUCION_BY_ITS_TYPE 1


CREATE OR ALTER PROC USP_GET_ONE_DEVOLUCION_BY_ID_EMPLOYEE
@IdEmpleado int
AS
BEGIN
    SELECT
         DE.IdDevolucion,
       EQT.IdEquipo,
       EQT.NombreEquipo,
    EQT.IdModelo,
        EQT.TipoEquipo,
      EQT.Marca,
		RE.IdEmpleado,
       DE.FechaDevolucion
    FROM dbo.TblDevolucionesEquipos DE
    INNER JOIN TblReservasEquipos RE 
        ON DE.IdReserva = RE.IdReserva
    INNER JOIN TblEquipoTrabajo EQT 
        ON RE.IdEquipo = EQT.IdEquipo
		WHERE RE.IdEmpleado = @IdEmpleado
END
GO

exec dbo.USP_GET_ONE_DEVOLUCION_BY_ID_EMPLOYEE 1


CREATE OR ALTER PROC USP_GET_ONE_DEVOLUCION_BY_DATE
@FechaDevolucion DATETIME
AS
BEGIN
    SELECT
         DE.IdDevolucion,
       EQT.IdEquipo,
       EQT.NombreEquipo,
    EQT.IdModelo,
        EQT.TipoEquipo,
      EQT.Marca,
		RE.IdEmpleado,
       DE.FechaDevolucion
    FROM dbo.TblDevolucionesEquipos DE
    INNER JOIN TblReservasEquipos RE 
        ON DE.IdReserva = RE.IdReserva
    INNER JOIN TblEquipoTrabajo EQT 
        ON RE.IdEquipo = EQT.IdEquipo
		WHERE DE.FechaDevolucion = @FechaDevolucion
END
GO

exec dbo.USP_GET_ONE_DEVOLUCION_BY_DATE '2025-08-21 15:29:21.607'




CREATE OR ALTER PROCEDURE USP_InsertDevolucionEquipo
    @IdReserva INT
AS
BEGIN
    DECLARE @IdEquipo INT;
    DECLARE @Estado CHAR(1);
    DECLARE @NewId INT;

    -- 1. Buscar los datos de la reserva
    SELECT @IdEquipo = R.IdEquipo
    FROM TblReservasEquipos R
    WHERE R.IdReserva = @IdReserva;

    -- Validar si la reserva existe
    IF @IdEquipo IS NULL
    BEGIN
        RAISERROR('La reserva no existe.', 16, 1);
        RETURN;
    END

    -- 2. Obtener el estado actual del equipo
    SELECT @Estado = Estado
    FROM TblEquipoTrabajo
    WHERE IdEquipo = @IdEquipo;

    -- 3. Validar si el equipo está alquilado (A = Alquilado)
    IF @Estado = 'A'
    BEGIN
        -- 4. Insertar la devolución y capturar ID
        INSERT INTO TblDevolucionesEquipos(IdReserva)
        VALUES (@IdReserva);

        SET @NewId = SCOPE_IDENTITY();

        -- 5. Actualizar el estado del equipo
        UPDATE TblEquipoTrabajo
        SET Estado = 'S'
        WHERE IdEquipo = @IdEquipo;

        -- Retornar el id de la devolución
        SELECT @NewId;
    END
    ELSE
    BEGIN
        RAISERROR('El equipo no está actualmente alquilado, no se puede devolver.', 16, 1);
    END
END
GO


-- equipos filtrado ---
CREATE OR ALTER PROCEDURE USP_GET_ONE_EQUIPO_BY_ID
@IdEquipo int
AS
BEGIN
		
		SELECT
		EQT.IdEquipo,
		EQT.NombreEquipo,
		EQM.IdModelo,
		EQM.TipoEquipo,
		EQM.Marca,
		EQT.Estado,
		SEQM.Cantidad,
		EQT.FechaAsignacion

		FROM dbo.TblEquipoTrabajo EQT
		inner join TblEquipoModelo EQM ON EQT.IdModelo = EQM.IdModelo
		inner join TblStockEquipoModelo SEQM ON EQM.IdModelo = SEQM.IdModelo and EQT.Estado = SEQM.Estado
		where EQT.IdEquipo = @IdEquipo
END
GO

exec USP_GET_ONE_EQUIPO_BY_ID 1


CREATE OR ALTER PROCEDURE USP_GET_EQUIPOS_BY_NAME
@NombreEquipo VARCHAR(100)
AS
BEGIN
		
		SELECT
		EQT.IdEquipo,
		EQT.NombreEquipo,
		EQM.IdModelo,
		EQM.TipoEquipo,
		EQM.Marca,
		EQT.Estado,
		SEQM.Cantidad,
		EQT.FechaAsignacion

		FROM dbo.TblEquipoTrabajo EQT
		inner join TblEquipoModelo EQM ON EQT.IdModelo = EQM.IdModelo
		inner join TblStockEquipoModelo SEQM ON EQM.IdModelo = SEQM.IdModelo and EQT.Estado = SEQM.Estado
		where EQT.NombreEquipo = @NombreEquipo
END
GO

EXEC dbo.USP_GET_EQUIPOS_BY_NAME 'HP ProBook 440 - SN123'

CREATE OR ALTER PROCEDURE USP_GET_EQUIPOS_BY_TYPE
@TipoEquipo VARCHAR(50)AS
BEGIN
		
		SELECT
		EQT.IdEquipo,
		EQT.NombreEquipo,
		EQM.IdModelo,
		EQM.TipoEquipo,
		EQM.Marca,
		EQT.Estado,
		SEQM.Cantidad,
		EQT.FechaAsignacion

		FROM dbo.TblEquipoTrabajo EQT
		inner join TblEquipoModelo EQM ON EQT.IdModelo = EQM.IdModelo
		inner join TblStockEquipoModelo SEQM ON EQM.IdModelo = SEQM.IdModelo and EQT.Estado = SEQM.Estado
		where EQT.TipoEquipo = @TipoEquipo
END
GO

EXEC dbo.USP_GET_EQUIPOS_BY_TYPE 'Laptop'


CREATE OR ALTER PROCEDURE USP_GET_EQUIPOS_BY_STATEMENT
	@Estado CHAR(1)
AS
BEGIN
		
		SELECT
		EQT.IdEquipo,
		EQT.NombreEquipo,
		EQM.IdModelo,
		EQM.TipoEquipo,
		EQM.Marca,
		EQT.Estado,
		SEQM.Cantidad,
		EQT.FechaAsignacion

		FROM dbo.TblEquipoTrabajo EQT
		inner join TblEquipoModelo EQM ON EQT.IdModelo = EQM.IdModelo
		inner join TblStockEquipoModelo SEQM ON EQM.IdModelo = SEQM.IdModelo and EQT.Estado = SEQM.Estado
		where EQT.Estado = @Estado
END
GO

EXEC dbo.USP_GET_EQUIPOS_BY_STATEMENT 'N'


CREATE OR ALTER PROCEDURE USP_GET_EQUIPOS_BY_DATE
    @FechaAsignacion DATETIME 
AS
BEGIN
		
		SELECT
		EQT.IdEquipo,
		EQT.NombreEquipo,
		EQM.IdModelo,
		EQM.TipoEquipo,
		EQM.Marca,
		EQT.Estado,
		SEQM.Cantidad,
		EQT.FechaAsignacion

		FROM dbo.TblEquipoTrabajo EQT
		inner join TblEquipoModelo EQM ON EQT.IdModelo = EQM.IdModelo
		inner join TblStockEquipoModelo SEQM ON EQM.IdModelo = SEQM.IdModelo and EQT.Estado = SEQM.Estado
		where EQT.FechaAsignacion = @FechaAsignacion
END
GO


EXEC dbo.USP_GET_EQUIPOS_BY_DATE '2023-05-05'


-- Empleados

CREATE OR ALTER PROC USP_SELECT_EMPLEADOS
AS
BEGIN
    SELECT 
        E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
        E.Turno,
        E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
        INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
    ORDER BY E.NombreCompleto
END
-- EXEC USP_SELECT_EMPLEADOS

GO


-- Filtrados y busqueda de Empleado 

CREATE OR ALTER PROC USP_GET_ONE_EMPLEADO_BY_ID
	@IdEmpleado INT
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
        E.Turno,
        E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.IdEmpleado = @IdEmpleado
END
-- EXEC USP_GET_ONE_EMPLEADO_BY_ID 3
GO


CREATE OR ALTER PROC USP_GET_ONE_EMPLEADO_BY_ITS_NUMBER_OF_DOCUMENT
 @NumeroDocumento VARCHAR(50)
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,        
		E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.NumeroDocumento = @NumeroDocumento
END
-- EXEC USP_GET_ONE_EMPLEADO_BY_ITS_NUMBER_OF_DOCUMENT 12345678
GO

CREATE OR ALTER PROC USP_GET_ONE_EMPLEADO_BY_NAME
 @NombreCompleto VARCHAR(100)
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,        
		E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.NombreCompleto = @NombreCompleto
END
-- EXEC USP_GET_ONE_EMPLEADO_BY_NAME 'Jorge'
GO


CREATE OR ALTER PROC USP_GET_ONE_EMPLEADO_BY_DATE
 @FechaIngreso DATE
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,        
		E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.FechaIngreso = @FechaIngreso
END
-- EXEC USP_GET_ONE_EMPLEADO_BY_DATE '2022-03-01'
GO


CREATE OR ALTER PROC USP_GET_EMPLEADOS_BY_TURNO
	@Turno CHAR(1)
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,
        E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.Turno = @Turno
END
-- EXEC USP_GET_EMPLEADOS_BY_TURNO 't'
GO


CREATE OR ALTER PROC USP_GET_EMPLEADOS_BY_EMAIL
	@Correo VARCHAR(100)
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,
        E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.Correo = @Correo
END
-- EXEC USP_GET_EMPLEADOS_BY_EMAIL 'andrea.torres@empresa.com'
GO


CREATE OR ALTER PROC USP_GET_EMPLEADOS_BY_CARGO
	@IdCargo int
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,
        E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.IdCargo = @IdCargo
END
-- EXEC USP_GET_EMPLEADOS_BY_CARGO 
GO


CREATE OR ALTER PROC USP_GET_EMPLEADOS_BY_OFFICE
	@IdDepartamento int
AS
BEGIN
	SELECT 
		E.IdEmpleado,
        E.IdTipoDocumento,
		E.NumeroDocumento,
        E.NombreCompleto,
        E.FechaIngreso,
		Turno = CASE E.Turno
		WHEN 'M' THEN 'Mañana'
		WHEN 'T' THEN 'Tarde'
		WHEN 'N' THEN 'Noche'
		END,
        E.Correo,
        E.IdCargo,
        C.Titulo AS Cargo,
        E.IdDepartamento,
        D.NombreDepartamento AS Departamento
    FROM TblEmpleado E
		INNER JOIN TblCargo C ON E.IdCargo = C.IdCargo
        INNER JOIN TblDepartamento D ON E.IdDepartamento = D.IdDepartamento
	WHERE E.IdDepartamento = @IdDepartamento
END
-- EXEC USP_GET_EMPLEADOS_BY_OFFICE 
GO


-- USP_INSERT_EMPLEADO (actualizado)
CREATE OR ALTER PROC USP_INSERT_EMPLEADO
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20),
    @NombreCompleto VARCHAR(100),
    @FechaIngreso DATE,
    @Turno CHAR(1),
    @Correo VARCHAR(100),
    @IdCargo INT,
    @IdDepartamento INT
AS
BEGIN
    INSERT INTO TblEmpleado 
    (IdTipoDocumento, NumeroDocumento, NombreCompleto, FechaIngreso, Turno, Correo, IdCargo, IdDepartamento)
    VALUES 
    (@IdTipoDocumento, @NumeroDocumento, @NombreCompleto, @FechaIngreso, @Turno, @Correo, @IdCargo, @IdDepartamento)
END
GO

-- Ejemplo de ejecución
-- EXEC USP_INSERT_EMPLEADO 1, '87654321', 'Juan Mendoza Vera', '2023-01-01', 'M', 'juan.mendoza@empresa.com', 4, 1;


-- USP_UPDATE_EMPLEADO (actualizado)
CREATE OR ALTER PROC USP_UPDATE_EMPLEADO
    @IdEmpleado INT,
    @IdTipoDocumento INT,
    @NumeroDocumento VARCHAR(20),
    @NombreCompleto VARCHAR(100),
    @FechaIngreso DATE,
    @Turno CHAR(1),
    @Correo VARCHAR(100),
    @IdCargo INT,
    @IdDepartamento INT
AS
BEGIN
    UPDATE TblEmpleado
    SET 
        IdTipoDocumento = @IdTipoDocumento,
        NumeroDocumento = @NumeroDocumento,
        NombreCompleto = @NombreCompleto,
        FechaIngreso = @FechaIngreso,
        Turno = @Turno,
        Correo = @Correo,
        IdCargo = @IdCargo,
        IdDepartamento = @IdDepartamento
    WHERE IdEmpleado = @IdEmpleado
END
GO

-- Ejemplo de ejecución
-- EXEC USP_UPDATE_EMPLEADO 3, 1, '34567890', 'María G. Salas', '2022-07-15', 'M', 'maria.actualizada@empresa.com', 3, 2;


-- USP_DELETE_EMPLEADO
CREATE OR ALTER PROC USP_DELETE_EMPLEADO
	@IdEmpleado INT
AS
BEGIN
	DELETE FROM TblEmpleado
	WHERE IdEmpleado = @IdEmpleado
END
-- EXEC USP_DELETE_EMPLEADO 10
GO


-- capacitaciones vista empleado 

CREATE OR ALTER PROC USP_GET_ALL_TESTS
AS
BEGIN
	SELECT 
		EV.IdEvaluacion,
		CAP.Nombre AS Curso_de_la_Evaluación,
		EMP.IdEmpleado,
        EMP.NombreCompleto,
		CAR.Titulo,
		DEP.NombreDepartamento,
		DEP.Ubicacion,
		EV.Calificacion,
		EV.FechaEvaluacion,
		EV.FechaFinalizacion,
		EV.Comentarios
    FROM TblEvaluacion EV
		INNER JOIN TblEmpleado EMP ON EV.IdEmpleado = EMP.IdEmpleado
        INNER JOIN TblCapacitaciones CAP ON EV.IdCapacitacion = CAP.IdCapacitacion
		INNER JOIN TblCargo CAR ON EMP.IdCargo = CAR.IdCargo
		INNER JOIN TblDepartamento DEP ON EMP.IdDepartamento = DEP.IdDepartamento
		ORDER BY EV.FechaFinalizacion
END
-- EXEC USP_GET_ALL_TESTS 
GO


CREATE OR ALTER PROC USP_GET_TESTS_BY_COURSE_NAME
	@Nombre varchar(100)
AS
BEGIN
	SELECT 
		EV.IdEvaluacion,
		CAP.Nombre AS Curso_de_la_Evaluación,
		EMP.IdEmpleado,
        EMP.NombreCompleto,
		CAR.Titulo,
		DEP.NombreDepartamento,
		DEP.Ubicacion,
		EV.Calificacion,
		EV.FechaEvaluacion,
		EV.FechaFinalizacion,
		EV.Comentarios
    FROM TblEvaluacion EV
		INNER JOIN TblEmpleado EMP ON EV.IdEmpleado = EMP.IdEmpleado
        INNER JOIN TblCapacitaciones CAP ON EV.IdCapacitacion = CAP.IdCapacitacion
		INNER JOIN TblCargo CAR ON EMP.IdCargo = CAR.IdCargo
		INNER JOIN TblDepartamento DEP ON EMP.IdDepartamento = DEP.IdDepartamento
		WHERE CAP.Nombre = @Nombre
END
-- EXEC USP_GET_TESTS_BY_COURSE_NAME 
GO



CREATE OR ALTER PROC USP_GET_TEST_BY_EMPLOYEE_NAME
 @NombreCompleto VARCHAR(100)
AS
BEGIN
	SELECT 
		EV.IdEvaluacion,
		CAP.Nombre AS Curso_de_la_Evaluación,
		EMP.IdEmpleado,
        EMP.NombreCompleto,
		CAR.Titulo,
		DEP.NombreDepartamento,
		DEP.Ubicacion,
		EV.Calificacion,
		EV.FechaEvaluacion,
		EV.FechaFinalizacion,
		EV.Comentarios
    FROM TblEvaluacion EV
		INNER JOIN TblEmpleado EMP ON EV.IdEmpleado = EMP.IdEmpleado
        INNER JOIN TblCapacitaciones CAP ON EV.IdCapacitacion = CAP.IdCapacitacion
		INNER JOIN TblCargo CAR ON EMP.IdCargo = CAR.IdCargo
		INNER JOIN TblDepartamento DEP ON EMP.IdDepartamento = DEP.IdDepartamento
		WHERE CAP.Nombre = @NombreCompleto
END
-- EXEC USP_GET_TEST_BY_EMPLOYEE_NAME 
GO


CREATE OR ALTER PROC USP_GET_TEST_BY_GRADE
 @Calificacion int
AS
BEGIN
	SELECT 
		EV.IdEvaluacion,
		CAP.Nombre AS Curso_de_la_Evaluación,
		EMP.IdEmpleado,
        EMP.NombreCompleto,
		CAR.Titulo,
		DEP.NombreDepartamento,
		DEP.Ubicacion,
		EV.Calificacion,
		EV.FechaEvaluacion,
		EV.FechaFinalizacion,
		EV.Comentarios
    FROM TblEvaluacion EV
		INNER JOIN TblEmpleado EMP ON EV.IdEmpleado = EMP.IdEmpleado
        INNER JOIN TblCapacitaciones CAP ON EV.IdCapacitacion = CAP.IdCapacitacion
		INNER JOIN TblCargo CAR ON EMP.IdCargo = CAR.IdCargo
		INNER JOIN TblDepartamento DEP ON EMP.IdDepartamento = DEP.IdDepartamento
		WHERE EV.Calificacion = @Calificacion
END
-- EXEC USP_GET_TEST_BY_GRADE 1
GO

 
CREATE OR ALTER PROC USP_GET_TEST_BY_DATE
	@FechaEvaluacion DATE
AS
BEGIN
	SELECT 
		EV.IdEvaluacion,
		CAP.Nombre AS Curso_de_la_Evaluación,
		EMP.IdEmpleado,
        EMP.NombreCompleto,
		CAR.Titulo,
		DEP.NombreDepartamento,
		DEP.Ubicacion,
		EV.Calificacion,
		EV.FechaEvaluacion,
		EV.FechaFinalizacion,
		EV.Comentarios
    FROM TblEvaluacion EV
		INNER JOIN TblEmpleado EMP ON EV.IdEmpleado = EMP.IdEmpleado
        INNER JOIN TblCapacitaciones CAP ON EV.IdCapacitacion = CAP.IdCapacitacion
		INNER JOIN TblCargo CAR ON EMP.IdCargo = CAR.IdCargo
		INNER JOIN TblDepartamento DEP ON EMP.IdDepartamento = DEP.IdDepartamento
		WHERE EV.FechaEvaluacion = @FechaEvaluacion
END
-- EXEC USP_GET_TEST_BY_DATE ''
GO





-- SELECT Asistencia con nombre de empleado
CREATE OR ALTER PROC USP_SELECT_ASISTENCIAS
AS
BEGIN
	SELECT 
		A.IdAsistencia,
		A.IdEmpleado,
		E.NombreCompleto,
		E.DNI,
		E.IdCargo,
		E.IdDepartamento,
		A.Fecha,
		A.HoraEntrada,
		A.HoraSalida
	FROM TblAsistencia A
	JOIN TblEmpleado E ON A.IdEmpleado = E.IdEmpleado
	ORDER BY A.Fecha DESC
END
GO

exec dbo.USP_SELECT_ASISTENCIAS



-- INSERT Asistencia
CREATE OR ALTER PROC USP_INSERT_ASISTENCIA
	@IdEmpleado INT,
	@Fecha DATE,
	@HoraEntrada TIME,
	@HoraSalida TIME
AS
BEGIN
	INSERT INTO TblAsistencia (IdEmpleado, Fecha, HoraEntrada, HoraSalida)
	VALUES (@IdEmpleado, @Fecha, @HoraEntrada, @HoraSalida)
END
GO



-- UPDATE Asistencia
CREATE OR ALTER PROC USP_UPDATE_ASISTENCIA
	@IdAsistencia INT,
	@IdEmpleado INT,
	@Fecha DATE,
	@HoraEntrada TIME,
	@HoraSalida TIME
AS
BEGIN
	UPDATE TblAsistencia
	SET IdEmpleado = @IdEmpleado,
		Fecha = @Fecha,
		HoraEntrada = @HoraEntrada,
		HoraSalida = @HoraSalida
	WHERE IdAsistencia = @IdAsistencia
END
GO



-- DELETE Asistencia
CREATE OR ALTER PROC USP_DELETE_ASISTENCIA
	@IdAsistencia INT
AS
BEGIN
	DELETE FROM TblAsistencia WHERE IdAsistencia = @IdAsistencia
END
GO



-- Listar todos los contratos
CREATE OR ALTER PROC USP_GET_ALL_CONTRATOS
AS
BEGIN
    SELECT 
        C.IdContrato,
        C.IdEmpleado,
        E.NombreCompleto AS NombreEmpleado,
		E.DNI,
		E.IdCargo as Cargo,
		E.IdDepartamento as Departamento,
        C.TipoContrato,
		C.SueldoBase,
        C.FechaInicio,
        C.FechaFin
    FROM TblContrato C
    INNER JOIN TblEmpleado E ON C.IdEmpleado = E.IdEmpleado
    ORDER BY C.IdContrato
END
GO



-- Insertar un contrato
CREATE OR ALTER PROC USP_INSERT_CONTRATO
    @IdEmpleado INT,
    @TipoContrato VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE,
    @SueldoBase DECIMAL(10,2)
AS
BEGIN
    INSERT INTO TblContrato (IdEmpleado, TipoContrato, FechaInicio, FechaFin, SueldoBase)
    VALUES (@IdEmpleado, @TipoContrato, @FechaInicio, @FechaFin, @SueldoBase)
    SELECT SCOPE_IDENTITY() AS IdContrato
END
GO



-- Actualizar un contrato
CREATE OR ALTER PROC USP_UPDATE_CONTRATO
    @IdContrato INT,
    @IdEmpleado INT,
    @TipoContrato VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE,
    @SueldoBase DECIMAL(10,2)
AS
BEGIN
    UPDATE TblContrato
    SET IdEmpleado = @IdEmpleado,
        TipoContrato = @TipoContrato,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        SueldoBase = @SueldoBase
    WHERE IdContrato = @IdContrato
END
GO



-- Eliminar un contrato
CREATE OR ALTER PROC USP_DELETE_CONTRATO
    @IdContrato INT
AS
BEGIN
    DELETE FROM TblContrato WHERE IdContrato = @IdContrato
END
GO



CREATE OR ALTER PROC USP_GET_ONE_CONTRATO_BY_NAME_OF_EMPLOYEE
    @NombreCompleto varchar(100)
AS
BEGIN
    SELECT 
        C.IdContrato,
        C.IdEmpleado,
        E.NombreCompleto AS NombreEmpleado,
		E.DNI,
		E.IdCargo as Cargo,
		E.IdDepartamento as Departamento,
        C.TipoContrato,
		C.SueldoBase,
        C.FechaInicio,
        C.FechaFin
    FROM TblContrato C
    INNER JOIN TblEmpleado E ON C.IdEmpleado = E.IdEmpleado
    WHERE E.NombreCompleto = @NombreCompleto
END
GO




CREATE OR ALTER PROC USP_GET_ALL_CONTRATOS_BY_EMPLOYEES_JOB
    @IdCargo int
AS
BEGIN
    SELECT 
        C.IdContrato,
        C.IdEmpleado,
        E.NombreCompleto AS NombreEmpleado,
		E.DNI,
		E.IdCargo as Cargo,
		E.IdDepartamento as Departamento,
        C.TipoContrato,
		C.SueldoBase,
        C.FechaInicio,
        C.FechaFin
    FROM TblContrato C
    INNER JOIN TblEmpleado E ON C.IdEmpleado = E.IdEmpleado
    WHERE E.IdCargo = @IdCargo
END
GO




CREATE OR ALTER PROC USP_GET_ALL_CONTRATOS_BY_EMPLOYEES_JOB_AREA
    @IdDepartamento int
AS
BEGIN
    SELECT 
        C.IdContrato,
        C.IdEmpleado,
        E.NombreCompleto AS NombreEmpleado,
		E.DNI,
		E.IdCargo as Cargo,
		E.IdDepartamento as Departamento,
        C.TipoContrato,
		C.SueldoBase,
        C.FechaInicio,
        C.FechaFin
    FROM TblContrato C
    INNER JOIN TblEmpleado E ON C.IdEmpleado = E.IdEmpleado
    WHERE E.IdDepartamento = @IdDepartamento
END
GO


CREATE OR ALTER PROC USP_GET_ALL_CONTRATOS_BY_TYPE
    @TipoContrato varchar (50)
AS
BEGIN
    SELECT 
        C.IdContrato,
        C.IdEmpleado,
        E.NombreCompleto AS NombreEmpleado,
		E.DNI,
		E.IdCargo as Cargo,
		E.IdDepartamento as Departamento,
        C.TipoContrato,
		C.SueldoBase,
        C.FechaInicio,
        C.FechaFin
    FROM TblContrato C
    INNER JOIN TblEmpleado E ON C.IdEmpleado = E.IdEmpleado
    WHERE C.TipoContrato = @TipoContrato
END
GO




-- usuarios 


-- SELECT ALL USUARIOS
CREATE OR ALTER PROC USP_GET_ALL_USUARIOS
AS
BEGIN
	SELECT 
		U.IdUsuario,
		U.Usuario,
		U.Contrasena,
		U.Rol,
		E.IdEmpleado
	FROM TblUsuario U
	JOIN TblEmpleado E ON U.IdEmpleado = E.IdEmpleado
	ORDER BY U.Usuario
END
GO

exec dbo.USP_GET_ALL_USUARIOS



-- INSERT USUARIO
CREATE OR ALTER PROC USP_INSERT_USUARIO
	@Usuario VARCHAR(50),
	@Contrasena varbinary(64),
	@Rol VARCHAR(50),
	@IdEmpleado INT
AS
BEGIN
	INSERT INTO TblUsuario (Usuario, Contrasena, Rol, IdEmpleado)
	VALUES (@Usuario, @Contrasena, @Rol, @IdEmpleado)
END
GO



-- UPDATE USUARIO
CREATE OR ALTER PROC USP_UPDATE_USUARIO
	@IdUsuario INT,
	@Usuario VARCHAR(50),
	@Contrasena varbinary(64),
	@Rol VARCHAR(50),
	@IdEmpleado INT
AS
BEGIN
	UPDATE TblUsuario
	SET 
		Usuario = @Usuario,
		Contrasena = @Contrasena,
		Rol = @Rol,
		IdEmpleado = @IdEmpleado
	WHERE IdUsuario = @IdUsuario
END
GO





-- DELETE USUARIO
CREATE OR ALTER PROC USP_DELETE_USUARIO
	@IdUsuario INT
AS
BEGIN
	DELETE FROM TblUsuario
	WHERE IdUsuario = @IdUsuario
END
GO










--  login de usuarios

CREATE or alter PROCEDURE sp_LoginUsuario
    @Usuario VARCHAR(50),
    @Contrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar si el usuario existe
    IF NOT EXISTS (SELECT 1 FROM TblUsuario WHERE Usuario = @Usuario)
    BEGIN
        RAISERROR('El usuario no existe.', 16, 1);
        RETURN;
    END

    DECLARE @HashedInputPassword VARBINARY(64);
    SET @HashedInputPassword = HASHBYTES('SHA2_256', CONVERT(VARCHAR(255), @Contrasena));

    DECLARE @StoredPassword VARBINARY(64);
    DECLARE @Rol VARCHAR(50);
    DECLARE @IdUsuario INT;

    SELECT 
        @StoredPassword = Contrasena,
        @Rol = Rol,
        @IdUsuario = IdUsuario
    FROM TblUsuario
    WHERE Usuario = @Usuario;

    -- Comparar hashes
    IF @StoredPassword <> @HashedInputPassword
    BEGIN
        RAISERROR('Contraseña incorrecta.', 16, 1);
        RETURN;
    END

    -- Éxito: devolver info útil al frontend
    SELECT 
        @IdUsuario AS IdUsuario,
        @Usuario AS Usuario,
        @Rol AS Rol,
        'Login exitoso' AS Mensaje;
END
GO
