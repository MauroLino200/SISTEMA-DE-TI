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

        #region selection and filters

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaResponse>>> GetAll()
        {
            var result = await _repo.ObtenerTodosAsync();
            return Ok(result);
        }


        [HttpGet("id_reserva/{id_reserva}")]
        public async Task<ActionResult<ReservaResponse>> GetById(int id)
        {
            var result = await _repo.ObtenerPorIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("name_of_thing/{nombre_equipo}")]
        public async Task<ActionResult<ReservaResponse>> GetByNameOfThing(string nombre)
        {
            var result = await _repo.ObtenerPorNombreAsync(nombre);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("id_model/{id_modelo}")]
        public async Task<ActionResult<ReservaResponse>> GetByNameOfModel(int modelo)
        {
            var result = await _repo.ObtenerPorModeloAsync(modelo);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("id_employee/{id_empleado}")]
        public async Task<ActionResult<ReservaResponse>> GetByIdOfEmployee(int id_empleado)
        {
            var result = await _repo.ObtenerPorIdEmpleadoAsync(id_empleado);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("date/{fecha_reserva}")]
        public async Task<ActionResult<ReservaResponse>> GetByDate(DateTime fecha_reserva)
        {
            var result = await _repo.ObtenerPorFechaReservaAsync(fecha_reserva);
            if (result == null) return NotFound();
            return Ok(result);
        }
        #endregion


        #region insert, update, delete

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

        #endregion


    }
}
