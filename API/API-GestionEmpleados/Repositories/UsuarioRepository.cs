using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Response.Usuarios;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;

namespace API_GestionEmpleados.Repositories
{
    public class UsuarioRepository : IUsuariosRepository
    {
        private readonly IDatabaseExecutor _executor;
        
        public UsuarioRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IEnumerable<UsuariosResponse>> GetAllUsuariosAsync()
        {
            var sp = "USP_GET_ALL_USUARIOS";

            try
            {
                var listado = await _executor.ExecuteCommand(conexion => conexion.QueryAsync<UsuariosResponse>(sp));
                return listado;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
