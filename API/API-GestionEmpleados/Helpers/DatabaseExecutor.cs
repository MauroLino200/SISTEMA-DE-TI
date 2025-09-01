﻿using Microsoft.Data.SqlClient;

namespace API_GestionEmpleados.Helpers
{
    public class DatabaseExecutor : IDatabaseExecutor
    {
        private readonly string _cadena;

        public DatabaseExecutor(string cadena)
        {
            _cadena = cadena;
        }
        public async Task<T> ExecuteCommand<T>(Func<SqlConnection, Task<T>> operation)
        {

            try
            {
                using var conexion = new SqlConnection(_cadena);
                return await operation(conexion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
