using API_GestionEmpleados.Models.Request.Devoluciones;
using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.Devoluciones;
using API_GestionEmpleados.Models.Response.Reserva;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IDevolucionesRepository
    {
        Task<IEnumerable<DevolucionesResponse>> ObtenerTodosAsync();

        Task<int> InsertarAsync(DevolucionesInsertRequest request);

        Task<DevolucionesResponse> ObtenerPorIdAsync(int id);

    }
}
