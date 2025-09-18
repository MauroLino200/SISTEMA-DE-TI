using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.Asistencias;
using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Asistencias;
using API_GestionEmpleados.Models.Response.Evaluaciones;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace API_GestionEmpleados.Repositories
{
    public class AsistenciasRepository : IAsistenciasRepository
    {
        private readonly IDatabaseExecutor _executor;

        public AsistenciasRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IEnumerable<AsistenciasResponse>> ObtenerTodasLasAsistenciasAsync()
        {
            var sp = "USP_SELECT_ASISTENCIAS";
            try
            {
                var listado = await _executor.ExecuteCommand(conexion => conexion.QueryAsync<AsistenciasResponse>(sp));
                return listado;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<AsistenciasResponse>> ObtenerAsistenciaPorFechaAsync(DateTime fecha)
        {
            var sp = "USP_GET_ASISTENCIA_BY_DATE";
            var parameters = new DynamicParameters();
            parameters.Add("@HoraEntrada", fecha, DbType.DateTime);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<AsistenciasResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener asistencias: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AsistenciasResponse>> ObtenerAsistenciaPorNombreEmpleadoAsync(string nom_emp)
        {
            var sp = "USP_GET_ASISTENCIA_BY_EMPLOYEE_NAME";
            var parameters = new DynamicParameters();
            parameters.Add("@NombreCompleto", nom_emp, DbType.String);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<AsistenciasResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener asistencias: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AsistenciasResponse>> ObtenerAsistenciaPorNumeroDocumentoAsync(int num_doc)
        {
            var sp = "USP_GET_ASISTENCIA_BY_DOCUMENT_NUMBER";
            var parameters = new DynamicParameters();
            parameters.Add("@NumeroDocumento", num_doc, DbType.Int32);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<AsistenciasResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener asistencias: {ex.Message}", ex);
            }
        }

       
        public async Task<bool> EliminarAsistenciaAsync(int id)
        {
            var sp = "USP_DELETE_ASISTENCIA";
            var parameters = new DynamicParameters();
            parameters.Add("@IdAsistencia", id, DbType.Int32);

            try
            {
                // Ejecuta el SP y obtiene el número de filas afectadas
                var filasAfectadas = await _executor.ExecuteCommand(
                    async conexion => await conexion.ExecuteAsync(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                // Si filasAfectadas > 0, la eliminación fue exitosa
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la asistencia: {ex.Message}", ex);
            }
        }

        public async Task<AsistenciasInsert?> InsertarAsistenciaAsync(AsistenciasInsert asistencias)
        {
            var sp = "USP_INSERT_ASISTENCIA";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", asistencias.IdEmpleado, DbType.Int32);
            parameters.Add("@HoraEntrada", asistencias.HoraEntrada, DbType.DateTime);
            parameters.Add("@HoraSalida", asistencias.HoraSalida, DbType.DateTime);

            try
            {
                var result = await _executor.ExecuteCommand(
                    conexion => conexion.QueryFirstOrDefaultAsync<AsistenciasInsert>(
                        sp, parameters, commandType: CommandType.StoredProcedure
                    )
                );
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AsistenciasUpdate?> ActualizarAsistenciaAsync(int id, AsistenciasUpdate asistencias)
        {
            var sp = "USP_UPDATE_ASISTENCIA";
            var parameters = new DynamicParameters();
            parameters.Add("@IdAsistencia", id, DbType.Int32);
            parameters.Add("@IdEmpleado", asistencias.IdEmpleado, DbType.Int32);
            parameters.Add("@HoraEntrada", asistencias.HoraEntrada, DbType.DateTime);
            parameters.Add("@HoraSalida", asistencias.HoraSalida, DbType.DateTime);

            try
            {
                var result = await _executor.ExecuteCommand(
                    conexion => conexion.QueryFirstOrDefaultAsync<AsistenciasUpdate>(
                        sp, parameters, commandType: CommandType.StoredProcedure
                    )
                );
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
