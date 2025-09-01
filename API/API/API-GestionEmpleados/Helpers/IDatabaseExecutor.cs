using Microsoft.Data.SqlClient;

namespace API_GestionEmpleados.Helpers
{
    public interface IDatabaseExecutor
    {
        Task<T> ExecuteCommand<T>(Func<SqlConnection, Task<T>> task);

    }
}
