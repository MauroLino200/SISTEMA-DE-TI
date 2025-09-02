using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.Reserva;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        Task<IEnumerable<ReservaResponse>> ObtenerTodosAsync();
        Task<ReservaResponse> ObtenerPorIdAsync(int id);

        Task<ReservaResponse> ObtenerPorNombreAsync(string nombre);
        Task<ReservaResponse> ObtenerPorModeloAsync(int modelo);
        Task<ReservaResponse> ObtenerPorIdEmpleadoAsync(int id_empleado);
        Task<ReservaResponse> ObtenerPorFechaReservaAsync(DateTime fecha_reserva);
        Task<int> InsertarAsync(ReservaInsertRequest request);
        Task<bool> ActualizarAsync(int id, ReservaUpdateRequest request);
        Task<bool> EliminarAsync(int id);
    }
}
