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

        public async Task<IEnumerable<DevolucionesResponse>> ObtenerTodosAsync()
        {
            var sp = "USP_GET_ALL_DEVOLUCIONES";
            return await _executor.ExecuteCommand(con => con.QueryAsync<DevolucionesResponse>(sp, commandType: CommandType.StoredProcedure));
        }

        public async Task<int> InsertarAsync(DevolucionesInsertRequest request)
        {
            var sp = "USP_InsertDevolucionEquipo";
            var parameters = new DynamicParameters();
            parameters.Add("@IdReserva", request.IdReserva);
            return await _executor.ExecuteCommand(con =>
                con.ExecuteScalarAsync<int>(sp, parameters, commandType: CommandType.StoredProcedure));
        }


        public Task<DevolucionesResponse> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
