using API_GestionEmpleados.Models.Request.EquiposdeTrabajo;
using API_GestionEmpleados.Models.Response.EquiposdeTrabajo;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipodeTrabajoController : ControllerBase
    {
        private readonly IEquipoTrabajoRepository _repo;

        public EquipodeTrabajoController(IEquipoTrabajoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipoTrabajoResponse>>> GetAll()
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
        public async Task<ActionResult<int>> Insert([FromBody] EquipoTrabajoInsertRequest request)
        {
            var id = await _repo.InsertarAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EquipoTrabajoUpdateRequest request)
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
