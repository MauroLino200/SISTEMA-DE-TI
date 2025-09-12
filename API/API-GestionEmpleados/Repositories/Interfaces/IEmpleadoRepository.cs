using API_GestionEmpleados.Models.Request.Empleados;
using API_GestionEmpleados.Models.Response.Empleados;
namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IEmpleadoRepository
    {

        // Get all Empleados on the system
        Task<IEnumerable<EmpleadoResponse>> GetAllEmpleadosAsync();

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByIdAsync(int id_emp);

        // Get Empleado by its Dcocument Number
        Task<EmpleadoResponse> GetOneEmpleadoByItsNumberDocumentAsync(string num_doc);

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByNameAsync(string nom_emp);

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByDateAsync(DateTime fecha_ing);

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByTurnoAsync(string turno);

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByEmailAsync(string email);

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByCargoAsync(int cargo);

        // Get Empleado by its ID
        Task<EmpleadoResponse> GetOneEmpleadoByOfficeAsync(int id_dep);

        // Add a new Empleado 
        Task<EmpleadoCreateRequest> AddEmpleadoAsync(EmpleadoCreateRequest request);

        // Update Empleado by its ID
        Task<string> UpdateEmpleadoAsync(int id, EmpleadoUpdateRequest request );

        // Uncomment the following methods if you need them in your repository
        //Task<bool> DeleteEmpleadoAsync(int id);
        //Task<bool> EmpleadoExistsAsync(int id);
    }
}
