using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.Reserva;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        Task<IEnumerable<ReservaResponse>> ObtenerTodosAsync();
        Task<ReservaResponse> ObtenerPorIdAsync(int id);
        Task<int> InsertarAsync(ReservaInsertRequest request);
        Task<bool> ActualizarAsync(int id, ReservaUpdateRequest request);
        Task<bool> EliminarAsync(int id);
    }
}
