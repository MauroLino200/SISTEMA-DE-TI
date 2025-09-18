using API_GestionEmpleados.Models.Request.Asistencias;
using API_GestionEmpleados.Models.Request.Evaluaciones;
using API_GestionEmpleados.Models.Response.Asistencias;
using API_GestionEmpleados.Models.Response.Evaluaciones;
using API_GestionEmpleados.Repositories;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciasRepository _asistenciaRepository;

        public AsistenciaController(IAsistenciasRepository asistenciaRepository)
        {
            _asistenciaRepository = asistenciaRepository;
        }

        [HttpGet("get_asistencias")]
        public async Task<ActionResult<IEnumerable<AsistenciasResponse>>> ObtenerTodasLasAsistenciasAsync()
        {
            try
            {
                var evaluaciones = await _asistenciaRepository.ObtenerTodasLasAsistenciasAsync();
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("get_asistencias_by_employee_name/{employee_name}")]
        public async Task<ActionResult<AsistenciasResponse>> ObtenerAsistenciaPorNombreEmpleadoAsync(string nom_emp)
        {
            try
            {
                var evaluaciones = await _asistenciaRepository.ObtenerAsistenciaPorNombreEmpleadoAsync(nom_emp);
                if (evaluaciones == null)
                {
                    return NotFound($"Empleado with name '{nom_emp}' not found.");
                }
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("get_asistencias_by_number_document/{num_doc}")]
        public async Task<ActionResult<AsistenciasResponse>> ObtenerAsistenciaPorNumeroDocumentoAsync(int num_doc)
        {
            try
            {
                var evaluaciones = await _asistenciaRepository.ObtenerAsistenciaPorNumeroDocumentoAsync(num_doc);
                if (evaluaciones == null)
                {
                    return NotFound($"Empleado with document number '{num_doc}' not found.");
                }
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_asistencias_by_date/{date}")]
        public async Task<ActionResult<AsistenciasResponse>> ObtenerAsistenciaPorFechaAsync(DateTime fecha)
        {
            try
            {
                var evaluaciones = await _asistenciaRepository.ObtenerAsistenciaPorFechaAsync(fecha);
                if (evaluaciones == null)
                {
                    return NotFound($"Empleado with date '{fecha}' not found.");
                }
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<EvaluacionInsertRequest>> InsertarAsistenciaAsync(AsistenciasInsert asistencias)
        {
            try
            {
                var resultado = await _asistenciaRepository.InsertarAsistenciaAsync(asistencias);
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
        public async Task<ActionResult<EvalucacionUpdateRequest>> ActualizarAsistenciaAsync(int id, AsistenciasUpdate asistencias)
        {
            try
            {
                var resultado = await _asistenciaRepository.ActualizarAsistenciaAsync(id, asistencias);
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
                var eliminado = await _asistenciaRepository.EliminarAsistenciaAsync(id);
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
