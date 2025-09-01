using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Empleados;
using API_GestionEmpleados.Models.Response.Evaluaciones;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionController : ControllerBase
    {
        private readonly IEvaluacionRepository _evaluacionRepository;

        public EvaluacionController(IEvaluacionRepository evaluacionesRepository)
        {
            _evaluacionRepository = evaluacionesRepository;
        }


        [HttpGet("action")]
        public async Task<ActionResult<IEnumerable<EvaluacionResponse>>> ObtenerTodasLasEvaluacionesAsync()
        {
            try
            {
                var evaluaciones = await _evaluacionRepository.ObtenerTodasLasEvaluacionesAsync();
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("capacitacion/{id}")]
        public async Task<ActionResult<EvaluacionResponse>> ObtenerEvaluacionesPorCapacitacionAsync(int idCapacitacion)
        {
            try
            {
                var evaluaciones = await _evaluacionRepository.ObtenerEvaluacionesPorCapacitacionAsync(idCapacitacion);
                if (evaluaciones == null)
                {
                    return NotFound($"Empleado with ID {idCapacitacion} not found.");
                }
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("empleado/{id}")]
        public async Task<ActionResult<EvaluacionResponse>> ObtenerEvaluacionesPorEmpleadoAsync(int idEmpleado)
        {
            try
            {
                var empleados = await _evaluacionRepository.ObtenerEvaluacionesPorEmpleadoAsync(idEmpleado);
                if (empleados == null)
                {
                    return NotFound($"Empleado with ID {idEmpleado} not found.");
                }
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("por-fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<EvaluacionResponse>>> ObtenerEvaluacionesPorFechanAsync(DateTime fecha)
        {
            try
            {
                var evaluaciones = await _evaluacionRepository.ObtenerEvaluacionesPorFechanAsync(fecha);
                if (evaluaciones == null || !evaluaciones.Any())
                {
                    return NotFound($"No se encontraron evaluaciones para la fecha {fecha:yyyy-MM-dd}.");
                }
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al recuperar los datos: {ex.Message}");
            }
        }


        [HttpGet("evaluacion/{id}")]
        public async Task<ActionResult<EvaluacionResponse>> ObtenerEvaluacionPorIdAsync(int id)
        {
            try
            {
                var evaluaciones = await _evaluacionRepository.ObtenerEvaluacionPorIdAsync(id);
                if (evaluaciones == null)
                {
                    return NotFound($"Empleado with ID {id} not found.");
                }
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<EvaluacionInsertRequest>> InsertarEvaluacionAsync(EvaluacionInsertRequest evaluacion)
        {
            try
            {
                var resultado = await _evaluacionRepository.InsertarEvaluacionAsync(evaluacion);
                if (resultado == null)
                {
                    return BadRequest("No se pudo insertar la evaluación.");
                }
                // No se puede usar resultado.IdEvaluacion porque EvaluacionInsertRequest no tiene esa propiedad.
                // Se debe devolver simplemente el resultado o, si el método InsertarEvaluacionAsync retorna un tipo con IdEvaluacion, usar ese tipo.
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al insertar la evaluación: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EvalucacionUpdateRequest>> ActualizarEvaluacionAsync(int id, EvalucacionUpdateRequest evaluacion)
        {
            try
            {
                var resultado = await _evaluacionRepository.ActualizarEvaluacionAsync(id, evaluacion);
                if (resultado == null)
                {
                    return NotFound($"No se encontró la evaluación con ID {id}.");
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la evaluación: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarEvaluacionAsync(int id)
        {
            try
            {
                var eliminado = await _evaluacionRepository.EliminarEvaluacionAsync(id);
                if (!eliminado)
                {
                    return NotFound($"No se encontró la evaluación con ID {id}.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la evaluación: {ex.Message}");
            }
        }




    }
}
