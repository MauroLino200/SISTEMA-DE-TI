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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevolucionesResponse>>> GetAll()
        {
            var result = await _repo.ObtenerTodosAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetById(int id)
        {
            var result = await _repo.ObtenerPorIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Insert([FromBody] DevolucionesInsertRequest request)
        {
            var id = await _repo.InsertarAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
