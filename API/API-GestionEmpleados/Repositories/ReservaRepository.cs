using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.Reserva;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace API_GestionEmpleados.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly IDatabaseExecutor _executor;

        public ReservaRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IEnumerable<ReservaResponse>> ObtenerTodosAsync()
        {
            var sp = "USP_GET_ALL_RESERVAS";
            return await _executor.ExecuteCommand(con => con.QueryAsync<ReservaResponse>(sp, commandType: CommandType.StoredProcedure));
        }

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

       

        public async Task<ReservaResponse> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

       
    }
}
