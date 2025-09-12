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


        #region seleccion y filtros

        [HttpGet("get_empleados")]
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

        [HttpGet("get_empleado_by_i/{id_empleado}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByIdAsync(int id)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByIdAsync(id);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'ID':{id} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_n/{num_doc}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByItsNumberDocumentAsync(string num_doc)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByItsNumberDocumentAsync(num_doc);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'num':{num_doc} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_na/{nom_emp}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByNameAsync(string nom_emp)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByNameAsync(nom_emp);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'name':{nom_emp} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_fi/{fecha_ing}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByDateAsync(DateTime fecha_ing)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByDateAsync(fecha_ing);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'datetime':{fecha_ing} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_t/{turno}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByTurnoAsync(string turno)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByTurnoAsync(turno);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'turno':{turno} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_em/{email}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByEmailAsync(string email)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByEmailAsync(email);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'email':{email} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_cg/{cargo}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByCargoAsync(int cargo)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByCargoAsync(cargo);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'cargo':{cargo} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("get_empleado_by_dep/{departamento}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoByOffice(int id_dep)
        {
            try
            {
                var empleado = await _empleadosRepository.GetOneEmpleadoByOfficeAsync(id_dep);
                if (empleado == null)
                {
                    return NotFound($"Empleado with 'departamento':{id_dep} not found.");
                }
                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }




        #endregion

        [HttpPost("insrt_emp")]
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
      
        
        [HttpPut("updt_emp/{id}")]
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
