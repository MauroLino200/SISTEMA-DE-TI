using API_GestionEmpleados.Models.Request.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IEquipoTrabajoRepository
    {
        Task<IEnumerable<EquipoTrabajoResponse>> ObtenerTodosAsync();
        Task<EquipoTrabajoResponse> ObtenerPorIdAsync(int id);
        Task<int> InsertarAsync(EquipoTrabajoInsertRequest request);
        Task<bool> ActualizarAsync(int id, EquipoTrabajoUpdateRequest request);
        Task<bool> EliminarAsync(int id);
    }
}
