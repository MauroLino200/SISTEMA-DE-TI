using API_GestionEmpleados.Models.Request.Empleados;
using API_GestionEmpleados.Models.Response.Empleados;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoRepository _empleadosRepository;

        public EmpleadoController(IEmpleadoRepository empleadosRepository)
        {
            _empleadosRepository = empleadosRepository;
        }

        [HttpGet("action")]
        public async Task<ActionResult<IEnumerable<EmpleadoResponse>>> GetAllEmpleadosAsync()
        {
            try
            {
                var empleados = await _empleadosRepository.GetAllEmpleadosAsync();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("action/{id}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByIdAsync(int id)
        {
            try
            {
                var empleado = await _empleadosRepository.GetEmpleadoByIdAsync(id);
                if (empleado == null)
                {
                    return NotFound($"Empleado with ID {id} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost("action")]
        public async Task<ActionResult<EmpleadoCreateRequest>> AddEmpleadoAsync([FromBody] EmpleadoCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var newEmpleado = await _empleadosRepository.AddEmpleadoAsync(request);
                return newEmpleado;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating data: {ex.Message}");
            }
        }
        [HttpPut("action/{id}")]
        public async Task<ActionResult<string>> UpdateEmpleadoAsync(int id, [FromBody] EmpleadoUpdateRequest request)
        {
            if (request == null || id != request.IdEmpleado)
            {
                return BadRequest("Datos de solicitud inválidos.");
            }
            try
            {
                var result = await _empleadosRepository.UpdateEmpleadoAsync(id, request);
                if (string.IsNullOrEmpty(result))
                {
                    return NotFound($"Empleado con ID {id} no encontrado.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error actualizando datos: {ex.Message}");
            }
        }



    }
}
