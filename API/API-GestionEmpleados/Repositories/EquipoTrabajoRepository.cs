using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace API_GestionEmpleados.Repositories
{
    public class EquipoTrabajoRepository : IEquipoTrabajoRepository
    {
        private readonly IDatabaseExecutor _executor;

        public EquipoTrabajoRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IEnumerable<EquipoTrabajoResponse>> ObtenerTodosAsync()
        {
            var sp = "USP_GET_ALL_EQUIPOS";
            return await _executor.ExecuteCommand(con => con.QueryAsync<EquipoTrabajoResponse>(sp, commandType: CommandType.StoredProcedure));
        }


        public async Task<int> InsertarAsync(EquipoTrabajoInsertRequest request)
        {
            var sp = "USP_INSERT_EQUIPOTRABAJO";
            var parameters = new DynamicParameters();
            parameters.Add("@IdModelo", request.IdModelo);
            parameters.Add("@NombreEquipo", request.NombreEquipo);
            parameters.Add("@TipoEquipo", request.TipoEquipo);
            parameters.Add("@Marca", request.Marca);
            parameters.Add("@FechaAsignacion", request.FechaAsignacion);
            parameters.Add("@Estado", request.Estado);
            return await _executor.ExecuteCommand(con => con.ExecuteScalarAsync<int>(sp, parameters, commandType: CommandType.StoredProcedure));
        }


        public async Task<bool> ActualizarAsync(int id, EquipoTrabajoUpdateRequest request)
        {
            var sp = "USP_UPDATE_EQUIPOTRABAJO";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEquipo", id);
            parameters.Add("@IdModelo", request.IdModelo);
            parameters.Add("@NombreEquipo", request.NombreEquipo);
            parameters.Add("@TipoEquipo", request.TipoEquipo);
            parameters.Add("@Marca", request.Marca);
            parameters.Add("@FechaAsignacion", request.FechaAsignacion);
            parameters.Add("@Estado", request.Estado);
            var result = await _executor.ExecuteCommand(con => con.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure));
            return result > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var sp = "USP_DELETE_EQUIPOTRABAJO";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEquipo", id);
            var result = await _executor.ExecuteCommand(con => con.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure));
            return result > 0;
        }



        public async Task<EquipoTrabajoResponse> ObtenerPorIdAsync(int id)
        {
            var sp = "USP_GET_ONE_EQUIPO_BY_ID";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEquipo", id, DbType.Int32);
            return await _executor.ExecuteCommand(con => con.QueryFirstOrDefaultAsync<EquipoTrabajoResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
        }

    }
}
