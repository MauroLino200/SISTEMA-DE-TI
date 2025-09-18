using API_GestionEmpleados.Models.Request.Asistencias;
using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Asistencias;
using API_GestionEmpleados.Models.Response.Evaluaciones;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IAsistenciasRepository
    {
        Task<IEnumerable<AsistenciasResponse>> ObtenerTodasLasAsistenciasAsync();

        Task<IEnumerable<AsistenciasResponse>> ObtenerAsistenciaPorNombreEmpleadoAsync(string nom_emp);

        Task<IEnumerable<AsistenciasResponse>> ObtenerAsistenciaPorNumeroDocumentoAsync(int num_doc);

        Task<IEnumerable<AsistenciasResponse>> ObtenerAsistenciaPorFechaAsync(DateTime fecha);

        Task<AsistenciasInsert?> InsertarAsistenciaAsync(AsistenciasInsert asistencias);
        
        Task<AsistenciasUpdate?> ActualizarAsistenciaAsync(int id, AsistenciasUpdate asistencias);

        Task<bool> EliminarAsistenciaAsync(int id);

    }
}
