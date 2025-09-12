using API_GestionEmpleados.Models.Response.Empleados;
using API_GestionEmpleados.Models.Response.Usuarios;
using API_GestionEmpleados.Repositories;
using API_GestionEmpleados.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public UsuariosController(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        #region seleccion y filtros

        [HttpGet("get_usuarios")]
        public async Task<ActionResult<IEnumerable<UsuariosResponse>>> GetAllUsuariosAsync()
        {
            try
            {
                var usuarios = await _usuariosRepository.GetAllUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        #endregion

    }
}
