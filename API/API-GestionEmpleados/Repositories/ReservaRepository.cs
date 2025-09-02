using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.Reserva;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;
using System.Data;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_GestionEmpleados.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly IDatabaseExecutor _executor;

        public ReservaRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        #region selecciones y filtrados
        public async Task<IEnumerable<ReservaResponse>> ObtenerTodosAsync()
        {
            var sp = "USP_GET_ALL_RESERVAS";
            return await _executor.ExecuteCommand(con => con.QueryAsync<ReservaResponse>(sp, commandType: CommandType.StoredProcedure));
        }

        public async Task<ReservaResponse> ObtenerPorIdAsync(int id)
        {
            var sp = "USP_GET_ONE_RESERVA_BY_ITS_ID";
            var parameters = new DynamicParameters();
            parameters.Add("@IdReserva", id, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<ReservaResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<ReservaResponse> ObtenerPorNombreAsync(string nombre)
        {
            var sp = "USP_GET_ONE_RESERVA_BY_ITS_NAME";
            var parameters = new DynamicParameters();
            parameters.Add("@NombreEquipo", nombre, DbType.String);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<ReservaResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<ReservaResponse> ObtenerPorModeloAsync(int modelo)
        {
            var sp = "USP_GET_ONE_RESERVA_BY_ITS_TYPE_OF_THING";
            var parameters = new DynamicParameters();
            parameters.Add("@IdModelo", modelo, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<ReservaResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<ReservaResponse> ObtenerPorIdEmpleadoAsync(int id_empleado)
        {
            var sp = "USP_GET_ONE_RESERVA_BY_ID_OF_EMPLOYEE";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", id_empleado, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<ReservaResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<ReservaResponse> ObtenerPorFechaReservaAsync(DateTime fecha_reserva)
        {
            var sp = "USP_GET_ONE_RESERVA_BY_ITS_DATE";
            var parameters = new DynamicParameters();
            parameters.Add("@FechaReserva", fecha_reserva, DbType.DateTime);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<ReservaResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }
        #endregion

        #region insercion, actualizacion y eliminacion

        public async Task<int> InsertarAsync(ReservaInsertRequest request)
        {
            var sp = "USP_InsertReservaEquipo";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", request.IdEmpleado);
            parameters.Add("@IdEquipo", request.IdEquipo);

            return await _executor.ExecuteCommand(con => con.ExecuteScalarAsync<int>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<bool> ActualizarAsync(int id, ReservaUpdateRequest request)
        {
            var sp = "USP_ActualizarReservaEquipo";
            var parameters = new DynamicParameters();
            parameters.Add("@IdReserva", request.IdReserva);
            parameters.Add("@NuevoIdEmpleado", request.NuevoIdEmpleado);
            parameters.Add("@NuevoIdEquipo", request.NuevoIdEmpleado);

            var result = await _executor.ExecuteCommand(con => con.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure));
            return result > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var sp = "USP_EliminarReservaEquipo";
            var parameters = new DynamicParameters();
            parameters.Add("@IdReserva", id);
            var result = await _executor.ExecuteCommand(con => con.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure));
            return result > 0;
        }

        #endregion

    }
}
