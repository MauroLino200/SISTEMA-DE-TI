using API_GestionEmpleados.Models.Request.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IEquipoTrabajoRepository
    {
        Task<IEnumerable<EquipoTrabajoResponse>> ObtenerTodosAsync();
        Task<int> InsertarAsync(EquipoTrabajoInsertRequest request);
        Task<bool> ActualizarAsync(int id, EquipoTrabajoUpdateRequest request);
        Task<bool> EliminarAsync(int id);

        Task<EquipoTrabajoResponse> ObtenerPorIdAsync(int id);

        Task<EquipoTrabajoResponse> ObtenerPorNombreAsync(string nom_equipo);

        Task<EquipoTrabajoResponse> ObtenerPorTipoEquipoAsync(string tipo_equipo);

        Task<EquipoTrabajoResponse> ObtenerPorEstadoAsync(string estado);

        Task<EquipoTrabajoResponse> ObtenerPorFechaAsync(DateTime fecha);

    }
}
