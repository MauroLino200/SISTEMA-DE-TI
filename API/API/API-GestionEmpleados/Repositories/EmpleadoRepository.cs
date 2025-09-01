using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Models.Request.Empleados;
using API_GestionEmpleados.Models.Response.Empleados;
using API_GestionEmpleados.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace API_GestionEmpleados.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly IDatabaseExecutor _executor;

        public EmpleadoRepository(IDatabaseExecutor executor)
        {
            _executor = executor;
        }

        public async Task<IEnumerable<EmpleadoResponse>> GetAllEmpleadosAsync()
        {
            var sp = "USP_SELECT_EMPLEADOS";
            try
            {
                var listado = await _executor.ExecuteCommand(conexion => conexion.QueryAsync<EmpleadoResponse>(sp));
                return listado;
            
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            }

        public async Task<EmpleadoResponse> GetEmpleadoByIdAsync(int id)
        {
            var sp = "USP_GET_ONE_EMPLEADO_BY_ID";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", id, System.Data.DbType.Int32);

            try
            {
                var registro = await _executor.ExecuteCommand(conexion => conexion.QueryFirstOrDefaultAsync<EmpleadoResponse>(sp, parameters, commandType: CommandType.StoredProcedure));
                return registro;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EmpleadoCreateRequest> AddEmpleadoAsync(EmpleadoCreateRequest request)
        {
            var sp = "USP_INSERT_EMPLEADO";
            var parameters = new DynamicParameters();
            parameters.Add("@DNI", request.DNI, DbType.String);
            parameters.Add("@NombreCompleto", request.NombreCompleto, DbType.String);
            parameters.Add("@FechaIngreso", request.FechaIngreso, DbType.DateTime);
            parameters.Add("@Turno", request.Turno, DbType.String);
            parameters.Add("@Correo", request.Correo, DbType.String);
            parameters.Add("@IdCargo", request.IdCargo, DbType.Int32);
            parameters.Add("@IdDepartamento", request.IdDepartamento, DbType.Int32);

            try
            {

                var result = await _executor.ExecuteCommand(conexion => conexion.QueryFirstOrDefaultAsync<EmpleadoCreateRequest>(sp, parameters, commandType: CommandType.StoredProcedure));
                return result;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

       
        public async Task<string> UpdateEmpleadoAsync(int id, EmpleadoUpdateRequest request)
        {
            var sp = "USP_UPDATE_EMPLEADO";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", id, DbType.Int32);
            parameters.Add("@DNI", request.DNI, DbType.String);
            parameters.Add("@NombreCompleto", request.NombreCompleto, DbType.String);
            parameters.Add("@FechaIngreso", request.FechaIngreso, DbType.DateTime);
            parameters.Add("@Turno", request.Turno, DbType.String);
            parameters.Add("@Correo", request.Correo, DbType.String);
            parameters.Add("@IdCargo", request.IdCargo, DbType.Int32);
            parameters.Add("@IdDepartamento", request.IdDepartamento, DbType.Int32);

            try
            {
                await _executor.ExecuteCommand(conexion => conexion.ExecuteAsync(sp, parameters, commandType: CommandType.StoredProcedure));
                return $"El empleado  número {id} ha sido actualizado correctamente";

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        // Uncomment the following methods if you need them in your repository
    }
}
