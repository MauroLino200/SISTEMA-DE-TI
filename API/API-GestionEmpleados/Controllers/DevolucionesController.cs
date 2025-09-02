using API_GestionEmpleados.Models.Request.Devoluciones;
using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.Devoluciones;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.Reserva;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionesController : ControllerBase
    {
        private readonly IDevolucionesRepository _repo;

        public DevolucionesController(IDevolucionesRepository repo)
        {
            _repo = repo;
        }

        #region seleccionables y filtros

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevolucionesResponse>>> GetAll()
        {
            var result = await _repo.ObtenerTodosAsync();
            return Ok(result);
        }


        [HttpGet("id_devoluciones/{id_devoluciones}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetById_Devoluciones(int id)
        {
            var result = await _repo.ObtenerPorIdDevolucionAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("nombre_equipo/{nombre_equipo}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetByNombre_Equipo(string nombre_equipo)
        {
            var result = await _repo.ObtenerPorNombreEquipoAsync(nombre_equipo);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("id_modelo/{id_modelo}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetById_Modelo(int id_modelo)
        {
            var result = await _repo.ObtenerPorIdModeloAsync(id_modelo);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("id_empleado/{id_empleado}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetById_Empleado(int id_empleado)
        {
            var result = await _repo.ObtenerPorIdEmpleadoAsync(id_empleado);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("fecha_devolucion/{fecha_devolucion}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetById_Devoluciones(DateTime fecha_devolucion)
        {
            var result = await _repo.ObtenerPorFechaAsync(fecha_devolucion);
            if (result == null) return NotFound();
            return Ok(result);
        }
        #endregion


        [HttpPost]
        public async Task<ActionResult<int>> Insert([FromBody] DevolucionesInsertRequest request)
        {
            var id = await _repo.InsertarAsync(request);
            return CreatedAtAction(nameof(GetById_Devoluciones), new { id }, id);
        }
    }
}
