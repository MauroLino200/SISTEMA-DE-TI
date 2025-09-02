using API_GestionEmpleados.Models.Request.Devoluciones;
using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.Devoluciones;
using API_GestionEmpleados.Models.Response.Reserva;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IDevolucionesRepository
    {
        Task<IEnumerable<DevolucionesResponse>> ObtenerTodosAsync();

        Task<DevolucionesResponse> ObtenerPorIdDevolucionAsync(int id_devolucion);

        Task<DevolucionesResponse> ObtenerPorNombreEquipoAsync(string nombre_equipo);

        Task<DevolucionesResponse> ObtenerPorIdModeloAsync(int id_modelo);

        Task<DevolucionesResponse> ObtenerPorIdEmpleadoAsync(int id_empleado);

        Task<DevolucionesResponse> ObtenerPorFechaAsync(DateTime fecha_devolucion);

        Task<int> InsertarAsync(DevolucionesInsertRequest request);
    }
}
