using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.Devoluciones;
using API_GestionEmpleados.Models.Response.Devoluciones;
using API_GestionEmpleados.Models.Response.Reserva;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace API_GestionEmpleados.Repositories
{
    public class DevolucionesRepository : IDevolucionesRepository
    {
        private readonly IDatabaseExecutor _executor;

        public DevolucionesRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        #region seleccionables y filtros

        public async Task<IEnumerable<DevolucionesResponse>> ObtenerTodosAsync()
        {
            var sp = "USP_GET_ALL_DEVOLUCIONES";
            return await _executor.ExecuteCommand(con => con.QueryAsync<DevolucionesResponse>(sp, commandType: CommandType.StoredProcedure));
        }

        public async Task<DevolucionesResponse> ObtenerPorIdDevolucionAsync(int id_devolucion)
        {
            var sp = "USP_GET_ONE_DEVOLUCION_BY_ITS_ID";
            var parameters = new DynamicParameters();
            parameters.Add("IdDevolucion", id_devolucion, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<DevolucionesResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<DevolucionesResponse> ObtenerPorNombreEquipoAsync(string nombre_equipo)
        {
            var sp = "USP_GET_ONE_DEVOLUCION_BY_ITS_NAME";
            var parameters = new DynamicParameters();
            parameters.Add("@NombreEquipo", nombre_equipo, DbType.String);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<DevolucionesResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<DevolucionesResponse> ObtenerPorIdModeloAsync(int id_modelo)
        {
            var sp = "USP_GET_ONE_DEVOLUCION_BY_ITS_TYPE";
            var parameters = new DynamicParameters();
            parameters.Add("@IdModelo", id_modelo, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<DevolucionesResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<DevolucionesResponse> ObtenerPorIdEmpleadoAsync(int id_empleado)
        {
            var sp = "USP_GET_ONE_DEVOLUCION_BY_ID_EMPLOYEE";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", id_empleado, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<DevolucionesResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<DevolucionesResponse> ObtenerPorFechaAsync(DateTime fecha_devolucion)
        {
            var sp = "USP_GET_ONE_DEVOLUCION_BY_DATE";
            var parameters = new DynamicParameters();
            parameters.Add("@FechaDevolucion", fecha_devolucion, DbType.DateTime);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<DevolucionesResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }
        #endregion

        public async Task<int> InsertarAsync(DevolucionesInsertRequest request)
        {
            var sp = "USP_InsertDevolucionEquipo";
            var parameters = new DynamicParameters();
            parameters.Add("@IdReserva", request.IdReserva);
            return await _executor.ExecuteCommand(con =>
                con.ExecuteScalarAsync<int>(sp, parameters, commandType: CommandType.StoredProcedure));
        }
    }
}
