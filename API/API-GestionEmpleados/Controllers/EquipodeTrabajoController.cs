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

        #region Filtrados y selecciones

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipoTrabajoResponse>>> GetAll()
        {
            var result = await _repo.ObtenerTodosAsync();
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetById(int id)
        {
            var result = await _repo.ObtenerPorIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("name/{nom_equipo}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetByName(string nom_equipo)
        {
            var result = await _repo.ObtenerPorNombreAsync(nom_equipo);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("type/{tipo_equipo}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetByTypeOfThing(string tipo_equipo)
        {
            var result = await _repo.ObtenerPorTipoEquipoAsync(tipo_equipo);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("status/{estado}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetByStatus(string estado)
        {
            var result = await _repo.ObtenerPorEstadoAsync(estado);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("date/{fecha}")]
        public async Task<ActionResult<EquipoTrabajoResponse>> GetByDate(DateTime fecha)
        {
            var result = await _repo.ObtenerPorFechaAsync(fecha);
            if (result == null) return NotFound();
            return Ok(result);
        }

      

        #endregion

        #region CRUD

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

        #endregion

    }
}
