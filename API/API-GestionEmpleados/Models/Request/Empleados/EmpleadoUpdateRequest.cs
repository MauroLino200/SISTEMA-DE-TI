namespace API_GestionEmpleados.Models.Request.Empleados
{
    public class EmpleadoUpdateRequest
    {
        public int IdEmpleado { get; set; }
        public string? DNI { get; set; }
        public string? NombreCompleto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string? Turno { get; set; }
        public string? Correo { get; set; }
        public int IdCargo { get; set; }
        public int IdDepartamento { get; set; }

    }
}
