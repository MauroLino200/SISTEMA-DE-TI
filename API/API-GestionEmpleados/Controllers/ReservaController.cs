using API_GestionEmpleados.Models.Request.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Request.Reserva;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.Reserva;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaRepository _repo;

        public ReservaController(IReservaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaResponse>>> GetAll()
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
        public async Task<ActionResult<int>> Insert([FromBody] ReservaInsertRequest request)
        {
            var id = await _repo.InsertarAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ReservaUpdateRequest request)
        {
            var updated = await _repo.ActualizarAsync(id, request);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _repo.EliminarAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
