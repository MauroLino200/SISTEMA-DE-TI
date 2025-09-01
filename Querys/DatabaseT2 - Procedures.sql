USE DatabaseT2
GO

--- PROCEDIMIENTOS ALMACENADOS ---
CREATE OR ALTER PROC USP_SELECT_DENTISTAS 
AS
BEGIN
	SELECT 
		[Value]	= D.IdDentista,
		[Name]	= D.NombreCompleto 
	FROM TblDentista D
END
-- EXEC dbo.USP_SELECT_DENTISTAS
GO

CREATE OR ALTER PROC USP_SEARCH_EQUIPOS
AS
BEGIN
    SELECT 
        NroEquipo			= E.NroEquipo,
        NombreEquipo		= E.NombreEquipo,
        TipoEquipo			= E.TipoEquipo ,
        FechaAdquisicion	= E.FechaAdquisicion,
        Dentista			= D.Cop + ' - ' + D.NombreCompleto,
        Especialidad		= ES.Titulo,
        Estado				= CASE E.Estado 
								WHEN 'N' THEN 'Nuevo'
								WHEN 'A' THEN 'Alquilado'
								WHEN 'R' THEN 'Reparado'
								ELSE 'Segunda'
							  END
    FROM TblEquipoDental E
		INNER JOIN TblDentista D ON E.IdDentista = D.IdDentista
		INNER JOIN TblEspecialidad ES ON D.IdEspecialidad = ES.IdEspecialidad
    ORDER BY 
		E.NroEquipo DESC
END
-- EXEC dbo.USP_SEARCH_EQUIPOS
GO

CREATE OR ALTER PROC USP_GET_ONE_EQUIPO 
	@NroEquipo INT
AS
BEGIN
	SELECT
		E.NroEquipo,
		E.NombreEquipo,
		E.TipoEquipo,
		E.FechaAdquisicion,
		E.Estado,
		E.IdDentista
	FROM TblEquipoDental AS E
	WHERE 
		E.NroEquipo = @NroEquipo
END
-- EXEC dbo.USP_GET_ONE_EQUIPO 4
GO

CREATE OR ALTER PROC USP_UPDATE_EQUIPO
	@NroEquipo INT,
    @NombreEquipo VARCHAR(100),
    @TipoEquipo VARCHAR(50), 
    @FechaAdquisicion DATE,
    @Estado CHAR(1), 
    @IdDentista INT
AS 
BEGIN
	UPDATE TblEquipoDental
	SET
		NombreEquipo		= @NombreEquipo,
		TipoEquipo			= @TipoEquipo,
		FechaAdquisicion	= @FechaAdquisicion,
		Estado				= @Estado,
		IdDentista			= @IdDentista
	WHERE 
		NroEquipo = @NroEquipo
END
-- EXEC dbo.USP_UPDATE_EQUIPO 4, 'Aspirador', 'Diagnostico', '2025-02-20', 'N', 2
GO
