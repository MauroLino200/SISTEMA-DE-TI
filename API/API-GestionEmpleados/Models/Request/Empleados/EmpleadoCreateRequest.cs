using System.Net;

namespace API_GestionEmpleados.Models.Request.Empleados
{
    public class EmpleadoCreateRequest
    {
        public int IdTipoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public string? NombreCompleto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string? Turno { get; set; }
        public string? Correo { get; set; }
        public int IdCargo { get; set; }
        public string? Cargo { get; set; }

        public int IdDepartamento { get; set; }
        public string? Departamento { get; set; }

    }
}
