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

        #region seleccion y filtros

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByIdAsync(int id_emp)
        {
            var sp = "USP_GET_ONE_EMPLEADO_BY_ID";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEmpleado", id_emp, System.Data.DbType.Int32);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByItsNumberDocumentAsync(string num_doc)
        {
            var sp = "USP_GET_ONE_EMPLEADO_BY_ITS_DOCUMENT_NUMBER";
            var parameters = new DynamicParameters();
            parameters.Add("@NumeroDocumento", num_doc, System.Data.DbType.String);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByNameAsync(string nom_emp)
        {
            var sp = "USP_GET_ONE_EMPLEADO_BY_NAME";
            var parameters = new DynamicParameters();
            parameters.Add("@NombreCompleto", nom_emp, System.Data.DbType.String);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByDateAsync(DateTime fecha_ing)
        {
            var sp = "USP_GET_ONE_EMPLEADO_BY_DATE";
            var parameters = new DynamicParameters();
            parameters.Add("@FechaIngreso", fecha_ing, System.Data.DbType.DateTime);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByTurnoAsync(string turno)
        {
            var sp = "USP_GET_EMPLEADOS_BY_TURNO";
            var parameters = new DynamicParameters();
            parameters.Add("@Turno", turno, System.Data.DbType.String);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByEmailAsync(string email)
        {
            var sp = "USP_GET_EMPLEADOS_BY_EMAIL";
            var parameters = new DynamicParameters();
            parameters.Add("@Correo", email, System.Data.DbType.String);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByCargoAsync(int cargo)
        {
            var sp = "USP_GET_EMPLEADOS_BY_CARGO";
            var parameters = new DynamicParameters();
            parameters.Add("@IdCargo", cargo, System.Data.DbType.Int64);

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

        public async Task<EmpleadoResponse> GetOneEmpleadoByOfficeAsync(int id_dep)
        {
            var sp = "USP_GET_EMPLEADOS_BY_OFFICE";
            var parameters = new DynamicParameters();
            parameters.Add("@IdDepartamento", id_dep, System.Data.DbType.Int64);

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

        #endregion

       
        public async Task<EmpleadoCreateRequest> AddEmpleadoAsync(EmpleadoCreateRequest request)
        {
            var sp = "USP_INSERT_EMPLEADO";
            var parameters = new DynamicParameters();
            parameters.Add("@IdTipoDocumento", request.IdTipoDocumento, DbType.Int32);
            parameters.Add("@NumeroDocumento", request.NumeroDocumento, DbType.Int32);
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
            parameters.Add("@IdTipoDocumento", request.IdTipoDocumento, DbType.Int32);
            parameters.Add("@NumeroDocumento", request.NumeroDocumento, DbType.Int32);
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
    }
}
