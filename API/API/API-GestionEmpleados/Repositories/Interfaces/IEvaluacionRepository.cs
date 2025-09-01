using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Evaluaciones;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IEvaluacionRepository
    {
        Task<EvaluacionInsertRequest?> InsertarEvaluacionAsync(EvaluacionInsertRequest evaluacion);
        Task<EvalucacionUpdateRequest?> ActualizarEvaluacionAsync(int id, EvalucacionUpdateRequest evaluacion);
        Task<bool> EliminarEvaluacionAsync(int id);
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionPorIdAsync(int id);
        Task<IEnumerable<EvaluacionResponse>> ObtenerTodasLasEvaluacionesAsync();
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorEmpleadoAsync(int idEmpleado);
        Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorCapacitacionAsync(int idCapacitacion);
         Task<IEnumerable<EvaluacionResponse>> ObtenerEvaluacionesPorFechanAsync(DateTime fecha);
    }
}
