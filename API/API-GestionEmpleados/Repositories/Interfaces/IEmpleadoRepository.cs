using API_GestionEmpleados.Models.Request.Empleados;
using API_GestionEmpleados.Models.Response.Empleados;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IEmpleadoRepository
    {

        // Get all Empleados on the system
        Task<IEnumerable<EmpleadoResponse>> GetAllEmpleadosAsync();

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetEmpleadoByIdAsync(int id);

        // Add a new Empleado 
        Task<EmpleadoCreateRequest> AddEmpleadoAsync(EmpleadoCreateRequest request);

        // Update Empleado by its ID
        Task<string> UpdateEmpleadoAsync(int id, EmpleadoUpdateRequest request );

        // Uncomment the following methods if you need them in your repository
        //Task<bool> DeleteEmpleadoAsync(int id);
        //Task<bool> EmpleadoExistsAsync(int id);
    }
}
