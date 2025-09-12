using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Evaluaciones;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IEvaluacionRepository
    {
        Task<IEnumerable<EvaluacionResponse>> ObtenerTodasLasEvaluacionesAsync();
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionPorIdAsync(int id);
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorEmpleadoAsync(int idEmpleado);
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorFechanAsync(DateTime fecha);

        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorNombreCursoAsync(string nom_curso);
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorNombreEmpleadoAsync(string nom_completo);
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorNotaAsync(int calificacion);

        Task<EvaluacionInsertRequest?> InsertarEvaluacionAsync(EvaluacionInsertRequest evaluacion);
        Task<EvalucacionUpdateRequest?> ActualizarEvaluacionAsync(int id, EvalucacionUpdateRequest evaluacion);
        Task<bool> EliminarEvaluacionAsync(int id);

    }
}
