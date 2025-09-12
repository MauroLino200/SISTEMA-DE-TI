using API_GestionEmpleados.Models.Response.Usuarios;

namespace API_GestionEmpleados.Repositories.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<IEnumerable<UsuariosResponse>> GetAllUsuariosAsync();

    }
}
