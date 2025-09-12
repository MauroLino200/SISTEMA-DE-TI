using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.Empleados;
using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Empleados;
using API_GestionEmpleados.Models.Response.Evaluaciones;
using API_GestionEmpleados.Repositories.Interfaces;
using Azure.Core;
using Dapper;
using System.Data;

namespace API_GestionEmpleados.Repositories
{
    public class EvaluacionRepository : IEvaluacionRepository
    {
        private readonly IDatabaseExecutor _executor;

        public EvaluacionRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        #region Seleccion y Filtros

        public async Task<IEnumerable<EvaluacionResponse>> ObtenerTodasLasEvaluacionesAsync()
        {
            var sp = "USP_GET_ALL_TESTS";
            try
            {
                var listado = await _executor.ExecuteCommand(conexion => conexion.QueryAsync<EvaluacionResponse>(sp));
                return listado;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionPorIdAsync(int id)
        {
            var sp = "USP_GET_ONE_EVALUACION_BY_ID";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEvaluacion", id, DbType.Int32);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<EvaluacionResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener evaluaciones: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorEmpleadoAsync(int idEmpleado)
        {
            var sp = "USP_GET_ONE_EVALUACION_BY_EMPlOYEE_ID";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", idEmpleado, DbType.Int32);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<EvaluacionResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener evaluaciones: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorFechanAsync(DateTime fecha)
        {
            var sp = "USP_GET_TEST_BY_DATE";
            var parameters = new DynamicParameters();
            parameters.Add("@FechaEvaluacion", fecha, DbType.Int32);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<EvaluacionResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener evaluaciones: {ex.Message}", ex);
            }
        }

        
        public async Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorNombreCursoAsync(string nom_curso)
        {
            var sp = "USP_GET_TESTS_BY_COURSE_NAME";
            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", nom_curso, DbType.String);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<EvaluacionResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener nombre del curso: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorNombreEmpleadoAsync(string nom_completo)
        {
            var sp = "USP_GET_TEST_BY_EMPLOYEE_NAME";
            var parameters = new DynamicParameters();
            parameters.Add("@NombreCompleto", nom_completo, DbType.String);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<EvaluacionResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener nombre del empleado: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorNotaAsync(int calificacion) 
        {
            var sp = "USP_GET_TEST_BY_GRADE";
            var parameters = new DynamicParameters();
            parameters.Add("@Calificacion", calificacion, DbType.String);

            try
            {
                var registros = await _executor.ExecuteCommand(
                    conexion => conexion.QueryAsync<EvaluacionResponse>(
                        sp,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )
                );

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las notas: {ex.Message}", ex);
            }
        }

        #endregion

        public async Task<EvaluacionInsertRequest?> InsertarEvaluacionAsync(EvaluacionInsertRequest evaluacion)
        {
            var sp = "USP_INSERT_EVALUACION";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", evaluacion.IdEmpleado, DbType.Int32);
            parameters.Add("@IdCapacitacion", evaluacion.IdCapacitacion, DbType.Int32);
            parameters.Add("@FechaEvaluacion", evaluacion.FechaEvaluacion, DbType.DateTime);
            parameters.Add("@FechaFinalizacion", evaluacion.FechaFinalizacion, DbType.DateTime);
            parameters.Add("@Estado", evaluacion.Estado, DbType.Boolean);
            parameters.Add("@Calificacion", evaluacion.Calificacion, DbType.Int32);
            parameters.Add("@Comentarios", evaluacion.Comentarios, DbType.String);

            try
            {
                var result = await _executor.ExecuteCommand(
                    conexion => conexion.QueryFirstOrDefaultAsync<EvaluacionInsertRequest>(
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

        public async Task<EvalucacionUpdateRequest?> ActualizarEvaluacionAsync(int id, EvalucacionUpdateRequest evaluacion)
        {
            var sp = "USP_UPDATE_EVALUACION";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEvaluacion", id, DbType.Int32);
            parameters.Add("@IdEmpleado", evaluacion.IdEmpleado, DbType.Int32);
            parameters.Add("@IdCapacitacion", evaluacion.IdCapacitacion, DbType.Int32);
            parameters.Add("@FechaEvaluacion", evaluacion.FechaEvaluacion, DbType.DateTime);
            parameters.Add("@FechaFinalizacion", evaluacion.FechaFinalizacion, DbType.DateTime);
            parameters.Add("@Estado", evaluacion.Estado, DbType.String);
            parameters.Add("@Calificacion", evaluacion.Calificacion, DbType.Int32);
            parameters.Add("@Comentarios", evaluacion.Comentarios, DbType.String);

            try
            {
                var result = await _executor.ExecuteCommand(
                    conexion => conexion.QueryFirstOrDefaultAsync<EvalucacionUpdateRequest>(
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

        public async Task<bool> EliminarEvaluacionAsync(int id)
        {
            var sp = "USP_DELETE_EVALUACION";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEvaluacion", id, DbType.Int32);

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
                throw new Exception($"Error al eliminar la evaluación: {ex.Message}", ex);
            }
        }
    }
}
